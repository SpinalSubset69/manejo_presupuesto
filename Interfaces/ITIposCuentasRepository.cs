using ManejoPresupuesto.Models;

namespace ManejoPresupuesto.Interfaces;

public interface ITIposCuentasRepository
{
    Task Crear(TipoCuenta tipoCuenta);
    Task<bool> Exists(string nombre, int usuarioId);
    Task<IEnumerable<TipoCuenta>> Get(int usuarioId);
    Task Update(TipoCuenta tipoCuenta);
    Task<TipoCuenta> GetById(int id, int userId);
    Task Delete(int id);
}