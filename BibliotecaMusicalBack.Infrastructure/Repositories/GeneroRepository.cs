using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;
using Npgsql;

namespace BibliotecaMusicalBack.Infrastructure.Repositories;

public sealed class GeneroRepository(NpgsqlDataSource dataSource) : IGeneroRepository
{
    public async Task<List<Genero>> ObtenerGenerosAsync(bool soloActivos, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT gen_codigo, gen_nombre, gen_descripcion, gen_activo
            FROM generos
            WHERE (NOT @solo_activos OR gen_activo)
            ORDER BY gen_nombre
            """;
        await using NpgsqlCommand cmd = dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("solo_activos", soloActivos);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
        List<Genero> resultado = [];
        while (await reader.ReadAsync(cancellationToken)) resultado.Add(Mapear(reader));
        return resultado;
    }

    public async Task<Genero?> ObtenerPorCodigoAsync(int codigo, CancellationToken cancellationToken)
    {
        await using NpgsqlCommand cmd = dataSource.CreateCommand("SELECT gen_codigo, gen_nombre, gen_descripcion, gen_activo FROM generos WHERE gen_codigo = @codigo");
        cmd.Parameters.AddWithValue("codigo", codigo);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Mapear(reader) : null;
    }

    public async Task<Genero> CrearAsync(Genero genero, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO generos (gen_nombre, gen_descripcion, gen_activo) VALUES (@nombre, @descripcion, @activo) RETURNING gen_codigo, gen_nombre, gen_descripcion, gen_activo";
        await using NpgsqlCommand cmd = dataSource.CreateCommand(sql);
        AgregarParametros(cmd, genero);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);
        return Mapear(reader);
    }

    public async Task<Genero?> ActualizarAsync(Genero genero, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE generos SET gen_nombre=@nombre, gen_descripcion=@descripcion, gen_activo=@activo WHERE gen_codigo=@codigo RETURNING gen_codigo, gen_nombre, gen_descripcion, gen_activo";
        await using NpgsqlCommand cmd = dataSource.CreateCommand(sql);
        AgregarParametros(cmd, genero);
        cmd.Parameters.AddWithValue("codigo", genero.GenCodigo);
        await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
        return await reader.ReadAsync(cancellationToken) ? Mapear(reader) : null;
    }

    public async Task<bool> EliminarAsync(int codigo, CancellationToken cancellationToken)
    {
        await using NpgsqlCommand cmd = dataSource.CreateCommand("UPDATE generos SET gen_activo = FALSE WHERE gen_codigo = @codigo AND gen_activo");
        cmd.Parameters.AddWithValue("codigo", codigo);
        return await cmd.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AgregarParametros(NpgsqlCommand cmd, Genero genero)
    {
        cmd.Parameters.AddWithValue("nombre", genero.GenNombre);
        cmd.Parameters.AddWithValue("descripcion", (object?)genero.GenDescripcion ?? DBNull.Value);
        cmd.Parameters.AddWithValue("activo", genero.GenActivo);
    }

    private static Genero Mapear(NpgsqlDataReader reader) => new()
    {
        GenCodigo = reader.GetInt32(0),
        GenNombre = reader.GetString(1),
        GenDescripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
        GenActivo = reader.GetBoolean(3)
    };
}
