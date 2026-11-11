using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly IMascotaService _mascotas;

    public MascotasController(IMascotaService mascotas)
    {
        _mascotas = mascotas;
    }

    /// <summary>
    /// Lista mascotas. Filtros opcionales por query string:
    /// especie (exacta), nombre (contiene), propietarioId y pesoMinimo (kg).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MascotaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MascotaDto>>> Listar(
        [FromQuery] string? especie,
        [FromQuery] string? nombre,
        [FromQuery] int? propietarioId,
        [FromQuery] decimal? pesoMinimo)
    {
        return Ok(await _mascotas.ObtenerTodasAsync(especie, nombre, propietarioId, pesoMinimo));
    }

    /// <summary>Obtiene una mascota por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MascotaDto>> ObtenerPorId(int id)
    {
        var mascota = await _mascotas.ObtenerPorIdAsync(id);
        return mascota is null ? NotFound() : Ok(mascota);
    }

    /// <summary>Registra una mascota para un propietario existente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(MascotaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MascotaDto>> Crear(MascotaCrearDto dto)
    {
        try
        {
            var creada = await _mascotas.CrearAsync(dto);
            if (creada is null)
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Propietario no encontrado",
                    Detail = $"No existe un propietario con id {dto.PropietarioId}."
                });

            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.MascotaId }, creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    /// <summary>Actualiza una mascota existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, MascotaActualizarDto dto)
    {
        try
        {
            var existe = await _mascotas.ActualizarAsync(id, dto);
            return existe ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    /// <summary>Elimina una mascota.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existe = await _mascotas.EliminarAsync(id);
        return existe ? NoContent() : NotFound();
    }

    private ActionResult Conflicto(ReglaNegocioException ex) =>
        Conflict(new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Regla de negocio incumplida",
            Detail = ex.Message
        });
}
