using Microsoft.Extensions.Options;
using Octokit;
using WebHook.Interfaces;
using WebHook.Models;

namespace WebHook.infrastructure
{
    public class GitHubClientConfiguration : IGitHubClientConfiguration
    {
        private readonly AppSettings _appSettings;
        public GitHubClientConfiguration(IOptions<AppSettings> appSettingsAcessor)
        {
            _appSettings = appSettingsAcessor.Value;
        }

        public GitHubClient GetGitHubClient()
        {
            var github = new GitHubClient(new ProductHeaderValue("GitHubWebhook"));

            if (!string.IsNullOrEmpty(_appSettings.AccessToken))
            {
                var tokenAuth = new Credentials(_appSettings.AccessToken);
                github.Credentials = tokenAuth;
            }

            return github;
        }
    }
}
