using System.Security.Claims;

namespace GamersWorld.Service.WebApi.Auth;

public interface IKeycloakTokenService
{
    void ProcessValidatedToken(ClaimsPrincipal? principal);
}
