using System.Security.Claims;
using ELBORAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace ELBORAI.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? KeycloakUserId =>
      _httpContextAccessor.HttpContext?
          .User?
          .FindFirst("sub")?
          .Value;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?
            .User?
            .Identity?
            .IsAuthenticated ?? false;

    public bool IsInRole(string role) =>
        _httpContextAccessor.HttpContext?
            .User?
            .IsInRole(role) ?? false;
}