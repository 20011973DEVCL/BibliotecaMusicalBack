using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

using BibliotecaMusicalBack.Domain.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace BibliotecaMusicalBack.Infrastructure.Repositories;

public class CrudRepository<TEntity>(IConfiguration configuration) : ICrudRepository<TEntity>
    where TEntity : class, new()
{
    private static readonly EntityMetadata Metadata = EntityMetadata.Create(typeof(TEntity));

    private readonly string _connectionString = configuration.GetConnectionString("BibliotecaMusical")
        ?? throw new InvalidOperationException(
            "No existe la cadena de conexión 'BibliotecaMusical'. Configúrela mediante User Secrets o variables de entorno.");

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<TEntity> entities = [];
        string sql = $"SELECT {Metadata.ColumnList} FROM {Metadata.QuotedTable} ORDER BY {Metadata.QuotedKey}";

        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new(sql, connection);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            entities.Add(Map(reader));
        }

        return entities;
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        string sql =
            $"SELECT {Metadata.ColumnList} FROM {Metadata.QuotedTable} WHERE {Metadata.QuotedKey} = @id";

        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("id", id);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        string columnNames = string.Join(", ", Metadata.WritableProperties.Select(property => property.QuotedColumn));
        string parameterNames = string.Join(", ", Metadata.WritableProperties.Select(property => $"@{property.Property.Name}"));
        string sql =
            $"INSERT INTO {Metadata.QuotedTable} ({columnNames}) VALUES ({parameterNames}) " +
            $"RETURNING {Metadata.ColumnList}";

        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new(sql, connection);
        AddParameters(command, entity, Metadata.WritableProperties);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
        return Map(reader);
    }

    public async Task<bool> UpdateAsync(
        int id,
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        string assignments = string.Join(
            ", ",
            Metadata.WritableProperties.Select(property => $"{property.QuotedColumn} = @{property.Property.Name}"));
        string sql =
            $"UPDATE {Metadata.QuotedTable} SET {assignments} WHERE {Metadata.QuotedKey} = @id";

        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new(sql, connection);
        AddParameters(command, entity, Metadata.WritableProperties);
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        string sql = $"DELETE FROM {Metadata.QuotedTable} WHERE {Metadata.QuotedKey} = @id";

        await using NpgsqlConnection connection = new(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("id", id);
        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AddParameters(
        NpgsqlCommand command,
        TEntity entity,
        IReadOnlyList<PropertyMetadata> properties)
    {
        foreach (PropertyMetadata property in properties)
        {
            object value = property.Property.GetValue(entity) ?? DBNull.Value;
            command.Parameters.AddWithValue(property.Property.Name, value);
        }
    }

    private static TEntity Map(IDataRecord record)
    {
        TEntity entity = new();

        foreach (PropertyMetadata property in Metadata.Properties)
        {
            int ordinal = record.GetOrdinal(property.Column);
            if (record.IsDBNull(ordinal))
            {
                property.Property.SetValue(entity, null);
                continue;
            }

            object value = record.GetValue(ordinal);
            Type targetType = Nullable.GetUnderlyingType(property.Property.PropertyType)
                ?? property.Property.PropertyType;

            if (targetType == typeof(DateOnly) && value is DateTime dateTime)
            {
                value = DateOnly.FromDateTime(dateTime);
            }
            else if (!targetType.IsInstanceOfType(value))
            {
                value = Convert.ChangeType(value, targetType);
            }

            property.Property.SetValue(entity, value);
        }

        return entity;
    }

    private sealed class EntityMetadata
    {
        private EntityMetadata(
            string table,
            PropertyMetadata key,
            IReadOnlyList<PropertyMetadata> properties,
            IReadOnlyList<PropertyMetadata> writableProperties)
        {
            QuotedTable = Quote(table);
            Key = key;
            Properties = properties;
            WritableProperties = writableProperties;
            QuotedKey = key.QuotedColumn;
            ColumnList = string.Join(", ", properties.Select(property => property.QuotedColumn));
        }

        public string QuotedTable { get; }
        public string QuotedKey { get; }
        public string ColumnList { get; }
        public PropertyMetadata Key { get; }
        public IReadOnlyList<PropertyMetadata> Properties { get; }
        public IReadOnlyList<PropertyMetadata> WritableProperties { get; }

        public static EntityMetadata Create(Type entityType)
        {
            string table = entityType.GetCustomAttribute<TableAttribute>()?.Name
                ?? throw new InvalidOperationException($"{entityType.Name} no define TableAttribute.");

            List<PropertyMetadata> properties = [.. entityType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(property => property.CanRead && property.CanWrite)
                .Select(property =>
                {
                    DatabaseGeneratedOption generatedOption =
                        property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption
                        ?? DatabaseGeneratedOption.None;

                    return new PropertyMetadata(
                        property,
                        property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name,
                        property.GetCustomAttribute<KeyAttribute>() is not null,
                        generatedOption != DatabaseGeneratedOption.None);
                })];

            PropertyMetadata key = properties.Single(property => property.IsKey);
            List<PropertyMetadata> writable = [.. properties.Where(property => !property.IsKey && !property.IsGenerated)];
            return new EntityMetadata(table, key, properties, writable);
        }

        private static string Quote(string identifier)
        {
            return $"\"{identifier.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
        }
    }

    private sealed class PropertyMetadata(
        PropertyInfo property,
        string column,
        bool isKey,
        bool isGenerated)
    {
        public PropertyInfo Property { get; } = property;
        public string Column { get; } = column;
        public string QuotedColumn { get; } =
            $"\"{column.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
        public bool IsKey { get; } = isKey;
        public bool IsGenerated { get; } = isGenerated;
    }
}
