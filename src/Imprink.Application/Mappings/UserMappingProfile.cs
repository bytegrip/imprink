using System.Security.Claims;
using AutoMapper;
using Imprink.Application.Users.Dtos;
using Imprink.Domain.Entities.Users;
using Imprink.Domain.Models;

namespace Imprink.Application.Mappings;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.DefaultAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.DefaultAddress, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        
        CreateMap<UserRole, UserRoleDto>();
        CreateMap<UserRoleDto, UserRole>();
        
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
        CreateMap<RoleDto, Role>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
        
        CreateMap<ClaimsPrincipal, Auth0User>()
            .ForMember(dest => dest.Sub, opt => opt.MapFrom(src => 
                src.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                src.Claims.FirstOrDefault(c => c.Type == "name")!.Value))
            .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => 
                src.Claims.FirstOrDefault(c => c.Type == "nickname")!.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => 
                src.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value))
            .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => 
                src.Claims.FirstOrDefault(c => c.Type == "email_verified")!.Value == "true"));
    }
}