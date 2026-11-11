using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public interface IMascotaRepository
{
    Task<List<Mascota>> ObtenerTodasAsync(string? especie, string? nombre, int? propietarioId, decimal? pesoMinimo);
    Task<List<Mascota>> ObtenerPorPropietarioAsync(int propietarioId);
    Task<Mascota?> ObtenerPorIdAsync(int id);
    Task<int> ContarPorPropietarioAsync(int propietarioId);
    Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId = null);
    Task AgregarAsync(Mascota mascota);
    Task ActualizarAsync(Mascota mascota);
    Task EliminarAsync(Mascota mascota);
}
