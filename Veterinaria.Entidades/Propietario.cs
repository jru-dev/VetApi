namespace Veterinaria.Entidades;

public partial class Propietario
{
    public int PropietarioId { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
}
