using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CreateAssetBundles
{
    [MenuItem("AssetBundle/Build")]
    static void BuildAllAssetBundlesWin()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        string mac = "Assets/StreamingAssets/Mac";
        if (!Directory.Exists(mac))
        {
            Directory.CreateDirectory(mac);
        }
        else
        {
            Directory.EnumerateFiles(mac, "*.*", SearchOption.AllDirectories).ToList().ForEach(File.Delete);
        }
        string win = "Assets/StreamingAssets/Win";
        if (!Directory.Exists(win))
        {
            Directory.CreateDirectory(win);
        }
        else
        {
            Directory.EnumerateFiles(win, "*.*", SearchOption.AllDirectories).ToList().ForEach(File.Delete);
        }
        BuildPipeline.BuildAssetBundles(mac, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
        BuildPipeline.BuildAssetBundles(win, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
