using AutoMapper;
using Veterinaria.Entidades;
using Veterinaria.Entidades.Dtos;

namespace Veterinaria.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ---------- Entidad -> DTO de lectura (lo que sale de la API) ----------
        CreateMap<Propietario, PropietarioDto>()
            .ForMember(dest => dest.TotalMascotas,
                       opt => opt.MapFrom(src => src.Mascotas.Count));

        CreateMap<Mascota, MascotaDto>()
            .ForMember(dest => dest.NombrePropietario,
                       opt => opt.MapFrom(src => src.Propietario.Nombres + " " + src.Propietario.Apellidos));

        // ---------- DTO de entrada -> Entidad (lo que llega del cliente) ----------
        CreateMap<PropietarioCrearDto, Propietario>();

        CreateMap<PropietarioActualizarDto, Propietario>();

        CreateMap<MascotaCrearDto, Mascota>()
            .ForMember(dest => dest.FechaNacimiento,
                       opt => opt.MapFrom(src => src.FechaNacimiento.GetValueOrDefault()));

        CreateMap<MascotaActualizarDto, Mascota>()
            .ForMember(dest => dest.FechaNacimiento,
                       opt => opt.MapFrom(src => src.FechaNacimiento.GetValueOrDefault()));
    }
}
