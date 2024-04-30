namespace IconPacks;

public class PackJson
{
    public string Name { get; set; }
    public string RepoOwner { get; set; }
    public string RepoName { get; set; }
    public string Branch { get; set; }
    public string JsonName { get; set; }

    public string JsonLink() => $"{RawLink()}/{JsonName}.json";

    public string RawLink() => $"https://raw.githubusercontent.com/{RepoOwner}/{RepoName}/{Branch}";

    public string Link() => $"https://github.com/{RepoOwner}/{RepoName}";

    public void SetDefaults()
    {
        Branch ??= "main";
        RepoOwner ??= "AlchlcDvl";
        RepoName ??= "RoleIconRecolors";
        Name ??= RepoName;
        JsonName ??= Name;
    }
}