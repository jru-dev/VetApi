using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IPropietarioRepository
{
    Task<List<Propietario>> ObtenerTodosAsync(string? apellido, string? dni);
    Task<Propietario?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteDniAsync(string dni, int? excluirId = null);
    Task AgregarAsync(Propietario propietario);
    Task ActualizarAsync(Propietario propietario);
    Task EliminarAsync(Propietario propietario);
}
