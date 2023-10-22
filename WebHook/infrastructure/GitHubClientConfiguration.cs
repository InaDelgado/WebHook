using Octokit;
using WebHook.Interfaces;

namespace WebHook.infrastructure
{
    public class GitHubClientConfiguration : IGitHubClientConfiguration
    {
        public GitHubClient GetGitHubClient(string token)
        {
            var github = new GitHubClient(new ProductHeaderValue("GitHubWebhook"));

            if (!string.IsNullOrEmpty(token))
            {
                var tokenAuth = new Credentials(token);
                github.Credentials = tokenAuth;
            }

            return github;
        }
    }
}
