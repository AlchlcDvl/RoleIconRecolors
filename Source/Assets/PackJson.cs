using Newtonsoft.Json;

namespace FancyUI.Assets;

public class PackJson
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("repoOwner")]
    public string RepoOwner { get; set; }

    [JsonProperty("repoName")]
    public string RepoName { get; set; }

    [JsonProperty("branch")]
    public string Branch { get; set; }

    [JsonProperty("credits")]
    public string Credits { get; set; }

    [JsonIgnore]
    public bool FromMainRepo { get; set; }

    public string Link() => $"https://github.com/{RepoOwner}/{RepoName}";

    public string ApiLink() => $"https://api.github.com/repos/{RepoOwner}/{RepoName}/zipball/{Branch}";

    public void SetDefaults()
    {
        Branch ??= "main";
        RepoOwner ??= "AlchlcDvl";
        RepoName ??= "RoleIconRecolors";
        Name ??= RepoName;
        Credits ??= "";
    }
}