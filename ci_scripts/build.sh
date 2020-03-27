if [ $# != 1 ];then  
    echo "需要一个参数。 参数是build version"  
    exit     
fi  
 
UNITY程序的路径#
UNITY_PATH=/Volumes/ExtSSD/UnityApp/2019.3.5f1/Unity.app/Contents/MacOS/Unity
 
游戏程序路径#
export PROJECT_PATH=$(dirname $(dirname $0))

echo $PROJECT_PATH
echo "version = $version"
 
在Unity中构建apk#
$UNITY_PATH -projectPath $PROJECT_PATH -executeMethod ProjectBuild.BuildForAndroid version-"$version" -quit
 
echo "Apk生成完毕"