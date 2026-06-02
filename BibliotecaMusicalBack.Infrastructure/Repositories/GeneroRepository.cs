using BibliotecaMusicalBack.Domain.Entities;
using BibliotecaMusicalBack.Domain.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace BibliotecaMusicalBack.Infrastructure.Repositories;

public class GeneroRepository(IConfiguration configuration) : IGeneroRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("BibliotecaMusical")
            ?? throw new Exception("No existe la cadena de conexión BibliotecaMusical.");

    public async Task<List<Genero>> ObtenerGenerosAsync()
    {
        List<Genero> lista = [];

        await using NpgsqlConnection cn = new(_connectionString);
        await cn.OpenAsync();

        await using NpgsqlCommand cmd = new("SELECT * FROM sp_obtener_generos()", cn);

        await using NpgsqlDataReader dr = await cmd.ExecuteReaderAsync();

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