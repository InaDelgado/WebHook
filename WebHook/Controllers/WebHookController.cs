using Microsoft.AspNetCore.Mvc;
using Octokit;
using WebHook.Interfaces;

namespace WebHook.Controllers
{
    [ApiController]
    [Route("api/github")]
    public class WebHookController : ControllerBase
    {
        private readonly IReceiveWebhook _receiverWebhook;
        public WebHookController(IReceiveWebhook receiverWebhook)
        {
            _receiverWebhook = receiverWebhook;
        }

        [HttpGet("repository/issues")]
        public async Task<IActionResult> GetRepositoryIssues([FromHeader(Name = "Authorization")] string token
            , string user, string repository)
        {
            try
            {
                var issues = await _receiverWebhook.SendRequest(user, repository, token);

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