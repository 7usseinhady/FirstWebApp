using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using WebApp.Core.Entities.Auth;
using WebApp.Core.Helpers;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.Custom.Services.Auth;
using WebApp.Core.Services.Auth;
using WebApp.SharedKernel.Dtos;
using WebApp.SharedKernel.Dtos.Auth.Request;
using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.Interfaces;

namespace WebApp.UnitTest.Core.Services.Auth
{
    [TestClass]
    public class AuthServiceTest
    {
        [TestMethod]
        public async Task RegisterAdminAsync()
        {
            // Arrange
            var userLoginRequestDto = new UserLoginRequestDto() { PersonalKey = "admin", Password = "Icity@2022" };

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var cultureMock = new Mock<ICulture>();
            var roleMock = new Mock<RoleManager<Role>>();
            var userMock = new Mock<UserManager<User>>();
            var emailSenderMock = new Mock<IEmailSender>();
            var jwtMock = new Mock<IOptions<Jwt>>();
            var serverMock = new Mock<IServer>();
            var smsServiceMock = new Mock<ISmsService>();

            var authService = new AuthService(unitOfWorkMock.Object, mapperMock.Object, new HolderOfDto(),
                                              cultureMock.Object, roleMock.Object, userMock.Object,
                                              emailSenderMock.Object, jwtMock.Object, serverMock.Object, smsServiceMock.Object);
            
            HolderOfDto holder = await authService.LoginAsync(userLoginRequestDto, null);

            // Act
            object state;
            holder.TryGetValue(Res.state, out state!);

            // Assert
            Assert.IsFalse((bool)state);
        }
    }
}
