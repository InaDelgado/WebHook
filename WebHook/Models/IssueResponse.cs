using Newtonsoft.Json;
using Octokit;

namespace WebHook.Models
{
    public class IssueResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string AuthorGit { get; set; }
        [JsonProperty("label")]
        public List<Label> LabelsGit { get; set; }
    }
}
