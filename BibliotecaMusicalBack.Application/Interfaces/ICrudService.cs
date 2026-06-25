namespace BibliotecaMusicalBack.Application.Interfaces;

public interface ICrudService<TDto>
    where TDto : class, new()
{
    Task<IReadOnlyList<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TDto> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
