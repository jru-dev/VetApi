using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos.Repositorios;

public class PropietarioRepository : IPropietarioRepository
{
    private readonly VeterinariaContext _context;

    public PropietarioRepository(VeterinariaContext context)
    {
        _context = context;
    }

    public async Task<List<Propietario>> ObtenerTodosAsync(string? apellido, string? dni)
    {
        IQueryable<Propietario> consulta = _context.Propietarios.Include(p => p.Mascotas);

        if (!string.IsNullOrWhiteSpace(apellido))
            consulta = consulta.Where(p => p.Apellidos.Contains(apellido));

        if (!string.IsNullOrWhiteSpace(dni))
            consulta = consulta.Where(p => p.Dni == dni);

        return await consulta
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
    }

    public async Task<Propietario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Propietarios
            .Include(p => p.Mascotas)
            .FirstOrDefaultAsync(p => p.PropietarioId == id);
    }

    public async Task<bool> ExisteDniAsync(string dni, int? excluirId = null)
    {
        return await _context.Propietarios
            .AnyAsync(p => p.Dni == dni && (excluirId == null || p.PropietarioId != excluirId));
    }

    public async Task AgregarAsync(Propietario propietario)
    {
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Propietario propietario)
    {
        // La entidad ya está rastreada por el contexto (se obtuvo con ObtenerPorIdAsync).
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Propietario propietario)
    {
        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
    }
}
