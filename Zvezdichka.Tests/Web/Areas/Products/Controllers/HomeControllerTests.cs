using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Zvezdichka.Web.Areas.Products.Controllers;
using Xunit;
using Zvezdichka.Data.Models;
using Zvezdichka.Data.Models.Mapping;
using Zvezdichka.Services.Contracts.Entity;
using Zvezdichka.Services.Contracts.Entity.Mapping;
using Zvezdichka.Tests.Mocks;
using Zvezdichka.Web.Areas.Products.Models;
using Zvezdichka.Web.Infrastructure.Constants;

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
                    .FirstOrDefault(a => a.GetType() == typeof(AreaAttribute))
                as AreaAttribute;

            // Assert
            areaAttribute.Should().NotBeNull();
            areaAttribute.RouteValue.Should().Be(WebConstants.Areas.ProductsArea);
        }

        [Fact]
        public async Task GetCreateShouldReturnViewWithValidModel()
        {
            // Arrange
            var categoryService = new Mock<ICategoriesDataService>();

            IList<Category> categoriesList = new List<Category>()
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

            categoryService.Setup(c => c.GetAll())
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

//        [Fact]
//        public async Task PostCreateShouldRedirectWithValidModel()
//        {
//            //Arrange
//            const string nameValue = "Name";
//            const string descriptionValue = "Description";
//            const string thumbnailSourceValue = "ThumbnailSource";
//            const decimal priceValue = 1.99m;
//            const byte stockValue = 1;
//
//            string modelName = null;
//            string modelDescription = null;
//            string modelThumbnailSource = null;
//            decimal modelPriceValue = 0;
//            byte modelStock = 1;
//
//            var categoryService = new Mock<ICategoriesDataService>();
//            var productsService = new Mock<IProductsDataService>();
//            var categoryProductsService = new Mock<ICategoryProductsDataService>();
//
//            //
//            var categoriesToAdd = new List<Category>()
//            {
//                new Category()
//                {
//                    Id = 1,
//                    Name = FirstCategory
//                },
//                new Category()
//                {
//                    Id = 2,
//                    Name = SecondCategory
//                }
//            };
//
//            var productToAdd = new Product()
//            {
//                Id = 1,
//                Name = "FirstProduct",
//                Price = 1,
//                Stock = 1
//            };
//
//            var categoryProductsToAdd = new CategoryProduct[]
//            {
//                new CategoryProduct()
//                {
//                    ProductId = 1,
//                    CategoryId = 1
//                },
//                new CategoryProduct()
//                {
//                    ProductId = 1,
//                    CategoryId = 2
//                }
//            };
//            //
//
//            categoryService.Setup(c => c.GetList(It.IsAny<Func<Category, bool>>()))
//                .Returns(categoriesToAdd);
//
//            productsService.Setup(c => c.Add(productToAdd))
//                .Callback((string name, string description, decimal price, byte stock, string thumbnail) =>
//                {
//                    modelName = name;
//                    modelDescription = description;
//                    modelPriceValue = price;
//                    modelStock = stock;
//                    modelThumbnailSource = thumbnail;
//                });
//
//
//            productsService.Setup(c => c.GetSingle(It.IsAny<Func<Product,bool>>()))
//                .Returns(productToAdd);
//
//            categoryProductsService.Setup(c => c.Add(categoryProductsToAdd));
//
//            var controller = new HomeController(productsService.Object, categoryService.Object, categoryProductsService.Object, null, null);
//
//            //Act
//            var result = await controller.Create(Mapper.Map<ProductCreateModel>(productToAdd));
//
//            //Assert
//
//            result.Should();
//        }

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