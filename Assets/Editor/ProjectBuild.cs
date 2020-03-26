using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ProjectBuild : Editor
{
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }

        return names.ToArray();
    }

    public static void BuildForAndroid()
    {
        Function.DeleteFolder(Path.Combine(Directory.GetCurrentDirectory(), "Build"));


        // Function.CopyDirectory(Application.dataPath + "/my_ci-test", Application.dataPath + "/Plugins/Android");
        // PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "USE_SHARE");


        string path = Path.Combine(Directory.GetCurrentDirectory(), "Build") + "/" + Function.Version.Replace(".", "") +
                      ".apk";
        BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
    }
}