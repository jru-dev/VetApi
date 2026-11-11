using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;

namespace Veterinaria.Datos;

// Generado con Scaffold (base de datos primero) y con las entidades movidas al proyecto Entidades.
public partial class VeterinariaContext : DbContext
{
    public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    public virtual DbSet<Propietario> Propietarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.MascotaId).HasName("PK_Mascota");

            entity.ToTable("Mascota");

            entity.HasIndex(e => e.Microchip, "UQ_Mascota_Microchip")
                .IsUnique()
                .HasFilter("([Microchip] IS NOT NULL)");

            entity.Property(e => e.Especie).HasMaxLength(30);
            entity.Property(e => e.Microchip).HasMaxLength(15);
            entity.Property(e => e.Nombre).HasMaxLength(60);
            entity.Property(e => e.PesoKg).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Raza).HasMaxLength(60);

            entity.HasOne(d => d.Propietario).WithMany(p => p.Mascotas)
                .HasForeignKey(d => d.PropietarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascota_Propietario");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasKey(e => e.PropietarioId).HasName("PK_Propietario");

            entity.ToTable("Propietario");

            entity.HasIndex(e => e.Dni, "UQ_Propietario_Dni").IsUnique();

            entity.Property(e => e.Apellidos).HasMaxLength(80);
            entity.Property(e => e.Dni).HasMaxLength(8);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Nombres).HasMaxLength(80);
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
