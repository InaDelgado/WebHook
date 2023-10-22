using Newtonsoft.Json;
using Octokit;

namespace WebHook.Models
{
    public class ContributorResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("user")]
        public User UserResponse { get; set; }
        [JsonProperty("qtd_commits")]
        public string QtdCommits { get; set; }
    }
}
