using FluentAssertions;
using Xunit;
using Zvezdichka.Web.Areas.Admin.Controllers;

namespace Zvezdichka.Tests.Web.Areas.Admin.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void UsersControllerShouldInheritAdminBaseController()
        {
            // Arrange
            var controller = typeof(HomeController);

            // Act
            var doesInherit = controller.BaseType == typeof(AdminBaseController);

            // Assert
            doesInherit.Should().BeTrue();
        }
    }
}
