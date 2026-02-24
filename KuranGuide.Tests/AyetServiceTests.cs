using KuranGuide.Application.Interfaces.Repositories;
using KuranGuide.Application.Services;
using KuranGuide.Domain.Entities;
using Moq;
using System.Linq.Expressions;

namespace KuranGuide.Tests
{
    public class AyetServiceTests
    {
        private readonly Mock<IAyetRepository> _mockRepo;
        private readonly AyetService _service;

        public AyetServiceTests()
        {
            _mockRepo = new Mock<IAyetRepository>();
            _service = new AyetService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAyetler()
        {
            // Arrange
            var ayetler = new List<Ayet>
            {
                new Ayet { Id = 1, AyetNo = 1, Meal = "Meal 1", SureId = 1 },
                new Ayet { Id = 2, AyetNo = 2, Meal = "Meal 2", SureId = 1 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(ayetler);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsAyet()
        {
            // Arrange
            var ayet = new Ayet { Id = 1, AyetNo = 1, Meal = "Test Meal", SureId = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ayet);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Ayet)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_WithExistingAyet_ReturnsTrue()
        {
            // Arrange
            var ayet = new Ayet { Id = 1, AyetNo = 1, Meal = "Test" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ayet);
            _mockRepo.Setup(r => r.DeleteAsync(ayet)).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(ayet), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingAyet_ReturnsFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Ayet)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Ayet>()), Times.Never);
        }

        [Fact]
        public async Task SearchAsync_WithValidQuery_ReturnsMatchingAyetler()
        {
            // Arrange
            var ayetler = new List<Ayet>
            {
                new Ayet { Id = 1, Meal = "Allah rahmet eder", SureId = 1 },
                new Ayet { Id = 2, Meal = "Sabir gosterenler", SureId = 1 },
                new Ayet { Id = 3, Meal = "Allah affedicidir", SureId = 2 }
            };

            _mockRepo.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Ayet, bool>>>()))
                .ReturnsAsync(ayetler.Where(a => a.Meal.Contains("Allah")).ToList());

            // Act
            var result = await _service.SearchAsync("Allah");

            // Assert
            Assert.Equal(2, result.Count());
        }
    }
}
