using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BibliotecaMusicalBack.Infrastructure.Repositories;

public class GeneroRepository : IGeneroRepository
{
    private readonly string _connectionString;

    public GeneroRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("BibliotecaMusical")
            ?? throw new Exception("No existe la cadena de conexión BibliotecaMusical.");
    }

    public async Task<List<Genero>> ObtenerGenerosAsync()
    {
        var lista = new List<Genero>();

        await using var cn = new NpgsqlConnection(_connectionString);
        await cn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM sp_obtener_generos()", cn);

        await using var dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new Genero
            {
                GenCodigo = dr.GetInt32(dr.GetOrdinal("gen_codigo")),
                GenNombre = dr.GetString(dr.GetOrdinal("gen_nombre")),
                GenActivo = dr.GetBoolean(dr.GetOrdinal("gen_activo"))
            });
        }

        return lista;
    }
}