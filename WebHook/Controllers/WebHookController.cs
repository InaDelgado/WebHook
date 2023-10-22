using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using WebHook.Models;

namespace WebHook.Controllers
{
    [ApiController]
    [Route("api/github")]
    public class WebHookController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly ReceiverWebhook _receiverWebhook;
        public WebHookController(IOptions<AppSettings> appSettingsAcessor, ReceiverWebhook receiverWebhook)
        {
            _appSettings = appSettingsAcessor.Value;
            _receiverWebhook = receiverWebhook;
        }

        [HttpGet("repository/issues")]
        public async Task<IActionResult> GetRepositoryIssues(string user, string repository)
        {
            try
            {
                var issues = await _receiverWebhook.SendRequest(user, repository, _appSettings.AccessToken);

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