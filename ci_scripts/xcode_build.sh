export PROJECT_PATH=$(dirname $(dirname $0))
# 工程路径  
xcode_project_path=${PROJECT_PATH}/BuildIpa/hzq_ci_test
 
# build文件夹路径  
build_path=${PROJECT_PATH}/BuildIpa  
 
archive_path=${PROJECT_PATH}/Archive
M_EXPORT_PATH=${PROJECT_PATH}/ipa
if [ ! -d "$archive_path" ];then
mkdir $archive_path
else
echo "文件夹已经存在"
fi

# 清理#
xcodebuild clean
echo "-------------------------clean-------------------------------------" 
 
# 编译工程  
cd $xcode_project_path  
xcodebuild || exit  

echo "-------------------------build  start-------------------------------------"
xcodebuild -project ${xcode_project_path}/Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release -allowProvisioningUpdates 
echo "-------------------------archive  start-------------------------------------"
xcodebuild -project ${xcode_project_path}/Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release -allowProvisioningUpdates -archivePath "${archive_path}.xcarchive"
echo "------------------------exportArchive  start--------------------------------"
xcodebuild -exportArchive -allowProvisioningUpdates -archivePath "${archive_path}.xcarchive" -exportPath ${M_EXPORT_PATH} -exportOptionsPlist ${xcode_project_path}/info.plist