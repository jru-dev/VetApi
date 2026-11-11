namespace Veterinaria.Negocio.Excepciones;

/// <summary>Se lanza cuando se incumple una regla de negocio. El controlador la convierte en 409 Conflict.</summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje)
    {
    }
}
