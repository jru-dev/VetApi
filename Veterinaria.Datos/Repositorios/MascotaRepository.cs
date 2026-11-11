using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class MascotaRepository : IMascotaRepository
{
    private readonly VeterinariaContext _context;

    public MascotaRepository(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<List<Mascota>> ObtenerTodasAsync(string? especie, string? nombre, int? propietarioId, decimal? pesoMinimo)
    {
        IQueryable<Mascota> consulta = _context.Mascotas.Include(m => m.Propietario);

        if (!string.IsNullOrWhiteSpace(especie))
            consulta = consulta.Where(m => m.Especie == especie);

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(m => m.Nombre.Contains(nombre));

        if (propietarioId.HasValue)
            consulta = consulta.Where(m => m.PropietarioId == propietarioId.Value);

        if (pesoMinimo.HasValue)
            consulta = consulta.Where(m => m.PesoKg >= pesoMinimo.Value);

        return await consulta.OrderBy(m => m.Nombre).ToListAsync();
    }

    public async Task<List<Mascota>> ObtenerPorPropietarioAsync(int propietarioId)
    {
        return await _context.Mascotas
            .Include(m => m.Propietario)
            .Where(m => m.PropietarioId == propietarioId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }

    public async Task<Mascota?> ObtenerPorIdAsync(int id)
    {
        return await _context.Mascotas
            .Include(m => m.Propietario)
            .FirstOrDefaultAsync(m => m.MascotaId == id);
    }

    public async Task<int> ContarPorPropietarioAsync(int propietarioId)
    {
        return await _context.Mascotas.CountAsync(m => m.PropietarioId == propietarioId);
    }

    public async Task<bool> ExisteMicrochipAsync(string microchip, int? excluirId = null)
    {
        return await _context.Mascotas
            .AnyAsync(m => m.Microchip == microchip && (excluirId == null || m.MascotaId != excluirId));
    }

    public async Task AgregarAsync(Mascota mascota)
    {
        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Mascota mascota)
    {
        // La entidad ya está rastreada por el contexto (se obtuvo con ObtenerPorIdAsync).
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Mascota mascota)
    {
        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();
    }
}
