// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Security.Cryptography;
// using System.Text;
// using System.Threading;
// using Facebook.Unity.Editor;
// using Facebook.Unity.Settings;
// using I2.Loc;
// using LitJson;
// using QuickEngine.Extensions;
// using TheNextFlow.UnityPlugins;
// using UIWidgets;
// using UnityEditor;
// using UnityEditor.Android;
// using UnityEditor.Build.Content;
// using UnityEngine;
// using Object = System.Object;
// using UnityEngine.Networking;
//
// public class AppPacker
// {
//     private static void MoveBundlesToStreaming(List<string> bundleList, BuildTarget buildTarget)
//     {
//         bundleList.ForEach(x =>
//         {
//             var targetPath = StreamingBundlePath + "/" + x + ".assetBundle";
//
//             var outputPath = GetBundleOutputPath(x, buildTarget);
//
//             if (File.Exists(outputPath))
//             {
//                 if (File.Exists(targetPath))
//                     File.Delete(targetPath);
//
//                 File.Copy(outputPath,
//                     targetPath
//                 );
//             }
//         });
//     }
//
//     private static string StreamingBundlePath => Application.streamingAssetsPath + "/AssetBundle";
//
//     private static void CreateStreamingAssetPath()
//     {
//         if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets"))
//             AssetDatabase.CreateFolder("Assets", "StreamingAssets");
//
//         if (!AssetDatabase.IsValidFolder("Assets/StreamingAssets/AssetBundle"))
//             AssetDatabase.CreateFolder("Assets/StreamingAssets", "AssetBundle");
//     }
//
//     public static void ReimportCollidingPlugins()
//     {
// #if !UNITY_IOS
//         var plugins = new[]
//         {
//             "Assets/Plugins/EveryPlay/AndroidDynamicLibraries/armeabi-v7a/libeveryplay.so",
//             "Assets/Plugins/EveryPlay/AndroidDynamicLibraries/x86/libeveryplay.so"
//         };
//
//         foreach (var plugin in plugins)
//         {
//             var pluginImporter = AssetImporter.GetAtPath(plugin) as PluginImporter;
//             pluginImporter.SetCompatibleWithAnyPlatform(true);
//             pluginImporter.SaveAndReimport();
//         }
// #endif
//     }
//
//
//     private static Dictionary<string, BuildTarget> _targetMap = new Dictionary<string, BuildTarget>
//     {
//         ["iOS"] = BuildTarget.iOS,
//         ["Android"] = BuildTarget.Android,
//     };
//
//     private static List<string> usablePackageNames = new List<string>
//     {
//         "com.bbg.panda.slots",
//         "com.cirtron.panda.slots",
//         "com.bbg.dw.wealth_cn"
//     };
//
//     [MenuItem("Tools/Build/打常规安卓包")]
//     public static void PackAndriodApk_Routine()
//     {
//         var buildNumber = GetBuildVersion();
//         DetailDebugger.Log("Build number : " + buildNumber);
//         UpdateBuildVersion(buildNumber);
//
//         var packageName = GetEnvironmentVariable("package_name");
//         if (!usablePackageNames.Contains(packageName))
//             packageName = "com.bbg.panda.slots";
//
//         var versionStr = GetEnvironmentVariable("version_str");
//         if (versionStr.IsNullOrEmpty() || !versionStr.Contains('.'))
//             versionStr = Config.GetAppConfigInAssetDataBase().AppVersionStr;
//         else
//         {
//             var versions = versionStr.Split('.');
//
//             Config.GetAppConfigInAssetDataBase().MajorVersion = Convert.ToInt32(versions[0]);
//             Config.GetAppConfigInAssetDataBase().MinorVersion = Convert.ToInt32(versions[1]);
//             Config.GetAppConfigInAssetDataBase().MiniVersion = Convert.ToInt32(versions[2]);
//             SaveAssets(new UnityEngine.Object[] {Config.GetAppConfigInAssetDataBase()});
//         }
//
//         var apkName = packageName + "_b" + buildNumber + "_v" + versionStr;
//         var apkSaveFolder = Directory.GetParent(Application.dataPath).FullName
//                             + "/BuildApks";
//
//         if (Directory.Exists(apkSaveFolder))
//         {
//             Directory.Delete(apkSaveFolder, true);
//         }
//
//         Directory.CreateDirectory(apkSaveFolder);
//
//         var apkSavePath = apkSaveFolder + "/" + apkName + ".apk";
//
//         YWWSettings.Instance.isProductionVersion = true;
//         if (File.Exists(apkSavePath))
//             File.Delete(apkSavePath);
//
//         PlayerSettings.keystorePass = "bigbang123";
//         PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel18;
//         PlayerSettings.Android.keyaliasName = "slots.android.keystore";
//         PlayerSettings.Android.bundleVersionCode = AppInfo.VersionNumber;
//
//         PlayerSettings.keyaliasPass = "bigbang123";
//
//         SetPackage(packageName);
//
//         BuildApp(apkSavePath, BuildTarget.Android);
//         /*        Import_Languages_Coroutine(() =>
//         {
//             AssetDatabase.SaveAssets();
//             AssetDatabase.Refresh();
//             BuildApk(apkSavePath);
//
//         }, () => { throw new Exception("import I2 languages failed."); });*/
//     }
//
//
//     [MenuItem("Tools/Build/打常规iOS包")]
//     public static void PackIOS_Routine()
//     {
//         var buildNumber = GetBuildVersion();
//         DetailDebugger.Log("Build number : " + buildNumber);
//         UpdateBuildVersion(buildNumber);
//
//         var versionStr = GetEnvironmentVariable("version_str");
//         if (versionStr.IsNullOrEmpty() || !versionStr.Contains('.'))
//             versionStr = Config.Instance.AppConfig.AppVersionStr;
//         else
//         {
//             var versions = versionStr.Split('.');
//
//             Config.Instance.AppConfig.MajorVersion = Convert.ToInt32(versions[0]);
//             Config.Instance.AppConfig.MinorVersion = Convert.ToInt32(versions[1]);
//             Config.Instance.AppConfig.MiniVersion = Convert.ToInt32(versions[2]);
//         }
//
//         var packageName = GetEnvironmentVariable("package_name");
//         if (!usablePackageNames.Contains(packageName))
//             packageName = "com.cirtron.panda.slots";
//
//         var ipaSavePath = Directory.GetParent(Application.dataPath).FullName
//                           + "/BuildIpa/" + packageName;
//
//         if (Directory.Exists(ipaSavePath))
//         {
//             Directory.Delete(ipaSavePath, true);
//         }
//
//         Directory.CreateDirectory(ipaSavePath);
//
//         PlayerSettings.keystorePass = "bigbang123";
//
//         YWWSettings.Instance.AppleApplicationID = "1446205247";
//         SaveAssets(new UnityEngine.Object[] {YWWSettings.Instance});
//
//         SetPackage(packageName);
//
//         EditorUtility.SetDirty(YWWSettings.Instance);
//         AssetDatabase.Refresh();
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         BuildApp(ipaSavePath, BuildTarget.iOS);
//     }
//
//
//     private static void SetPackage(string packageName)
//     {
//         Config.GetAppConfigInAssetDataBase().PackageName = packageName;
//
//         EditorUtility.SetDirty(YWWSettings.Instance);
//         EditorUtility.SetDirty(Config.GetAppConfigInAssetDataBase());
//         AssetDatabase.Refresh();
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//
//
//         ThemeSwitcher.SwitchByReleaseName(packageName);
//     }
//
//
//     private static string GetBuildVersion()
//     {
//         var environmentVariables = Environment.GetEnvironmentVariables();
//         var originVersion = (Resources.Load("build_version") as TextAsset).text.ToInteger();
//         var buildNumber = environmentVariables.Contains("BUILD_NUMBER")
//             ? environmentVariables["BUILD_NUMBER"].ToString()
//             : (originVersion + 1).ToString();
//         return buildNumber;
//     }
//
//     private static string GetEnvironmentVariable(string key)
//     {
//         var environmentVariables = Environment.GetEnvironmentVariables();
//         return environmentVariables.Contains(key)
//             ? Convert.ToString(environmentVariables[key])
//             : "";
//     }
//
//     private static bool GetEnviromentBoolean(string key)
//     {
//         var environmentVariables = Environment.GetEnvironmentVariables();
//         return environmentVariables.Contains(key)
//             ? Convert.ToBoolean(environmentVariables[key])
//             : true;
//     }
//
//     private static void BuildApp(string savePath, BuildTarget buildTarget)
//     {
//         SetDefaultPathIcon();
//         SetAppIdsAndManifest();
//
//         PlayerSettings.productName = Config.GetCurPackageConfigInAssetDataBase().ProductName;
//         PlayerSettings.companyName = Config.GetCurPackageConfigInAssetDataBase().CompanyName;
//         PlayerSettings.applicationIdentifier = Config.GetAppConfigInAssetDataBase().PackageName;
//
//         var server = GetEnvironmentVariable("server_selection");
//
// //        if (!Config.GetCurPackageConfigInAssetDataBase().ServerOptions.Contains(server))
// //            server = "https://v2.51zcd.com/";
//
//         CheckAndSetServer(server, "production");
//
//         PlayerSettings.bundleVersion = Config.GetAppConfigInAssetDataBase().AppVersionStr;
//
//         ResetVersion(Config.GetAppConfigInAssetDataBase().BigVersionStr);
//
//         BMDataAccessor.RefreshAndSaveBundleData();
//
//         if (GetEnviromentBoolean("if_build_initial_bundles"))
//             PackInitialBundles(buildTarget);
//
//         ResetYWWBundleInitialSettings(Config.GetCurPackageConfigInAssetDataBase().InitialBundles);
//
//         SaveAssets(new UnityEngine.Object[]
//             {Config.GetAppConfigInAssetDataBase(), Config.GetCurPackageConfigInAssetDataBase()});
//
//         ReimportCollidingPlugins();
//
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//
//         Build(savePath, buildTarget);
//     }
//
//     private static void SaveAssets(UnityEngine.Object[] assets)
//     {
//         foreach (var asset in assets)
//         {
//             EditorUtility.SetDirty(asset);
//         }
//
//         AssetDatabase.Refresh();
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//     }
//
//
//     [MenuItem("Tools/Build/Set Icon")]
//     private static void SetDefaultPathIcon()
//     {
//         SetIcon("Assets/_ThemeArtworks/Images/icon.png");
//     }
//
//     private static void CheckAndSetServer(string server, string bundleServer)
//     {
// //        Config.GetCurPackageConfigInAssetDataBase().SelectedServerIndex =
// //            Config.GetCurPackageConfigInAssetDataBase().ServerOptions.IndexOf(server);
// //        Config.GetCurPackageConfigInAssetDataBase().SelectedBundleServerIndex =
// //            Config.GetCurPackageConfigInAssetDataBase().BundleServerOptions.IndexOf(bundleServer);
// //
// //        EditorUtility.SetDirty(Config.GetCurPackageConfigInAssetDataBase());
// //
// //        AssetDatabase.Refresh();
// //        AssetDatabase.SaveAssets();
// //        AssetDatabase.Refresh();
//     }
//
//     private static void UpdateBuildVersion(string buildNumber)
//     {
//         File.WriteAllText(Application.dataPath + "/_Data/Resources/build_version.txt", buildNumber);
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//     }
//
//     [MenuItem("Tools/Build/打安卓初始AssetBundle")]
//     public static void PackAndroidInitialBundles()
//     {
//         PackInitialBundles(BuildTarget.Android);
//         ResetYWWBundleInitialSettings(Config.GetCurPackageConfigInAssetDataBase().InitialBundles);
//     }
//
//     [MenuItem("Tools/Build/打iOS初始AssetBundle")]
//     public static void PackIOSInitialBundles()
//     {
//         PackInitialBundles(BuildTarget.iOS);
//         ResetYWWBundleInitialSettings(Config.GetCurPackageConfigInAssetDataBase().InitialBundles);
//     }
//
//
//     public static void PackInitialBundles(BuildTarget buildTarget)
//     {
//         if (Directory.Exists(Application.streamingAssetsPath + "/AssetBundle"))
//             Directory.Delete(Application.streamingAssetsPath + "/AssetBundle", true);
//
//         AssetDatabase.Refresh();
//         CreateStreamingAssetPath();
//         var initialBundles = Config.GetCurPackageConfigInAssetDataBase().InitialBundles;
//         BuildBundles(initialBundles);
//
//         MoveBundlesToStreaming(initialBundles, buildTarget);
//         AssetDatabase.Refresh();
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//     }
//
//     [MenuItem("Tools/Build/Set bundle hashs")]
//     public static void SetBundleHashs()
//     {
//         ResetYWWBundleInitialSettings(Config.GetCurPackageConfigInAssetDataBase().InitialBundles);
//     }
//
//     private static void ResetYWWBundleInitialSettings(List<string> initialBundles)
//     {
//         var bundleHashs = CalculateBundleHashs(initialBundles);
//
//         Config.GetCurPackageConfigInAssetDataBase().InitialBundles = bundleHashs.Keys.ToList();
//         var hashStringList = new List<string>(bundleHashs.Count);
//         for (int i = 0; i < bundleHashs.Count; i++)
//         {
//             hashStringList.Add("");
//         }
//
//         foreach (var bundleHashPair in bundleHashs)
//         {
//             var index = Config.GetCurPackageConfigInAssetDataBase().InitialBundles.IndexOf(bundleHashPair.Key);
//             hashStringList[index] = bundleHashPair.Value;
//         }
//
//         Config.GetCurPackageConfigInAssetDataBase().InitialBundleHashs = hashStringList;
//         EditorUtility.SetDirty(Config.GetCurPackageConfigInAssetDataBase());
//         AssetDatabase.Refresh();
//         AssetDatabase.SaveAssets();
//     }
//
//     private static Dictionary<string, string> CalculateBundleHashs(List<string> bundleList)
//     {
//         var hashDic = new Dictionary<string, string>();
//
//         bundleList.ForEach(x =>
//         {
//             var bundlePath = StreamingBundlePath + "/" + x + ".assetBundle";
//
//             DetailDebugger.Log(bundlePath);
//             if (File.Exists(bundlePath))
//             {
//                 var md5 = MD5.Create();
//                 var bytes = File.ReadAllBytes(bundlePath);
//
//                 var fileMd5Bytes = md5.ComputeHash(bytes);
//                 var fileMd5 = BitConverter.ToString(fileMd5Bytes).Replace("-", "").ToLower();
//                 hashDic.Add(x, fileMd5);
//             }
//         });
//
//         return hashDic;
//     }
//
//
//     static void Import_Languages_Coroutine(Action onFinished, Action onFailure)
//     {
//         var languageSource = Resources.Load<LanguageSourceAsset>("I2Languages").mSource;
//
//         var request = languageSource.Import_Google_CreateWWWcall(false, false);
//
//
//         var startTime = DateTime.Now;
//         while (!request.isDone)
//         {
//             if ((DateTime.Now - startTime).TotalSeconds > 120)
//             {
//                 throw new Exception("Download google doc time out");
//             }
//
//
//             Thread.Sleep(100);
//         }
//
//
//         //DetailDebugger.Log ("Google Result: " + www.text);
//         bool notError = String.IsNullOrEmpty(request.error);
//         string wwwText = null;
//
//         if (notError)
//         {
//             var bytes = request.downloadHandler.data;
//             wwwText = Encoding.UTF8.GetString(bytes, 0, bytes.Length); //www.text
//
//             bool isEmpty = String.IsNullOrEmpty(wwwText) || wwwText == "\"\"";
//
//             if (!isEmpty)
//             {
//                 var errorMsg = languageSource.Import_Google_Result(wwwText, eSpreadsheetUpdateMode.Replace, true);
//                 if (String.IsNullOrEmpty(errorMsg))
//                 {
//                     LocalizationManager.LocalizeAll(true);
//                     onFinished?.Invoke();
//                     DetailDebugger.Log("Done Google Sync");
//                 }
//                 else
//                 {
//                     onFailure?.Invoke();
//                 }
//             }
//             else
//             {
//                 onFailure?.Invoke();
//             }
//         }
//     }
//
//     private static void Build(string savePath, BuildTarget buildTarget)
//     {
//         var buildPlayerOptions = new BuildPlayerOptions();
//         buildPlayerOptions.scenes = new[]
//         {
//             "Assets/__Scenes/Start.unity",
//             "Assets/__Scenes/Entrance.unity",
//             "Assets/__Scenes/SceneLoader.unity"
//         };
//         buildPlayerOptions.locationPathName = savePath;
//         buildPlayerOptions.target = buildTarget;
//         buildPlayerOptions.options = BuildOptions.None;
//
//         if (GetEnviromentBoolean("is_develop_build"))
//         {
//             buildPlayerOptions.options = BuildOptions.Development | BuildOptions.ConnectWithProfiler |
//                                          BuildOptions.AllowDebugging;
//         }
//
//
//         BuildPipeline.BuildPlayer(buildPlayerOptions);
//     }
//
//
//     public static void BuildAseetBundlesThenUpload()
//     {
//         BuildBundlesThenUploadWithTarget(BuildConfiger.UnityBuildTarget);
//     }
//
//     private static void BuildBundlesThenUploadWithTarget(BuildTarget buildTarget)
//     {
//         var projectPath = Directory.GetParent(Application.dataPath).FullName;
//         ExecutePythonScript.Execute("git_check/set_update_bundles.py", projectPath);
//
//         var fs = new FileStream(projectPath + "/Builds/updated_bundles.json", FileMode.Open);
//         var sr = new StreamReader(fs);
//         var bundleNames = JsonMapper.ToObject(sr.ReadToEnd());
//
//         sr.Close();
//         sr.Dispose();
//         fs.Close();
//         fs.Dispose();
//
//         var prefabBundleNames = bundleNames["widget_bundles"].ToStringList();
//         var sceneBundleNames = bundleNames["scene_bundles"].ToStringList();
//         var bundleToBuildNames = prefabBundleNames.ToList();
//         bundleToBuildNames.AddRange(sceneBundleNames.Select(x =>
//             x.Substring(x.LastIndexOf('/') + 1).Replace(".unity", String.Empty)
//         ));
//
//         BuildAndUpload(buildTarget, bundleToBuildNames);
//     }
//
//     public static void BuildAndUpload(BuildTarget buildTarget, List<string> bundleToBuildNames)
//     {
//         var uploadServerOptions = Config.GetAppConfigInAssetDataBase().BundleServerIps;
//
//         var selection = EditorUtility.DisplayDialogComplex("选择bundle服务器", "请选择一个AssetBundle server用以上传",
//             uploadServerOptions[0], uploadServerOptions[1], "取消");
//
//         Debug.Log("selected asset bundle server index : " + selection);
//
//
//         if (selection < uploadServerOptions.Count)
//         {
//             var bundleServer = Config.GetAppConfigInAssetDataBase().BundleServerIps[selection];
//             Debug.Log("upload bundle server : " + bundleServer);
//             BuildBundles(bundleToBuildNames);
//             GenerateFileAndUpload(buildTarget, bundleToBuildNames, bundleServer);
//         }
//     }
//
//
//     public static void GenerateFileAndUpload(BuildTarget buildTarget, List<string> bundleToBuildNames,
//         string bundleServer)
//     {
//         GenerateUploadBundleIndexFile(buildTarget, bundleToBuildNames);
//         //UploadBundles(bundleServer);
//     }
//
//
//     private static string GetBundleOutputPath(string bundleName, BuildTarget target)
//     {
//         return Directory.GetParent(Application.dataPath).FullName + "/AssetBundle/" + target + "/" +
//                bundleName + "." + BuildConfiger.BundleSuffix;
//     }
//
//     private static void GenerateUploadBundleIndexFile(BuildTarget buildTarget, List<string> bundleToBuildNames)
//     {
//         var uploadBundlesListFilePath = Directory.GetParent(Application.dataPath).FullName + "/Builds";
//         if (!Directory.Exists(uploadBundlesListFilePath))
//         {
//             Directory.CreateDirectory(uploadBundlesListFilePath);
//         }
//
//         var uploadBundlesListFileName = uploadBundlesListFilePath + "/upload_bundles.txt";
//         if (File.Exists(uploadBundlesListFileName))
//             File.Delete(uploadBundlesListFileName);
//
//         var fs2 = new FileStream(uploadBundlesListFileName, FileMode.OpenOrCreate);
//         var sw = new StreamWriter(fs2);
//
//         var bundleDataList = new List<object>();
//         bundleToBuildNames.ForEach(x =>
//         {
//             var path = GetBundleOutputPath(x, buildTarget);
//
//             if (File.Exists(path))
//                 bundleDataList.Add(new
//                 {
//                     Bundle = x,
//                     AppName = Config.GetAppConfigInAssetDataBase().PackageName,
//                     AppVersion = AppInfo.VersionNumber.ToString(),
//                     Path = path,
//                     Platform = buildTarget.ToString(),
//                 });
//
//             DetailDebugger.Log("to uploaded bundle : " + GetBundleOutputPath(x, buildTarget));
//         });
//         sw.Write(JsonMapper.ToJson(bundleDataList));
//         sw.Close();
//         sw.Dispose();
//         fs2.Close();
//         fs2.Dispose();
//     }
//
//     private static void BuildBundles(List<string> bundleToBuildNames)
//     {
//         if (bundleToBuildNames.Count > 0)
//         {
//             BuildHelper.BuildBundles(bundleToBuildNames.ToArray());
//             BuildHelper.ExportBMDatasToOutput();
//         }
//
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//     }
//
//     public static void UploadBundles(string bundleServer)
//     {
//         var uploadBundlesListFileName =
//             Directory.GetParent(Application.dataPath).FullName + "/Builds/upload_bundles.txt";
//         ExecutePythonScript.Execute("upload_file/upload_bundles.py",
//             bundleServer,
//             uploadBundlesListFileName);
//     }
//
//
//     [MenuItem("Tools/Build/Log caching path")]
//     public static void LogCachingPath()
//     {
//         var cachingPathList = new List<string>();
//
//         Caching.GetAllCachePaths(cachingPathList);
//
//         cachingPathList.ForEach(x => { LogPathRecursively(x); });
//     }
//
//     [MenuItem("Tools/Log/Log transform hierarchy")]
//     public static void LogMessages()
//     {
//         Debug.LogWarning(Selection.activeGameObject.transform.GetHierarchy());
//     }
//
//     [MenuItem("Tools/Log/Log all usable games")]
//     public static void LogAllUsableGames()
//     {
//         QATestHacker.AllGameNames().ForEach(Debug.LogWarning);
//     }
//
//     private static void LogPathRecursively(string dir)
//     {
//         DetailDebugger.Log(dir);
//         foreach (var file in Directory.GetFiles(dir))
//         {
//             DetailDebugger.Log("file: " + file);
//         }
//
//         foreach (var subDir in Directory.GetDirectories(dir))
//         {
//             LogPathRecursively(subDir);
//         }
//     }
//
//     public static void SetIcon(string iconPath)
//     {
//         if (AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath) == null)
//         {
//             throw new Exception("icon not found!, icon name : " + iconPath);
//         }
//         else
//         {
//             var icon = (Texture2D) AssetDatabase.LoadAssetAtPath(iconPath, typeof(Texture2D));
//             Texture2D[] iosIcons =
//             {
//                 icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon, icon,
//                 icon,
//                 icon
//             };
//             Texture2D[] defaultIcon = {icon};
//             //Default ICON
//             PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, defaultIcon);
//
//             //IOS ICON
//             PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, iosIcons);
//
//
//             //Round Icon
//             var Roundicons = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Round);
//             for (var i = 0; i < Roundicons.Length; i++)
//             {
//                 Roundicons[i].SetTexture(icon);
//             }
//
//             PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Round, Roundicons);
//
//             //Legacy ICON
//             var Legacyicons = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Legacy);
//             for (var i = 0; i < Legacyicons.Length; i++)
//             {
//                 Legacyicons[i].SetTexture(icon);
//             }
//
//             PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, AndroidPlatformIconKind.Legacy, Legacyicons);
//         }
//     }
//
//     public static void SetAppIdsAndManifest()
//     {
//         // UnityEditor.FacebookEditor.ManifestMod.GenerateManifest();
//
//         //		if (YWWSettings.Instance.useFacebook) {
//         ManifestMod.GenerateManifest();
//         //		}
//
//         //		AppInfo.SetBundleOutputPath();
//         SetWechatAppID();
//
//         //		YWWSettings.Instance.
//
//         EditorUtility.SetDirty(YWWSettings.Instance);
//     }
//
//     public static void ResetVersion(string version)
//     {
//         var featureConfigAssetPath = AssetDatabase.GetAssetPath(FeatureConfig.Instance);
//         AssetDatabase.DeleteAsset(featureConfigAssetPath);
//         AssetDatabase.CopyAsset(FeatureConfig.GetCfgFilePath(version), featureConfigAssetPath);
//         AssetDatabase.Refresh();
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//
//         var versionArr = version.Split('.').Select(x => x.ToInteger()).ToList();
//
//         Config.GetAppConfigInAssetDataBase().MajorVersion = versionArr[0];
//         Config.GetAppConfigInAssetDataBase().MinorVersion = versionArr[1];
//         EditorUtility.SetDirty(YWWSettings.Instance);
//     }
//
//     public static void SetWechatAppID()
//     {
// #if UNITY_IOS
// 		TextReader reader = new StreamReader("Assets/_Refactored/_Modules/WechatUnity/ios_template.txt");
// 		if (reader == null) {
// 			Debug.LogError("reader fail");
// 			reader.Close();
// 			return;
// 		}
// 		string content = reader.ReadToEnd();
// 		reader.Close();
//
// 		content = content.Replace("{WECHAT_APP_ID_TEMPLATE}", Config.GetCurPackageConfigInAssetDataBase().WechatAppId);
// 		Debug.LogWarning("content: " + content);
// 		TextWriter writer =
//  new StreamWriter("Assets/_Refactored/_Modules/WechatUnity/Plugins/iOS/WechatUnityInterface.mm");
// 		if (writer == null) {
// 			Debug.LogError("writer fail");
// 			return;
// 		}
// 		writer.Write(content);
// 		writer.Flush();
// 		writer.Close();
// 		Debug.Log("SetWechatAppID success!");
//
// #endif
//     }
// }