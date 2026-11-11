namespace Veterinaria.Entidades.Dtos;

/// <summary>DTO de lectura: devuelve el nombre del propietario en lugar del objeto completo.</summary>
public class MascotaDto
{
    public int MascotaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Especie { get; set; } = null!;
    public string? Raza { get; set; }
    public DateOnly FechaNacimiento { get; set; }
    public decimal PesoKg { get; set; }
    public string? Microchip { get; set; }
    public int PropietarioId { get; set; }

    /// <summary>Calculado en el MappingProfile (ForMember + MapFrom).</summary>
    public string NombrePropietario { get; set; } = null!;
}
