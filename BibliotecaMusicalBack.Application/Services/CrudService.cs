using System.Reflection;

using BibliotecaMusicalBack.Application.Interfaces;
using BibliotecaMusicalBack.Domain.Interfaces;

namespace BibliotecaMusicalBack.Application.Services;

public class CrudService<TEntity, TDto>(ICrudRepository<TEntity> repository) : ICrudService<TDto>
    where TEntity : class, new()
    where TDto : class, new()
{
    private readonly ICrudRepository<TEntity> _repository = repository;

    public async Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TEntity> entities = await _repository.GetAllAsync(cancellationToken);
        return [.. entities.Select(ObjectMapper.Map<TEntity, TDto>)];
    }

    public async Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : ObjectMapper.Map<TEntity, TDto>(entity);
    }

    public async Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity entity = ObjectMapper.Map<TDto, TEntity>(dto);
        TEntity created = await _repository.CreateAsync(entity, cancellationToken);
        return ObjectMapper.Map<TEntity, TDto>(created);
    }

    public Task<bool> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity entity = ObjectMapper.Map<TDto, TEntity>(dto);
        return _repository.UpdateAsync(id, entity, cancellationToken);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteAsync(id, cancellationToken);
    }
}

internal static class ObjectMapper
{
    public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : class, new()
    {
        TDestination destination = new();
        Dictionary<string, PropertyInfo> destinationProperties = typeof(TDestination)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanWrite)
            .ToDictionary(property => property.Name);

        foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!sourceProperty.CanRead ||
                !destinationProperties.TryGetValue(sourceProperty.Name, out PropertyInfo? destinationProperty) ||
                !destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
            {
                continue;
            }

            destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
        }

        return destination;
    }
}
