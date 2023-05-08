using AutoMapper;
using WebApp.Core.Entities.Auth;
using WebApp.DataTransferObject.Dtos.Auth.Request;
using WebApp.DataTransferObject.Dtos.Auth.Response;

namespace WebApp.Core.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly SharedMapper _sharedMapper;

        public MappingProfile(SharedMapper sharedMapper)
        {
            _sharedMapper = sharedMapper;

            #region Auth
            CreateMap<UserRegisterRequestDto, User>();

            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserResponseDto>()
                .ForMember(d => d.DisplayPath, o => o.MapFrom(s => _sharedMapper.BuildProfileImagePath(s.Path)));

            CreateMap<Role, RoleResponseDto>();
            #endregion
        }


    }
}
