﻿using Oikono.Application.Authentication.Commands.Refresh;
using Oikono.Application.Common.Interfaces.Persistence;
using Oikono.Application.Common.Interfaces.Security;
using Oikono.Domain.Common.Specification;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Http;
using Errors = Oikono.Domain.Users.Errors.Errors;

namespace Oikono.Application.UnitTests.Authentication.Commands;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
    private readonly Mock<IJwtTokenProvider> _jwtTokenProvider = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    [Fact]
    public async Task Handle_Returns_RefreshTokenExpired_Error_When_ActiveRefreshToken_Is_Expired()
    {
        // Arrange
        var userId = new UserId(Guid.NewGuid());
        var user = new Mock<User>("Firstname", "Lastname", "Email", "Password");

        user.SetupGet(u => u.ActiveRefreshToken).Returns((RefreshToken)null);

        _userRepository
            .Setup(repository =>
                repository.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Specification<User, UserId>>()))
            .ReturnsAsync(user.Object);

        var commandHandler = new RefreshTokenCommandHandler(_refreshTokenRepository.Object, _userRepository.Object,
            _jwtTokenProvider.Object);
        var command = new RefreshTokenCommand("token", userId);

        // Act
        var result = await commandHandler.Handle(command, new CancellationToken());

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.Authentication.RefreshTokenExpired);
    }

    [Fact]
    public async Task Handle_Returns_UserNotFound_Error_When_User_Is_Not_Found()
    {
        // Arrange
        var mockUserId = new UserId(Guid.NewGuid());

        _userRepository
            .Setup(repository =>
                repository.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Specification<User, UserId>>()))
            .ReturnsAsync((User)null);

        var commandHandler = new RefreshTokenCommandHandler(_refreshTokenRepository.Object, _userRepository.Object,
            _jwtTokenProvider.Object);
        var command = new RefreshTokenCommand("token", mockUserId);

        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(Errors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Returns_Success_When_ActiveRefreshToken_Is_Not_Expired()
    {
        // Arrange
        var userId = new UserId(Guid.NewGuid());
        var user = new Mock<User>("Firstname", "Lastname", "Email", "Password");
        var refreshToken = new RefreshToken(userId);

        user.SetupGet(u => u.ActiveRefreshToken).Returns(refreshToken);

        _userRepository
            .Setup(repository =>
                repository.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Specification<User, UserId>>()))
            .ReturnsAsync(user.Object);

        var commandHandler = new RefreshTokenCommandHandler(_refreshTokenRepository.Object, _userRepository.Object,
            _jwtTokenProvider.Object);
        var command = new RefreshTokenCommand(refreshToken.Token, userId);

        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
    }
}