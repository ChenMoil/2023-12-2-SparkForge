
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildHandler
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string targetPath)
    {
        if (target != BuildTarget.WebGL)
            return;
        var path = Path.Combine(targetPath, "Build/UnityLoader.js");
        var text = File.ReadAllText(path);
        text = text.Replace("UnityLoader.SystemInfo.mobile", "false");
        text = text.Replace("[\"Edge\", \"Firefox\", \"Chrome\", \"Safari\"].indexOf(UnityLoader.SystemInfo.browser) == -1", "false");
        File.WriteAllText(path, text);
    }

}
