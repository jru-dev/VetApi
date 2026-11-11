namespace Veterinaria.Entidades;

public partial class Mascota
{
    public int MascotaId { get; set; }

    public int PropietarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Especie { get; set; } = null!;

    public string? Raza { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public decimal PesoKg { get; set; }

    public string? Microchip { get; set; }

    public virtual Propietario Propietario { get; set; } = null!;
}
