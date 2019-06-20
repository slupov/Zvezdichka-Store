using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services.Contracts;
using Zvezdichka.Tests.Mocks;
using Zvezdichka.Web.Areas.Products.Controllers;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Constants;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers;
using Zvezdichka.Web.Infrastructure.Extensions.Helpers.Html;

namespace Zvezdichka.Tests.Web.Areas.Products.Controllers
{
    public class HomeControllerTests
    {
        private const string FirstCategory = "FirstCategory";
        private const string SecondCategory = "SecondCategory";

        private const string FirstUserId = "1";
        private const string FirstUserUsername = "First";
        private const string SecondUserId = "2";
        private const string SecondUserUsername = "Second";

        [Fact]
        public void HomeShouldBeInProductsArea()
        {
            // Arrange
            var controller = typeof(HomeController);

            // Act
            var areaAttribute = controller
                    .GetCustomAttributes(true)
                    .SingleOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.Areas.ProductsArea);
        }

        [Fact]
        public async Task GetCreateShouldReturnViewWithValidModel()
        {
            // Arrange
            var categoryService = new Mock<IGenericDataService<Category>>();

            List<Category> categoriesList = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = FirstCategory
                },
                new Category()
                {
                    Id = 2,
                    Name = SecondCategory
                }
            };

            categoryService.Setup(c => c.GetAllAsync().GetAwaiter().GetResult())
                .Returns(categoriesList);

            var controller = new HomeController(null, categoryService.Object, null, null, null);

            // Act
            var result = await controller.Create();

            // Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;
            model.Should().BeOfType<ProductCreateModel>();

            var formModel = model.As<ProductCreateModel>();
            formModel.Categories.Should().NotBeEmpty();
        }

        [Fact]
        public async Task PostCreateShouldRedirectWithValidModel()
        {
            Tests.Initialize();
            //Arrange
            const string nameValue = "Name";
            const string descriptionValue = "Description";
            const string thumbnailSourceValue = "ThumbnailSource";
            const decimal priceValue = 1.99m;
            const byte stockValue = 1;

            string modelName = null;
            string modelDescription = null;
            string modelThumbnailSource = null;
            decimal modelPriceValue = 0;
            byte modelStock = 1;

            var categoryService = new Mock<IGenericDataService<Category>>();
            var productsService = new Mock<IGenericDataService<Product>>();
            var categoryProductsService = new Mock<IGenericDataService<CategoryProduct>>();

            #region 
            var categoriesToAdd = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = FirstCategory
                },
                new Category()
                {
                    Id = 2,
                    Name = SecondCategory
                }
            };

            var categoryProductsToAdd = new CategoryProduct[]
            {
                new CategoryProduct()
                {
                    ProductId = 1,
                    CategoryId = 1
                },
                new CategoryProduct()
                {
                    ProductId = 1,
                    CategoryId = 2
                }
            };

#endregion

            categoryService.Setup(c => c.GetListAsync(It.IsAny<Func<Category, bool>>()).GetAwaiter().GetResult())
                .Returns(categoriesToAdd);

            productsService.Setup(c => c.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>()).GetAwaiter().GetResult())
                .Returns(false);

            productsService.Setup(c => c.Add(It.IsAny<Product[]>()))
                .Callback((Product[] prod) =>
                {
                    modelName = prod[0].Name;
                    modelDescription = prod[0].Description;
                    modelPriceValue = prod[0].Price;
                    modelStock = prod[0].Stock;
                    modelThumbnailSource = prod[0].ThumbnailSource;
                });


            var vm = new Product()
            {
                Id = 1,
                Name = nameValue,
                Price = priceValue,
                Stock = stockValue,
                Description = descriptionValue,
                ThumbnailSource = thumbnailSourceValue
            };

            productsService.Setup(c => c.GetSingleOrDefault(It.IsAny<Expression<Func<Product,bool>>>()))
                .Returns(vm);

            categoryProductsService.Setup(c => c.Add(categoryProductsToAdd));

            var controller = new HomeController(productsService.Object, categoryService.Object, categoryProductsService.Object, null, null);

            //Act
            var result = await controller.Create(Mapper.Map<ProductCreateModel>(vm));

            // Assert
            modelName.Should().Be(nameValue);
            modelDescription.Should().Be(descriptionValue);
            modelStock.Should().Be(stockValue);
            modelPriceValue.Should().Be(priceValue);
            modelThumbnailSource.Should().Be(thumbnailSourceValue);

            result.Should().BeOfType<RedirectToActionResult>();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Details");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
            result.As<RedirectToActionResult>().RouteValues.Keys.Should().Contain("area");
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var userManager = UserManagerMock.New;
            userManager
                .Setup(u => u.GetUsersInRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ApplicationUser>
                {
                    new ApplicationUser {Id = FirstUserId, UserName = FirstUserUsername},
                    new ApplicationUser {Id = SecondUserId, UserName = SecondUserUsername}
                });

            return userManager;
        }

        private void AssertCategoriesList(ICollection<string> categories)
        {
            categories.Should().Match(items => items.Count() == 2);

            categories.First().Should().Match(u => u.As<string>() == FirstCategory);
            categories.Last().Should().Match(u => u.As<string>() == SecondCategory);
        }
    }
}