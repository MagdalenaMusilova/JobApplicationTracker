using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace Test.Repository;

public class UserRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);
        var users = new List<User>
        {
            new() { Id = "user1", UserName = "alice", Email = "alice@test.com" },
            new() { Id = "user2", UserName = "bob", Email = "bob@test.com" }
        };
        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Id = "user1",
            UserName = "testuser",
            Email = "test@test.com"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync("user1");

        // Assert
        result.Should().NotBeNull();
        result!.UserName.Should().Be("testuser");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetByIdAsync("nonexistent");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_DeletesUserSuccessfully()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Id = "user1",
            UserName = "testuser",
            Email = "test@test.com"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteAsync("user1");

        // Assert
        result.Should().BeTrue();
        var deletedUser = await context.Users.FindAsync("user1");
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
    {
        // Arrange
        await using var context = CreateInMemoryContext();
        var repository = new UserRepository(context);

        // Act
        var result = await repository.DeleteAsync("nonexistent");

        // Assert
        result.Should().BeFalse();
    }
}