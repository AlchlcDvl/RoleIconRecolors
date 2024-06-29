namespace FancyUI;

public class Asset
{
    public string Name { get; set; }
    public string FileType { get; set; }

    public string FileName() => $"{Name}.{FileType}";
}

public class FolderAsset
{
    public string Name { get; set; }
    public Asset[] Assets { get; set; }
}

public class ModAsset
{
    public string Name { get; set; }
    public FolderAsset[] Folders { get; set; }
    public Asset[] Assets { get; set; }
}

public class JsonItem
{
    public ModAsset[] ModAssets { get; set; }
    public Asset[] Assets { get; set; }
}