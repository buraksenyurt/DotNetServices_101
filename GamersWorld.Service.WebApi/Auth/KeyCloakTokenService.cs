using System.Security.Claims;
using System.Text.Json;

namespace GamersWorld.Service.WebApi.Auth;

public class KeycloakTokenService
    : IKeycloakTokenService
{
    private readonly ILogger<KeycloakTokenService> _logger;

    public KeycloakTokenService(ILogger<KeycloakTokenService> logger)
    {
        _logger = logger;
    }

    public void ProcessValidatedToken(ClaimsPrincipal? principal)
    {
        var userClaims = principal?.Claims.ToList();
        var identity = principal?.Identity as ClaimsIdentity;

        if (userClaims == null || identity == null)
        {
            _logger.LogWarning("Claims empty or identity null");
            return;
        }

        var realmAccessClaim = userClaims.FirstOrDefault(c => c.Type == "realm_access");

        if (realmAccessClaim != null)
        {
            var realmAccess = JsonSerializer.Deserialize<JsonElement>(realmAccessClaim.Value);
            if (realmAccess.TryGetProperty("roles", out var rolesElement))
            {
                foreach (var role in rolesElement.EnumerateArray())
                {
                    var roleName = role.GetString();
                    if (!string.IsNullOrEmpty(roleName))
                    {
                        _logger.LogInformation("User Role {}", roleName);
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                    }
                }
            }
        }
    }
}
