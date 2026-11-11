using System.ComponentModel.DataAnnotations;

namespace Veterinaria.Entidades.Dtos;

/// <summary>DTO de entrada para registrar una mascota.</summary>
public class MascotaCrearDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un PropietarioId válido.")]
    public int PropietarioId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, MinimumLength = 1)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La especie es obligatoria.")]
    [StringLength(30, MinimumLength = 2)]
    public string Especie { get; set; } = null!;

    [StringLength(60)]
    public string? Raza { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly? FechaNacimiento { get; set; }

    [Range(0.01, 200, ErrorMessage = "El peso debe estar entre 0.01 y 200 kg.")]
    public decimal PesoKg { get; set; }

    [RegularExpression(@"^\d{15}$", ErrorMessage = "El microchip debe tener 15 dígitos.")]
    public string? Microchip { get; set; }
}
