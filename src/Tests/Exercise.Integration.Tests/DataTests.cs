using Exercise.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xunit;

namespace Exercise.Integration.Tests;

public class DataTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public DataTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/Data/Hello")] 
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }


    [Fact]
    public async Task Should_Return_ModelType1_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mt1 = new ModelType1() { DenyUnlessLoggedIn = false, SomeOtherField = nameof(Should_Return_ModelType1_Ok) };
        var dataAsString = JsonConvert.SerializeObject(mt1);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PostAsync("/Data/ModelType1", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_InternalServerError()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mt1 = new ModelType1() { DenyUnlessLoggedIn = true, SomeOtherField = nameof(Should_Return_InternalServerError) };
        var dataAsString = JsonConvert.SerializeObject(mt1);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PostAsync("/Data/ModelType1", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_ModelType2_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();
        var mt1 = new ModelType2() { SomeOtherField = nameof(Should_Return_ModelType2_Ok) };
        var dataAsString = JsonConvert.SerializeObject(mt1);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PostAsync("/Data/ModelType2", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }
}