using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

public static class ExternalToolSelector {
    static readonly ProcessStartInfo vsCodeInfo = new ProcessStartInfo("C:/Users/Poletipo/AppData/Local/Programs/Microsoft VS Code/Code.exe"); //insert full path to file
    static readonly HashSet<System.Type> vsCodeTypes = new HashSet<System.Type>
    {
        typeof(UnityEngine.TextAsset), //hlsl is considered text
        typeof(UnityEngine.Shader)
    };

    [OnOpenAsset(1)]
    public static bool HandleOpenAsset(int instanceID, int line)
    {
        var asset = Selection.activeObject;
        var assetPath = AssetDatabase.GetAssetPath(asset);
        var assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
        if (vsCodeTypes.Contains(assetType)) //can check file extension instead
        {
            vsCodeInfo.Arguments = "\"" + Path.GetFullPath(assetPath) + "\""; //without "" will break at spaces
            Process.Start(vsCodeInfo);
            return true;
        }

        return false;
    }
}
