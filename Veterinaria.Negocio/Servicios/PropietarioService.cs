using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class PropietarioService : IPropietarioService
{
    private readonly IPropietarioRepository _repositorio;
    private readonly IMapper _mapper;

    public PropietarioService(IPropietarioRepository repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<List<PropietarioDto>> ObtenerTodosAsync(string? apellido, string? dni)
    {
        var propietarios = await _repositorio.ObtenerTodosAsync(apellido, dni);
        return _mapper.Map<List<PropietarioDto>>(propietarios);
    }

    public async Task<PropietarioDto?> ObtenerPorIdAsync(int id)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        return propietario is null ? null : _mapper.Map<PropietarioDto>(propietario);
    }

    public async Task<PropietarioDto> CrearAsync(PropietarioCrearDto dto)
    {
        // Regla de negocio 1: el DNI no puede repetirse.
        if (await _repositorio.ExisteDniAsync(dto.Dni))
            throw new ReglaNegocioException($"Ya existe un propietario registrado con el DNI {dto.Dni}.");

        var propietario = _mapper.Map<Propietario>(dto);   // DTO -> entidad
        await _repositorio.AgregarAsync(propietario);

        return _mapper.Map<PropietarioDto>(propietario);   // entidad -> DTO
    }

    public async Task<bool> ActualizarAsync(int id, PropietarioActualizarDto dto)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        if (propietario is null)
            return false;

        // Regla de negocio 1 (también al actualizar): el DNI no puede pertenecer a otro propietario.
        if (await _repositorio.ExisteDniAsync(dto.Dni, id))
            throw new ReglaNegocioException($"El DNI {dto.Dni} ya pertenece a otro propietario.");

        _mapper.Map(dto, propietario);                     // actualiza el objeto existente
        await _repositorio.ActualizarAsync(propietario);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var propietario = await _repositorio.ObtenerPorIdAsync(id);
        if (propietario is null)
            return false;

        // Regla de negocio 2: no se elimina un propietario que aún tiene mascotas registradas.
        if (propietario.Mascotas.Any())
            throw new ReglaNegocioException(
                $"No se puede eliminar al propietario porque tiene {propietario.Mascotas.Count} mascota(s) registrada(s).");

        await _repositorio.EliminarAsync(propietario);
        return true;
    }
}
