export PROJECT_PATH=$(dirname $(dirname $0))
# 工程路径  
xcode_project_path=${PROJECT_PATH}/BuildIpa/hzq_ci_test
 
# build文件夹路径  
build_path=${PROJECT_PATH}/BuildIpa  
 
archive_path=${PROJECT_PATH}/Archive

rm -rf $archive_path
mkdir $archive_path


# 清理#
xcodebuild clean
echo "-------------------------clean-------------------------------------" 
 
# 编译工程  
cd $xcode_project_path  
xcodebuild || exit  

echo "-------------------------build  start-------------------------------------"
xcodebuild -project ${xcode_project_path}/Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release -allowProvisioningUpdates 
echo "-------------------------archive  start-------------------------------------"
xcodebuild -project ${xcode_project_path}/Unity-iPhone.xcodeproj -scheme Unity-iPhone -configuration Release archive -allowProvisioningUpdates -archivePath "${archive_path}/ci.xcarchive"
echo "------------------------exportArchive  start--------------------------------"
xcodebuild -exportArchive -allowProvisioningUpdates -archivePath "${archive_path}/ci.xcarchive" -exportPath "${archive_path}/ci.iap" -exportOptionsPlist ${xcode_project_path}/info.plist