namespace IconPacks;

public class Asset
{
    public string Name { get; set; }
    public string Mod { get; set; }
    public string Folder { get; set; }
    public string Pack { get; set; }
    public string FileType { get; set; }

    public string FilePath() => Path.Combine(FolderPath(), $"{Name}.{FileType}");

    public string FolderPath()
    {
        var path = Path.Combine(AssetManager.ModPath, Pack);

        if (Pack is "Vanilla" or "BTOS2")
            return path;
        else
            return Path.Combine(path, Mod, Folder);
    }

    public string DownloadLink()
    {
        if (Pack is "Vanilla" or  "BTOS2")
            return $"{Pack}/{Name}.{FileType}";
        else
            return $"{Pack}/{Mod}/{Folder}/{Name}.{FileType}";
    }
}