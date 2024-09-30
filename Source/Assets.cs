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

    public int Count()
    {
        var result = 0;
        result += Assets?.Length ?? 0;

        if (ModAssets != null)
        {
            foreach (var modAsset in ModAssets)
            {
                result += modAsset.Assets?.Length ?? 0;
                modAsset.Folders?.ForEach(x => result += x.Assets?.Length ?? 0);
            }
        }

        return result;
    }
}