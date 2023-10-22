using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using WebHook.infrastructure;
using WebHook.Interfaces;
using WebHook.Models;

namespace WebHook.Controllers
{
    [ApiController]
    [Route("api/github")]
    public class WebHookController : ControllerBase
    {
        private readonly IGitHubClientConfiguration _gitHubClientConfig;
        private readonly IReceiveWebhook _receiverWebhook;
        public WebHookController(IOptions<AppSettings> appSettingsAcessor, IReceiveWebhook receiverWebhook)
        {
            _gitHubClientConfig = new GitHubClientConfiguration(appSettingsAcessor);
            _receiverWebhook = receiverWebhook;
        }

        [HttpGet("repository/issues")]
        public async Task<IActionResult> GetRepositoryIssues(string user, string repository)
        {
            try
            {
                var issues = await _receiverWebhook.SendRequest(user, repository);

                return Ok(issues);
            }
            catch (NotFoundException)
            {
                return NotFound("Repositório não encontrado no GitHub.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao acessar as issues: {ex.Message}");
            }
        }
    }
}