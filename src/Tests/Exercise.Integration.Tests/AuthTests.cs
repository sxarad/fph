using Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xunit;

namespace Exercise.Integration.Tests;

public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AuthTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // Note: Making sure we can run the API integration tests

    [Fact]
    public async Task Should_Return_Jwt()
    {
        // Arrange
        var client = _factory.CreateClient();
        var credential = new Credential() { UserName = "User1", Password = "Password1" };
        var dataAsString = JsonConvert.SerializeObject(credential);
        var content = new StringContent(dataAsString);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await client.PostAsync("/Auth", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
}
