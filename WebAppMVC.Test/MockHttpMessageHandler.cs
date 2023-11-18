using Microsoft.Net.Http.Headers;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAppMVC.Test
{
    internal class MockHttpMessageHandler<T>
    {
        internal static Mock<HttpMessageHandler> SetBasicOptionsReturnListOfProducts(List<T> expectedContent)
        {
            var moqResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedContent))
            };
            moqResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var moqHandler = new Mock<HttpMessageHandler>();

            moqHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(moqResponse);

            return moqHandler;
        }

        internal static Mock<HttpMessageHandler> Return404EmptyListOfProducts()
        {
            var moqResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(null)
            )}; 

            moqResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var moqHandler = new Mock<HttpMessageHandler>();

            moqHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(moqResponse);

            return moqHandler;
        }
    }
}
