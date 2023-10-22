using Newtonsoft.Json;
using Octokit;
using WebHook.Interfaces;
using WebHook.Models;

namespace WebHook.Service
{
    public class ReceiverWebhook : IReceiveWebhook
    {
        private readonly IGitHubClientConfiguration _gitHubClientConfig;

        public ReceiverWebhook(IGitHubClientConfiguration gitHubClientConfig)
        {
            _gitHubClientConfig = gitHubClientConfig;
        }

        public async Task<string> SendRequest(string user, string repository, string token)
        {
            var github = _gitHubClientConfig.GetGitHubClient(token);

            var contributors = await github.Repository.GetAllContributors(user, repository);
            var issues = await github.Issue.GetAllForRepository(user, repository);

            var issuesResponse = GetIssuesResponse(issues);
            var contributorsResponse = GetContributorsResponse(contributors, user, repository, github);

            return GetResponse(user, repository, issuesResponse, contributorsResponse);
        }

        private string GetResponse(string user
            , string repository
            , List<IssueResponse> issues
            , List<ContributorResponse> contributors)
        {
            var response = new WebHookResponse
            {
                UserName = user,
                Repository = repository,
                Issues = issues,
                Contributors = contributors
            };

            return JsonConvert.SerializeObject(response);
        }

        private List<IssueResponse> GetIssuesResponse(IReadOnlyList<Issue> issues)
        {
            List<IssueResponse> issuesResponse = new List<IssueResponse>();
            List<ContributorResponse> contributorsResponse = new List<ContributorResponse>();

            var issueList = issues.ToList();
            issueList.ForEach(issue =>
            {
                issuesResponse.Add(new IssueResponse
                {
                    Title = issue.Title,
                    AuthorGit = issue.User.Name,
                    LabelsGit = issue.Labels.ToList()
                });
            });

            return issuesResponse;
        }

        private List<ContributorResponse> GetContributorsResponse(IReadOnlyList<RepositoryContributor> contributors
            , string user
            , string repository
            , GitHubClient github)
        {
            List<Octokit.User> contributorsUser = new List<Octokit.User>();
            IReadOnlyList<GitHubCommit> commits = new List<GitHubCommit>();
            List<ContributorResponse> contributorsResponse = new List<ContributorResponse>();

            var contributorsList = contributors.ToList();
            contributorsList.ForEach(async contributor =>
            {
                var commitsContributor = await github.Repository.Commit.GetAll(user, repository);
                var total = commitsContributor.Count();
                var teste = await github.User.Get(contributor.Login);

                contributorsResponse.Add(new ContributorResponse
                {
                    Name = user,
                    UserResponse = teste,
                    QtdCommits = total.ToString()
                });

                await Task.Delay(1000);
            });

            Task.WaitAll();

            return contributorsResponse;
        }
    }
}
