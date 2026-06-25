using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BibliotecaMusicalBack.Api.Controllers;

internal static class ResourceKeyReader
{
    public static int GetId<TDto>(TDto dto)
        where TDto : class
    {
        PropertyInfo key = dto.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Single(property => property.GetCustomAttribute<KeyAttribute>() is not null);

        return (int)(key.GetValue(dto)
            ?? throw new InvalidOperationException($"El recurso {dto.GetType().Name} no contiene una clave."));
    }
}
