using Microsoft.AspNetCore.Identity;
using Moq;
using Zvezdichka.Data.Models;

namespace Zvezdichka.Tests.Mocks
{
    public class UserManagerMock
    {
        public static Mock<UserManager<ApplicationUser>> New
            => new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    }
}
