using Microsoft.AspNetCore.Mvc;
using Veterinaria.Entidades.Dtos;
using Veterinaria.Negocio.Excepciones;
using Veterinaria.Negocio.Servicios;

namespace Veterinaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropietariosController : ControllerBase
{
    private readonly IPropietarioService _propietarios;
    private readonly IMascotaService _mascotas;

    public PropietariosController(IPropietarioService propietarios, IMascotaService mascotas)
    {
        _propietarios = propietarios;
        _mascotas = mascotas;
    }

    /// <summary>Lista propietarios. Filtros opcionales por query string: apellido (contiene) y dni (exacto).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PropietarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PropietarioDto>>> Listar([FromQuery] string? apellido, [FromQuery] string? dni)
    {
        return Ok(await _propietarios.ObtenerTodosAsync(apellido, dni));
    }

    /// <summary>Obtiene un propietario por su id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PropietarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PropietarioDto>> ObtenerPorId(int id)
    {
        var propietario = await _propietarios.ObtenerPorIdAsync(id);
        return propietario is null ? NotFound() : Ok(propietario);
    }

    /// <summary>Ruta anidada: lista las mascotas de un propietario.</summary>
    [HttpGet("{id:int}/mascotas")]
    [ProducesResponseType(typeof(IEnumerable<MascotaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<MascotaDto>>> ListarMascotas(int id)
    {
        var mascotas = await _mascotas.ObtenerPorPropietarioAsync(id);
        return mascotas is null ? NotFound() : Ok(mascotas);
    }

    /// <summary>Registra un propietario.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PropietarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PropietarioDto>> Crear(PropietarioCrearDto dto)
    {
        try
        {
            var creado = await _propietarios.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.PropietarioId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    /// <summary>Actualiza un propietario existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, PropietarioActualizarDto dto)
    {
        try
        {
            var existe = await _propietarios.ActualizarAsync(id, dto);
            return existe ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    /// <summary>Elimina un propietario (solo si no tiene mascotas).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var existe = await _propietarios.EliminarAsync(id);
            return existe ? NoContent() : NotFound();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflicto(ex);
        }
    }

    private ActionResult Conflicto(ReglaNegocioException ex) =>
        Conflict(new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Regla de negocio incumplida",
            Detail = ex.Message
        });
}
