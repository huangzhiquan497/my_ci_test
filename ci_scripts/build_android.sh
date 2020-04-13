 echo "start"  
 
#UNITY程序的路径
UNITY_PATH=/Volumes/ExtSSD/UnityApp/2019.3.9f1/Unity.app/Contents/MacOS/Unity
 
#游戏程序路径
export PROJECT_PATH=$(dirname $(dirname $0))

echo $PROJECT_PATH

export logPath=$(dirname $(dirname $0))/BuildLog/last_build_android.log

#在Unity中构建apk
$UNITY_PATH -quit -batchmode -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildForAndroid -logFile $logPath
 
echo "Apk生成完毕"