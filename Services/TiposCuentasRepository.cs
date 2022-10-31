using System.Data.SqlClient;
using Dapper;
using ManejoPresupuesto.Interfaces;
using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Services;

public class TiposCuentasRepository : ITIposCuentasRepository
{
    private readonly string _connectionString;
    
    public TiposCuentasRepository(IConfiguration _configuration)
    {
        _connectionString = _configuration.GetConnectionString("defaultConnection");
    }

    public async Task Crear(TipoCuenta tipoCuenta)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var id = await connection.QuerySingleAsync<int>(
            $@"INSERT INTO TiposCuentas(Nombre, UsuarioId, Orden) 
                  VALUES(@Nombre, @UsuarioId, 0);
                  SELECT SCOPE_IDENTITY()", tipoCuenta); //SELECT SCOPE_IDENTITY nos regresa el id del registro creado
        
        tipoCuenta.Id = id;
    }

    public async Task<bool> Exists(string nombre, int usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        var exists = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas WHERE Nombre = @nombre AND UsuarioId = @usuarioId;", 
                                                                    new { nombre, usuarioId });
        return exists == 1;
    }
    
    public async Task<IEnumerable<TipoCuenta>> Get(int usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TipoCuenta>(@"SELECT * FROM TiposCuentas WHERE UsuarioId = @UsuarioId", new { usuarioId });
    }

    public async Task Update(TipoCuenta tipoCuenta)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("UPDATE TiposCuentas SET Nombre = @Nombre WHERE Id = @Id", tipoCuenta); //Execute es un query que no retorna algo
    }

    public async Task<TipoCuenta> GetById(int id, int usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(
            "SELECT * FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
    }

    public async Task Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id = @Id", new { id });
    }
}