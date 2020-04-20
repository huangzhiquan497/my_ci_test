using System;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessBuild
{
    // Start is called before the first frame update
    [PostProcessBuild(20000)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            //project settings

            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject proj = new PBXProject();

            proj.ReadFromString(File.ReadAllText(projPath));
            string target = proj.GetUnityMainTargetGuid();
            
            //proj.SetBuildProperty (target, "DEVELOPMENT_TEAM",  "9BXSA4V34W");
            proj.SetBuildProperty(target, "DEVELOPMENT_TEAM", "K36DYL578K");
            File.WriteAllText(projPath, proj.WriteToString());
        }
    }
}