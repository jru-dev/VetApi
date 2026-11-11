using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

/// <summary>DTO de entrada para crear un propietario.</summary>
public class PropietarioCrearDto
{
    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos.")]
    public string Dni { get; set; } = null!;

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(80, MinimumLength = 2)]
    public string Nombres { get; set; } = null!;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(80, MinimumLength = 2)]
    public string Apellidos { get; set; } = null!;

    [StringLength(15)]
    public string? Telefono { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }
}
