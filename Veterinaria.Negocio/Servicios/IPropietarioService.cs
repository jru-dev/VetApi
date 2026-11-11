using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Servicios;

public interface IPropietarioService
{
    Task<List<PropietarioDto>> ObtenerTodosAsync(string? apellido, string? dni);
    Task<PropietarioDto?> ObtenerPorIdAsync(int id);
    Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto);
    /// <returns>false si el propietario no existe.</returns>
    Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto);
    /// <returns>false si el propietario no existe.</returns>
    Task<bool> EliminarAsync(int id);
}
