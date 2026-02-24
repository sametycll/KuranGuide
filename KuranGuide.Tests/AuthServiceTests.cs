using KuranGuide.Application.DTOs.Auth;
using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Services;
using KuranGuide.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq.Expressions;

namespace KuranGuide.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IKullaniciRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IKullaniciRepository>();
            _mockConfig = new Mock<IConfiguration>();

            // JWT config setup
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("K7x9pQm3vZ2wR8nF5jL6tY4uH1gA0sD3bC8eX7iW2oP9qN5zM4kJ6rT1yU0fV3");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("KuranGuideApi");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("KuranGuideApiUsers");
            _mockConfig.Setup(c => c["Jwt:ExpiresHours"]).Returns("12");

            _authService = new AuthService(_mockRepo.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithNewEmail_ReturnsTrue()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Ad = "Test",
                Soyad = "User",
                Email = "test@test.com",
                Password = "Test1234!"
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Kullanici, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Kullanici>());

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Kullanici>()))
                .ReturnsAsync(new Kullanici { Id = 1, Email = dto.Email });

            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Kullanici>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ReturnsFalse()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Ad = "Test",
                Soyad = "User",
                Email = "existing@test.com",
                Password = "Test1234!"
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Kullanici, bool>>>()))
                .ReturnsAsync(new List<Kullanici> { new Kullanici { Email = "existing@test.com" } });

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Kullanici>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var password = "Test1234!";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = password
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Kullanici, bool>>>()))
                .ReturnsAsync(new List<Kullanici>
                {
                    new Kullanici
                    {
                        Id = 1,
                        Email = "test@test.com",
                        PasswordHash = hashedPassword,
                        Role = "User",
                        Ad = "Test",
                        Soyad = "User"
                    }
                });

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal("test@test.com", result.Email);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");

            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "WrongPassword"
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Kullanici, bool>>>()))
                .ReturnsAsync(new List<Kullanici>
                {
                    new Kullanici
                    {
                        Id = 1,
                        Email = "test@test.com",
                        PasswordHash = hashedPassword,
                        Role = "User"
                    }
                });

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_WithNonExistentUser_ReturnsNull()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "nonexistent@test.com",
                Password = "Test1234!"
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Kullanici, bool>>>()))
                .ReturnsAsync(Enumerable.Empty<Kullanici>());

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void PasswordHashing_UsesBCrypt()
        {
            // Verify that BCrypt produces proper hashes
            var password = "TestPassword123!";
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
            Assert.False(BCrypt.Net.BCrypt.Verify("WrongPassword", hash));
            Assert.StartsWith("$2", hash); // BCrypt hashes start with $2a$, $2b$, or $2y$
        }
    }
}
