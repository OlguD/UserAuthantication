using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using UserAuth.Models;

namespace UserAuth.Tests;

public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_And_Login_User_ShouldReturnToken()
    {
        var userRegister = (new User
        {
            Username = "test_username",
            Password = "test_password",
            Email = "test@email.com",
            Name = "Test",
            Surname = "User"
        });
        var userLogin = (new LoginRequest
        {
            Username = "test_username",
            Password = "test_password"
        });
        var contentRegister = new StringContent(JsonSerializer.Serialize(userRegister), Encoding.UTF8, "application/json");
        var contentLogin = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
        var registerResponse = await _client.PostAsync("/api/Auth/register", contentRegister);
        registerResponse.EnsureSuccessStatusCode();
        
        var loginResponse = await _client.PostAsync("/api/Auth/login", contentLogin);
        loginResponse.EnsureSuccessStatusCode();
        
        var responseString = await loginResponse.Content.ReadAsStringAsync();
        
        var jsonDoc = JsonDocument.Parse(responseString);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();
        
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        var meResponse = await _client.GetAsync("/api/Users/me");
        meResponse.EnsureSuccessStatusCode();
        var meResponseString = await meResponse.Content.ReadAsStringAsync();
        
        Assert.Contains("token", responseString);
        Assert.Contains("test_username", meResponseString);
    }
}