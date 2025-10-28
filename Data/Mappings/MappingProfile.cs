using AutoMapper;
using Entity.Dtos;
using Entity.Entities;

namespace Data.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => src.Created))
                .ReverseMap();

            // Role mappings
            CreateMap<Role, RoleDto>().ReverseMap();

            // Persona mappings
            CreateMap<Persona, PersonaDto>().ReverseMap();

            // Permission mappings
            CreateMap<Permission, PermissionDto>().ReverseMap();

            // Forma mappings
            CreateMap<Forma, FormaDto>().ReverseMap();

            // Modulo mappings
            CreateMap<Modulo, ModuloDto>().ReverseMap();

            // ModuleForm mappings
            CreateMap<ModuleForm, ModuleFormDto>().ReverseMap();

            // RoleFormPermission mappings
            CreateMap<RoleFormPermission, RoleFormPermissionDto>().ReverseMap();

            // UserRole mappings
            CreateMap<UserRole, UserRoleDto>().ReverseMap();
        }
    }
}
