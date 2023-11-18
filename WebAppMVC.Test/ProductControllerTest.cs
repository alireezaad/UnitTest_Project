using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAppMVC.Controllers;
using WebAppMVC.Models.Entities;
using WebAppMVC.Models.Services;
using WebAppMVC.TestData;
using Xunit;

namespace WebAppMVC.Test
{
    public class ProductControllerTest
    {
        
        ProductMemberData data;

        public ProductControllerTest()
        {
            data = new ProductMemberData();
        }

        [Fact]
        public void IndexTest_ReturnListProduct_AsViewResult()
        {
            //arrange 
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProducts()).Returns(data.GetData());
            var sut = new ProductController(service.Object);

            //act 
            var result = sut.Index();
            //assert
            service.Verify(p => p.GetProducts(), Times.Once());
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<List<Product>>(viewResult.Model);
        }

        [Theory]
        [InlineData(3)]
        public void DetailsTest_ReturnProduct_AsViewResult(int validId)
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(validId)).Returns(data.GetData().FirstOrDefault(p => p.Id == validId));
            var sut = new ProductController(service.Object);

            //act
            var result = sut.Details(validId);

            //assert
            service.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once());
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.NotNull(viewResult.Model);
        }

        [Theory]
        [InlineData(4)]
        public void DetailsTest_ReutrnNotFound_ResultNull(int invalidId)
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(invalidId)).Returns(data.GetData().FirstOrDefault(p => p.Id == invalidId));
            //service.Setup(p => p.GetProductById(invalidId)).Returns((Product)null);
            var sut = new ProductController(service.Object);

            //act
            var result = sut.Details(invalidId);

            //assert
            service.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once());
            var viewResult = (NotFoundResult)result;
            Assert.IsType<NotFoundResult>(result);
            Assert.IsAssignableFrom<NotFoundResult>(viewResult);
            Assert.Equal(404, viewResult.StatusCode);

        }

        [Fact]
        public void CreateTest_OnSuccess500StatusCode_Returns_RedirectToAction()
        {
            //arrange
            var service = new Mock<IProductService>();
            var sut = new ProductController(service.Object);

            //act 
            //var result = sut.Create(new Product()
            //{
            //    Id = 1,
            //    Name = "TDD",
            //    Price = 2000,
            //    Description = "Description"
            //});
            var result = sut.Create(data.GetData().FirstOrDefault());

            //assert
            service.Verify(p => p.Add(It.IsAny<Product>()), Times.Once());
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsType<RedirectToActionResult>(redirectResult);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("ProductController", redirectResult.ControllerName);

        }

        [Fact]
        public void CreateTest_ReturnBadRequest_400StatusCode()
        {
            //arrange
            var service = new Mock<IProductService>();
            var sut = new ProductController(service.Object);


            //act

            // hatman bayad azinja Error bedahim ta dar controller BadRequest return shavad!!!!!!!!!!
            sut.ModelState.AddModelError("Name", "Please enter Product name");

            var result = sut.Create(new Product()
            {
                Price = 3000,

            });

            //assert
            service.Verify(p => p.Add(It.IsAny<Product>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);
            var badReq = (BadRequestObjectResult)result;
            Assert.Equal(400, badReq.StatusCode);
            var error = Assert.IsAssignableFrom<SerializableError>(badReq.Value);
            Assert.True(error.ContainsKey("Name"));
            Assert.Equal(new[] { "Please enter Product name" }, (string[])error["Name"]);

        }
    }
}