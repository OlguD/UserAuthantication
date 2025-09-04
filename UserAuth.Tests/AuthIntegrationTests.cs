using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using UserAuth.Models;
using Microsoft.Extensions.Hosting;
using UserAuth.Services;
using Microsoft.AspNetCore.Hosting;
using UserAuth.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace UserAuth.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureServices(services =>
        {
            // Find and remove the app's DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            
            // Find and remove app's IUserService implementation
            var userServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IUserService));
                
            if (userServiceDescriptor != null)
            {
                services.Remove(userServiceDescriptor);
            }
            
            // Find and remove JwtHelper registration
            var jwtHelperDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(JwtHelper));
                
            if (jwtHelperDescriptor != null)
            {
                services.Remove(jwtHelperDescriptor);
            }
            
            // Create a mock configuration with JWT settings
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "testsecretkeytestsecretkeytestsecretkey12345"},
                {"Jwt:Issuer", "testissuer"},
                {"Jwt:Audience", "testaudience"},
                {"Jwt:ExpireMinutes", "60"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
                
            // Add configuration to services
            services.AddSingleton<IConfiguration>(configuration);
            
            // Add test database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestingDb_" + Guid.NewGuid().ToString());
            });
            
            // Add test services
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<JwtHelper>(sp => new JwtHelper(sp.GetRequiredService<IConfiguration>()));
            
            // Build a temp service provider to initialize the test database
            var serviceProvider = services.BuildServiceProvider();
            
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                
                // Ensure database is created
                db.Database.EnsureCreated();
            }
        });
    }
}

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_And_Login_User_ShouldReturnToken()
    {
        // Create a unique username to avoid conflicts
        string uniqueUsername = "test_user_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        
        var userRegister = new User
        {
            Username = uniqueUsername,
            Password = "test_password",
            Email = "test@email.com",
            Name = "Test",
            Surname = "User",
            Role = "User" // Make sure all required fields are set
        };
        
        var userLogin = new LoginRequest
        {
            Username = uniqueUsername,
            Password = "test_password"
        };
        
        var contentRegister = new StringContent(
            JsonSerializer.Serialize(userRegister),
            Encoding.UTF8,
            "application/json");
        
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(userLogin),
            Encoding.UTF8,
            "application/json");
        
        // Register the user
        var registerResponse = await _client.PostAsync("/api/Auth/register", contentRegister);
        registerResponse.EnsureSuccessStatusCode();
        
        // Login with the registered user
        var loginResponse = await _client.PostAsync("/api/Auth/login", contentLogin);
        loginResponse.EnsureSuccessStatusCode();
        
        var responseString = await loginResponse.Content.ReadAsStringAsync();
        
        var jsonDoc = JsonDocument.Parse(responseString);
        var token = jsonDoc.RootElement.GetProperty("token").GetString();
        
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }
}