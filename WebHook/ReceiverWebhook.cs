using Newtonsoft.Json;
using Octokit;
using WebHook.Interfaces;
using WebHook.Models;

namespace WebHook
{
    public class ReceiverWebhook : IReceiveWebhook
    {
        private readonly IGitHubClientConfiguration _gitHubClientConfig;

        public ReceiverWebhook(IGitHubClientConfiguration gitHubClientConfig)
        {
            _gitHubClientConfig = gitHubClientConfig;
        }

        public async Task<string> SendRequest(string user, string repository)
        {
            var github = _gitHubClientConfig.GetGitHubClient();

            var contributors = await github.Repository.GetAllContributors(user, repository);
            var issues = await github.Issue.GetAllForRepository(user, repository);

            var issuesResponse = GetIssuesResponse(issues);
            var contributorsResponse = GetContributorsResponse(contributors, repository, github);

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
            , string repository
            , GitHubClient github)
        {
            List<string> loginContributor = new List<string>();
            List<User> contributorsUser = new List<User>();
            IReadOnlyList<GitHubCommit> commits = new List<GitHubCommit>();
            List<ContributorResponse> contributorsResponse = new List<ContributorResponse>();

            var contributorsList = contributors.ToList();
            contributorsList.ForEach(contributor =>
            {
                loginContributor.Add(contributor.Login);
            });

            loginContributor.ForEach(async login =>
            {
                var user = await github.User.Get(login);
                contributorsUser.Add(user);
            });

            contributorsUser.ForEach(async user =>
            {
                var commitsContributor = await github.Repository.Commit.GetAll(user.Name, repository);
                var total = commitsContributor.Count();

                contributorsResponse.Add(new ContributorResponse
                {
                    Name = user.Name,
                    UserResponse = user,
                    QtdCommits = total.ToString()
                });
            });

            return  contributorsResponse;
        }
    }
}
