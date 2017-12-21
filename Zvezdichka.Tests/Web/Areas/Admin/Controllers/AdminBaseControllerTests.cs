using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Zvezdichka.Web.Areas.Admin.Controllers;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Tests.Web.Areas.Admin.Controllers
{
    public class AdminBaseControllerTests
    {
        [Fact]
        public void AdminBaseControllerShouldBeInAreaForAdmin()
        {
            //Arrange
            var controller = typeof(AdminBaseController);

            //Act
            var areaAttribute = controller.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute)) as AreaAttribute;

            //Assert
            areaAttribute.Should().NotBe(null);
            areaAttribute.RouteValue.Should().Be(WebConstants.Areas.AdminArea);
        }

        [Fact]
        public void AdminBaseControllerShouldBeForAuthorizedAdminsOnly()
        {
            // Arrange
            var controller = typeof(AdminBaseController);

            // Act
            var areaAttribute = controller
                    .GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.Roles.Should().Be(WebConstants.RoleNames.AdminRole);
        }
    }
}
