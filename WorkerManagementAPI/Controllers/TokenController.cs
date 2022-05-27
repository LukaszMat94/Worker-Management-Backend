using Microsoft.AspNetCore.Mvc;
using WorkerManagementAPI.Services.TokenService.Repository;

namespace WorkerManagementAPI.Controllers
{
    [Route("tokens")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenManager _tokenManager;

        public TokenController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _tokenManager.DeactivateCurrentAccessTokenAsync();

            return NoContent();
        }
    }
}
