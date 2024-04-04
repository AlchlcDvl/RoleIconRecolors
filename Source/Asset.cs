namespace IconPacks;

public class Asset
{
    public string Name { get; set; }
    public string Folder { get; set; }
    public string Pack { get; set; }
    public string FileType { get; set; }

    public string FilePath()
    {
        if (Pack is "Vanilla" or "BTOS2" || string.IsNullOrWhiteSpace(Folder) || string.IsNullOrWhiteSpace(Pack))
            return Path.Combine(AssetManager.ModPath, Pack, $"{Name}.{FileType}");
        else
            return Path.Combine(AssetManager.ModPath, Pack, Folder, $"{Name}.{FileType}");
    }

    public string FolderPath()
    {
        if (Pack is "Vanilla" or "BTOS2" || string.IsNullOrWhiteSpace(Folder) || string.IsNullOrWhiteSpace(Pack))
            return Path.Combine(AssetManager.ModPath, Pack);
        else
            return Path.Combine(AssetManager.ModPath, Pack, Folder);
    }

    public string DownloadLink()
    {
        if (Pack is "Vanilla" or  "BTOS2" || string.IsNullOrWhiteSpace(Folder) || string.IsNullOrWhiteSpace(Pack))
            return $"{Pack}/{Name}.{FileType}";
        else
            return $"{Pack}/{Folder}/{Name}.{FileType}";
    }
}