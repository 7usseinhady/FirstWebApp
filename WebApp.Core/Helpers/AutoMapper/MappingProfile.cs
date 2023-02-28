using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApp.Core.Entities.Auth;
using WebApp.DataTransferObjects.DTOs.Auth.Request;
using WebApp.DataTransferObjects.DTOs.Auth.Response;

namespace WebApp.Core.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly SharedMapper _sharedMapper;

        public MappingProfile(SharedMapper sharedMapper)
        {
            _sharedMapper = sharedMapper;

            #region Auth
            CreateMap<UserRegisterRequestDTO, User>();

            CreateMap<UserRequestDTO, User>();
            CreateMap<User, UserResponseDTO>()
                .ForMember(d => d.DisplayPath, o => o.MapFrom(s => _sharedMapper.BuildProfileImagePath(s.Path)));

            CreateMap<IdentityRole, RoleResponseDTO>();
                //.ForMember(d => d.UserCount, s => s.MapFrom(o => _sharedMapper.GetRoleUserCount(o.Id)));
            #endregion
        }


    }
}
