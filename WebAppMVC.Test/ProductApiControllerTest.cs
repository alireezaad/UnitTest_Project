using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppMVC.Controllers;
using WebAppMVC.Models.Entities;
using WebAppMVC.Models.Services;
using WebAppMVC.TestData;
using Xunit;

namespace WebAppMVC.Test
{
    public class ProductApiControllerTest
    {
        ProductMemberData data;
        public ProductApiControllerTest()
        {
            data= new ProductMemberData();

        }

        [Fact]
        public void Get_Return_ListOfProducts_UsingHttpRequest()
        {
            //arrange
            var moqMessageHandler = MockHttpMessageHandler<Product>.SetBasicOptionsReturnListOfProducts(data.GetData().ToList());
            var client = new HttpClient(moqMessageHandler.Object);
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProducts()).Returns(data.GetData());
            var sut = new ProductApiController(service.Object,client);

            //act
            var result = sut.Get();

            //assert
            moqMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
                );
            Assert.IsType<List<Product>>(result.Result);
            var products = result.Result;
            Assert.Equal(data.GetData().Count, products.Count);
        }
        [Fact]
        public async Task Get_ReturnEmptyListOfProducts_UsingHttpReqest()
        {
            //arrange
            var moqHttpMessageHandler = MockHttpMessageHandler<Product>.Return404EmptyListOfProducts();
            var httpClient = new HttpClient(moqHttpMessageHandler.Object);
            var service = new Mock<IProductService>();
            var sut = new ProductApiController(service.Object, httpClient);

            //act
            var result = await sut.Get();

            //assert
            moqHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
                );
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);
        }



        //[Fact]
        //public void Get_Return_ListOfProsucts_200StatusCode()
        //{
        //    //arrange
        //    var service = new Mock<IProductService>();
        //    service.Setup(p => p.GetProducts()).Returns(data.GetData());
        //    var sut = new ProductApiController(service.Object);

        //    //act
        //    var result = sut.Get();

        //    //assert
        //    Assert.IsType<OkObjectResult>(result);
        //    var okObjectResult = (OkObjectResult)result;
        //    Assert.IsType<List<Product>>(okObjectResult.Value);
        //    Assert.Equal(200, okObjectResult.StatusCode);
        //}




        [Theory]
        [InlineData(2)]
        public void GetById_Return_Product_AsOkObjectResult_StatusCode200(int validId)
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(validId)).Returns(data.GetData().FirstOrDefault(p => p.Id == validId));
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Get(validId);

            //assert
            service.Verify(p => p.GetProductById(2),Times.Once());
            Assert.IsType<OkObjectResult>(result);
            var ok = (OkObjectResult)result;
            Assert.IsType<Product>(ok.Value);
            Assert.True(ok.StatusCode == 200);
            Assert.NotNull(ok.Value);
        }
        [Theory]
        [InlineData(22)]
        public void GetById_Return_NotFoundResult_404StatusCode(int invalidId) 
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(invalidId)).Returns(data.GetData().FirstOrDefault(p => p.Id == invalidId));
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Get(invalidId);

            //assert
            service.Verify(p => p.GetProductById(22), Times.Once);
            Assert.IsType<NotFoundResult>(result);
            var notFound = (NotFoundResult)result;
            Assert.NotNull(notFound);
            Assert.Equal(404, notFound.StatusCode);
       
        }

        [Fact]
        public void Post_Return_Product_WithGetById_AsCreatedAtAction_201StatusCode()
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.Add(data.GetData().First())).Returns(data.GetData().First());
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Post(data.GetData().First());

            //assert
            service.Verify(p => p.Add(It.IsAny<Product>()), Times.Once);
            //service.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once);
            Assert.IsType<CreatedAtActionResult>(result);
            var createdAt = (CreatedAtActionResult)result;
            Assert.NotNull(createdAt);
            Assert.Equal("Get", createdAt.ActionName);
            Assert.True(createdAt.ControllerName == null);
            Assert.Equal(201,createdAt.StatusCode);
        }

        [Fact]
        public void Post_Return_BadRequest_400StatusCode()
        {
            //arrange
            var service = new Mock<IProductService>();
            Product product = null;
            service.Setup(p => p.Add(product)).Returns(new Product() { });
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Post(product);

            //assert
            service.Verify(p => p.Add(It.IsAny<Product>()), Times.Never);
            Assert.IsType<BadRequestResult>(result);
            var badReq = (BadRequestResult)result;
            Assert.Equal(400, badReq.StatusCode);
            Assert.True(badReq != null);

        }

        [Theory]
        [InlineData(2)]
        public void Delete_Return_OkObjectResult_200StatusCode(int validId)
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(validId)).Returns(data.GetData().FirstOrDefault(p => p.Id == validId));
            service.Setup(p => p.Delete(validId));
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Delete(validId);

            //assert
            service.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once);
            service.Verify(p => p.Delete(It.IsAny<int>()), Times.Once);
            Assert.IsType<OkObjectResult>(result);
            var ok = (OkObjectResult)result;
            Assert.Equal(200, ok.StatusCode);
            Assert.Equal(true, ok.Value);
        }

        [Theory]
        [InlineData(22)]
        public void Delete_Return_NotFoundResult_404StatusCode(int invalidId)
        {
            //arrange
            var service = new Mock<IProductService>();
            service.Setup(p => p.GetProductById(invalidId)).Returns(data.GetData().FirstOrDefault(p => p.Id == invalidId));
            service.Setup(p => p.Delete(invalidId));
            var sut = new ProductApiController(service.Object);

            //act
            var result = sut.Delete(invalidId);

            //assert
            service.Verify(p => p.GetProductById(It.IsAny<int>()), Times.Once);
            service.Verify(p => p.Delete(It.IsAny<int>()), Times.Never);
            Assert.IsType<NotFoundResult>(result);
            var notFound = (NotFoundResult)result;
            Assert.Equal(404, notFound.StatusCode);

        }
    }
}
