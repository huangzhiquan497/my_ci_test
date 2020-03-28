using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ProjectBuild : Editor
{
    private static string PackageName => "hzq_ci_test"; // todo get package name
    private static string AppVersion => "202008"; // todo

    private static readonly string _build_version_file =
        Directory.GetCurrentDirectory() + "/BuildData/build_version.txt";

    private static string GetBuildVersion()
    {
        if (!File.Exists(_build_version_file)) return "100";

        var text = File.ReadAllText(_build_version_file);

        var environmentVariables = Environment.GetEnvironmentVariables();
        var originVersion = text.ToInteger();
        var buildNumber = environmentVariables.Contains("BUILD_NUMBER")
            ? environmentVariables["BUILD_NUMBER"].ToString()
            : (originVersion + 1).ToString();
        return buildNumber;
    }

    private static void UpdateBuildVersion(string buildNumber)
    {
        if (!File.Exists(_build_version_file))
        {
            try
            {
                File.Create(_build_version_file);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }


        File.WriteAllText(_build_version_file, buildNumber);
    }

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
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildTargetGroup.Android;
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        }


        var buildVersion = GetBuildVersion();
        UpdateBuildVersion(buildVersion);

        var dir = Path.Combine(Directory.GetCurrentDirectory(), "BuildApks");

        if (Directory.Exists(dir)) Directory.Delete(dir, true);

        Directory.CreateDirectory(dir);


        var apkName = $"{PackageName}_v{AppVersion}_b{buildVersion}.apk";

        var path = dir + "/" + apkName;

        if (File.Exists(path)) File.Delete(path);
        Build(path, BuildTarget.Android);
    }

    public static void BuildForIos()
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildTargetGroup.iOS;
            EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        }


        var buildVersion = GetBuildVersion();
        UpdateBuildVersion(buildVersion);

        var dir = Path.Combine(Directory.GetCurrentDirectory(), "BuildIpa");

        var path = dir + "/" + PackageName;

        if (Directory.Exists(dir)) Directory.Delete(dir, true);

        Directory.CreateDirectory(dir);

        Build(path, BuildTarget.iOS);
    }

    static void Build(string savePath, BuildTarget buildTarget)
    {
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetBuildScenes(),
            locationPathName = savePath,
            target = buildTarget,
            options = BuildOptions.None
        };

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}