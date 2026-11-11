using AutoMapper;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;

namespace Veterinaria.Negocio.Servicios;

public class MascotaService : IMascotaService
{
    private const int MaximoMascotasPorPropietario = 5;

    private readonly IMascotaRepository _mascotas;
    private readonly IPropietarioRepository _propietarios;
    private readonly IMapper _mapper;

    public MascotaService(IMascotaRepository mascotas, IPropietarioRepository propietarios, IMapper mapper)
    {
        _mascotas = mascotas;
        _propietarios = propietarios;
        _mapper = mapper;
    }

    public async Task<List<MascotaDto>> ObtenerTodasAsync(string? especie, string? nombre, int? propietarioId, decimal? pesoMinimo)
    {
        var lista = await _mascotas.ObtenerTodasAsync(especie, nombre, propietarioId, pesoMinimo);
        return _mapper.Map<List<MascotaDto>>(lista);
    }

    public async Task<List<MascotaDto>?> ObtenerPorPropietarioAsync(int propietarioId)
    {
        if (await _propietarios.ObtenerPorIdAsync(propietarioId) is null)
            return null;

        var lista = await _mascotas.ObtenerPorPropietarioAsync(propietarioId);
        return _mapper.Map<List<MascotaDto>>(lista);
    }

    public async Task<MascotaDto?> ObtenerPorIdAsync(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        return mascota is null ? null : _mapper.Map<MascotaDto>(mascota);
    }

    public async Task<MascotaDto?> CrearAsync(MascotaCrearDto dto)
    {
        if (await _propietarios.ObtenerPorIdAsync(dto.PropietarioId) is null)
            return null;

        // Regla de negocio 3: máximo 5 mascotas por propietario.
        if (await _mascotas.ContarPorPropietarioAsync(dto.PropietarioId) >= MaximoMascotasPorPropietario)
            throw new ReglaNegocioException(
                $"El propietario ya tiene {MaximoMascotasPorPropietario} mascotas, que es el máximo permitido.");

        ValidarFechaNacimiento(dto.FechaNacimiento);
        await ValidarMicrochipAsync(dto.Microchip, null);

        var mascota = _mapper.Map<Mascota>(dto);           // DTO -> entidad
        await _mascotas.AgregarAsync(mascota);

        // Se vuelve a leer con el propietario incluido para poder armar NombrePropietario.
        var guardada = await _mascotas.ObtenerPorIdAsync(mascota.MascotaId);
        return _mapper.Map<MascotaDto>(guardada);          // entidad -> DTO
    }

    public async Task<bool> ActualizarAsync(int id, MascotaActualizarDto dto)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota is null)
            return false;

        ValidarFechaNacimiento(dto.FechaNacimiento);
        await ValidarMicrochipAsync(dto.Microchip, id);

        _mapper.Map(dto, mascota);                         // actualiza el objeto existente
        await _mascotas.ActualizarAsync(mascota);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        if (mascota is null)
            return false;

        await _mascotas.EliminarAsync(mascota);
        return true;
    }

    // Regla de negocio 4: la fecha de nacimiento no puede ser futura.
    private static void ValidarFechaNacimiento(DateOnly? fecha)
    {
        if (fecha is { } f && f > DateOnly.FromDateTime(DateTime.Today))
            throw new ReglaNegocioException("La fecha de nacimiento de la mascota no puede ser futura.");
    }

    // Regla de negocio 5: el microchip no puede repetirse.
    private async Task ValidarMicrochipAsync(string? microchip, int? excluirId)
    {
        if (!string.IsNullOrWhiteSpace(microchip) && await _mascotas.ExisteMicrochipAsync(microchip, excluirId))
            throw new ReglaNegocioException($"El microchip {microchip} ya está asignado a otra mascota.");
    }
}
