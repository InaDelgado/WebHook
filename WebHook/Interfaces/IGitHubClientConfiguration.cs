using Octokit;

namespace WebHook.Interfaces
{
    public interface IGitHubClientConfiguration
    {
        public GitHubClient GetGitHubClient(string token);
    }
}
