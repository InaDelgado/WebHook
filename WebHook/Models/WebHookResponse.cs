using Newtonsoft.Json;

namespace WebHook.Models
{
    public class WebHookResponse
    {
        [JsonProperty("user")]
        public string UserName { get; set; }
        [JsonProperty("repository")]
        public string Repository { get; set; }
        [JsonProperty("issues")]
        public List<IssueResponse> Issues { get; set; }
        public List<ContributorResponse> Contributors { get; set; }
    }
}
