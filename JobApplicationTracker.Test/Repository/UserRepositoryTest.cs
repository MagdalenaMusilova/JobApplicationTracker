using FluentAssertions;
using JobApplicationTracker.Database;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Test.Repository;

public class UserRepositoryTest
{
    private static UserDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new UserDbContext(options);
    }

    /*[Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        await using var context = CreateContext();
        context.Users.AddRange(
            new User { Id = 1, Username = "alice", PasswordHash = "hash1" },
            new User { Id = 2, Username = "bob", PasswordHash = "hash2" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);

        var result = await repository.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        await using var context = CreateContext();
        context.Users.Add(new User { Id = 1, Username = "alice", PasswordHash = "hash1" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);

        var result = await repository.GetByIdAsync(1);

        result.Should().NotBeNull();
        result!.Username.Should().Be("alice");
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var result = await repository.AddAsync(new UserDo
        {
            Username = "alice",
            PasswordHash = "hash"
        });

        result.Id.Should().BeGreaterThan(0);
        result.Username.Should().Be("alice");

        context.Users.Should().ContainSingle(u => u.Username == "alice");
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        var result = await repository.UpdateAsync(new UserDo
        {
            Id = 999,
            Username = "updated",
            PasswordHash = "hash"
        });

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUser_WhenUserExists()
    {
        await using var context = CreateContext();
        context.Users.Add(new User { Id = 1, Username = "alice", PasswordHash = "hash1" });
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);

        var result = await repository.DeleteAsync(1);

        result.Should().BeTrue();
        context.Users.Should().BeEmpty();
    }*/
}