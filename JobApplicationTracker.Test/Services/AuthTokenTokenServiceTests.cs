using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using JobApplicationTracker.Models;
using JobApplicationTracker.Services;
using Microsoft.Extensions.Configuration;

namespace Test.Services;

public class AuthTokenTokenServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly AuthTokenTokenService _tokenTokenService;

    public AuthTokenTokenServiceTests()
    {
        // Disable default claim type mapping
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "ThisIsAVerySecretKeyForTestingPurposesOnly12345" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _tokenTokenService = new AuthTokenTokenService(_configuration);
    }

    [Fact]
    public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
    {
        // Arrange
        var user = new User
        {
            Id = "user-123",
            UserName = "testuser",
            Email = "test@example.com"
        };

        // Act
        var token = _tokenTokenService.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        // Decode the JWT manually
        var parts = token.Split('.');
        var payload = parts[1];
        
        // Add padding if needed
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        
        var payloadJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        var payloadDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(payloadJson);

        payloadDict.Should().ContainKey("iss");
        payloadDict["iss"].GetString().Should().Be("TestIssuer");
        
        payloadDict.Should().ContainKey("aud");
        payloadDict["aud"].GetString().Should().Be("TestAudience");
    
        // Verify expiration
        payloadDict.Should().ContainKey("exp");
        var expValue = payloadDict["exp"].GetInt64();
        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expValue).UtcDateTime;
        expirationDate.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(15), TimeSpan.FromMinutes(1));
    }


    [Fact]
    public void GenerateToken_WithValidUser_ContainsCorrectClaims()
    {
        // Arrange
        var user = new User
        {
            Id = "user-456",
            UserName = "johndoe",
            Email = "john@example.com"
        };

        // Act
        var token = _tokenTokenService.GenerateToken(user);

        // Assert - Decode the JWT manually
        var parts = token.Split('.');
        var payload = parts[1];
        
        // Add padding if needed
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        
        var payloadJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        var payloadDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, System.Text.Json.JsonElement>>(payloadJson);

        payloadDict.Should().ContainKey("sub");
        payloadDict["sub"].GetString().Should().Be("user-456");

        payloadDict.Should().ContainKey("name");
        payloadDict["name"].GetString().Should().Be("johndoe");

        payloadDict.Should().ContainKey("email");
        payloadDict["email"].GetString().Should().Be("john@example.com");
    }

    [Fact]
    public void GenerateToken_WithUserWithoutEmail_DoesNotIncludeEmailClaim()
    {
        // Arrange
        var user = new User
        {
            Id = "user-789",
            UserName = "nomail",
            Email = null
        };

        // Act
        var token = _tokenTokenService.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");
        emailClaim.Should().BeNull();

        var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        subClaim.Should().NotBeNull();
        subClaim!.Value.Should().Be("user-789");
    }

    [Fact]
    public void GenerateToken_WithMissingJwtKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var configWithoutKey = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" }
            }!)
            .Build();

        var tokenService = new AuthTokenTokenService(configWithoutKey);

        var user = new User
        {
            Id = "user-303",
            UserName = "testuser",
            Email = "test@example.com"
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => tokenService.GenerateToken(user));
        exception.Message.Should().Be("Jwt:Key is missing.");
    }

    [Fact]
    public void GenerateToken_TokenExpiresInFifteenMin()
    {
        // Arrange
        var user = new User
        {
            Id = "user-404",
            UserName = "timetest",
            Email = "time@example.com"
        };

        var beforeGeneration = DateTime.UtcNow;

        // Act
        var token = _tokenTokenService.GenerateToken(user);

        var afterGeneration = DateTime.UtcNow;

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Use ValidTo property instead of parsing exp claim
        var expirationDate = jwtToken.ValidTo;

        // Verify expiration is approximately 15 min from now
        var expectedExpiry = beforeGeneration.AddMinutes(15);
        expirationDate.Should().BeCloseTo(expectedExpiry, TimeSpan.FromMinutes(1));
    }
    

    [Fact]
    public void GenerateToken_MultipleCalls_GenerateDifferentTokens()
    {
        // Arrange
        var user = new User
        {
            Id = "user-606",
            UserName = "multitest",
            Email = "multi@example.com"
        };

        // Act
        var token1 = _tokenTokenService.GenerateToken(user);
        Thread.Sleep(1000); // Wait 1 second to ensure different expiry
        var token2 = _tokenTokenService.GenerateToken(user);

        // Assert
        token1.Should().NotBe(token2);
    }
    
    
}