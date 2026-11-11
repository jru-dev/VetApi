namespace Veterinaria.Entidades.Dtos;

/// <summary>DTO de lectura: es lo que la API devuelve al cliente.</summary>
public class PropietarioDto
{
    public int PropietarioId { get; set; }
    public string Dni { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public DateTime FechaRegistro { get; set; }

    /// <summary>Calculado en el MappingProfile (ForMember + MapFrom).</summary>
    public int TotalMascotas { get; set; }
}
