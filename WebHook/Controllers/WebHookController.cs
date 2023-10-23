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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRepositoryIssues([FromHeader(Name = "Authorization")] string token
            , string user, string repository)
        {
            try
            {
                var issues = await _receiverWebhook.SendRequest(user, repository, token);

                if (issues == null) return NotFound("GitHub Repository Not Found.");

                return Ok(issues);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error accessing issues: {ex.Message}");
            }
        }
    }
}