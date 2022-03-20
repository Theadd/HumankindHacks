using System.Linq;
using UnityEngine;

namespace AnN3x.HumankindLib;

public class AssetHunter
{
    public static AssetBundle[] GetAssetBundlesContaining(string assetPath)
    {
        var bundles = AssetBundle.GetAllLoadedAssetBundles()
            .Where(b => b.Contains(assetPath))
            .ToArray();

        return bundles;
    }
}
