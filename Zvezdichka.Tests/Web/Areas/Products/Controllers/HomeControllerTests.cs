using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Web.Areas.Products.Controllers;
using Xunit;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Constants;

namespace Zvezdichka.Tests.Web.Areas.Products.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void HomeShouldBeInProductsArea()
        {
            // Arrange
            var controller = typeof(HomeController);

            // Act
            var areaAttribute = controller
                    .GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.ProductsArea);
        }

//        [Fact]
//        public async Task SearchShouldReturnNoResultsWithNoCriteria()
//        {
//            // Arrange
//            var controller = new HomeController(null, null, null, null, null);
//
//            // Act
//            var result = await controller.Index("toy", 1, 20);
//
//            // Assert
//            result.Should().BeOfType<ViewResult>();
//
//            result.As<ViewResult>().Model.Should().BeOfType<ProductIndexViewModel>();
//
//            var indexViewModel = result.As<ViewResult>().Model.As<ProductIndexViewModel>();
//
//            indexViewModel.
//            indexViewModel.Users.Should().BeEmpty();
//            indexViewModel.SearchText.Should().BeNull();
//
//        }
    }
}

