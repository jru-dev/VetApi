using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IMascotaService
{
    Task<List<MascotaDto>> ObtenerTodasAsync(string? especie, string? nombre, int? propietarioId, decimal? pesoMinimo);
    /// <returns>null si el propietario no existe.</returns>
    Task<List<MascotaDto>?> ObtenerPorPropietarioAsync(int propietarioId);
    Task<MascotaDto?> ObtenerPorIdAsync(int id);
    /// <returns>null si el propietario indicado no existe.</returns>
    Task<MascotaDto?> CrearAsync(MascotaCrearDto dto);
    /// <returns>false si la mascota no existe.</returns>
    Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto);
    /// <returns>false si la mascota no existe.</returns>
    Task<bool> EliminarAsync(int id);
}
