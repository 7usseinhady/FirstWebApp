using AutoMapper;
using WebApp.Core.Entities.Auth;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Dtos.Auth.Response;

namespace WebApp.Core.Helpers.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly SharedMapper _sharedMapper;

        public MappingProfile(SharedMapper sharedMapper)
        {
            _sharedMapper = sharedMapper;

            #region Auth
            CreateMap<UserRegisterRequestDto, User>()
                .ForMember(d => d.NationalPhoneNumber, o => o.MapFrom(s => _sharedMapper.ToNationalPhoneNumber(s.PhoneNumber, null)))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => _sharedMapper.ToInternationalPhoneNumber(s.PhoneNumber, null)))
                .ForMember(d => d.NationalPhoneNumber2, o => o.MapFrom(s => _sharedMapper.ToNationalPhoneNumber(s.PhoneNumber2, null)))
                .ForMember(d => d.PhoneNumber2, o => o.MapFrom(s => _sharedMapper.ToInternationalPhoneNumber(s.PhoneNumber2, null)));

            CreateMap<UserEditRequestDto, User>()
                .ForMember(d => d.NationalPhoneNumber, o => o.MapFrom(s => _sharedMapper.ToNationalPhoneNumber(s.PhoneNumber, null)))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => _sharedMapper.ToInternationalPhoneNumber(s.PhoneNumber, null)))
                .ForMember(d => d.NationalPhoneNumber2, o => o.MapFrom(s => _sharedMapper.ToNationalPhoneNumber(s.PhoneNumber2, null)))
                .ForMember(d => d.PhoneNumber2, o => o.MapFrom(s => _sharedMapper.ToInternationalPhoneNumber(s.PhoneNumber2, null)));

            CreateMap<User, UserResponseDto>()
                .ForMember(d => d.DisplayPath, o => o.MapFrom(s => _sharedMapper.BuildProfileImagePath(s.Path)));

            CreateMap<RoleRequestDto, Role>()
                .ForMember(d => d.NormalizedName, o => o.MapFrom(s => s.Name.ToUpper()));
            CreateMap<Role, RoleResponseDto>();
            #endregion
        }


    }
}
