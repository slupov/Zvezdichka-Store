using FluentAssertions;
using Xunit;
using Zvezdichka.Web.Areas.Admin.Controllers;

namespace Zvezdichka.Tests.Web.Areas.Admin.Controllers
{
    public class CategoriesControllerTests
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
