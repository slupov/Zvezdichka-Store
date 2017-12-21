using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using Zvezdichka.Web.Areas.Admin.Controllers;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Tests.Web.Areas.Admin.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void UsersControllerShouldInheritAdminBaseController()
        {
            // Arrange
            var controller = typeof(UsersController);

            // Act
            var doesInherit = controller.BaseType == typeof(AdminBaseController);

            // Assert
            doesInherit.Should().BeTrue();
        }
    }
}
