M_PROVISIONING_PROFILE="11b48f0c-04af-46c0-9fbd-2d12529a3b5c" #证书配置
M_CODE_SIGN_IDENTITY="iPhone Distribution: TECHNOLOGY IN PLAY LTD (K36DYL578K)" #证书配置
DEVELOPMENT_TEAM="K36DYL578K"
export PROJECT_PATH=$(dirname $(dirname $0))
#M_XCODE_PATH=${PROJECT_PATH}/BuildIpa/hzq_ci_test  #XCODE工程目录路径
#M_XCODE_NAME="Unity-iPhone"
#M_ARCHIVE_PATH=${PROJECT_PATH}/archive
#M_EXPORT_PATH=${PROJECT_PATH}/ipa
#cd ${M_XCODE_PATH}

#xcodebuild clean archive -scheme ${M_XCODE_NAME} -target ${M_XCODE_NAME} -archivePath ${M_ARCHIVE_PATH} -configuration Release DEVELOPMENT_TEAM="${DEVELOPMENT_TEAM}" CODE_SIGN_IDENTITY="${M_CODE_SIGN_IDENTITY}" PROVISIONING_PROFILE="${M_PROVISIONING_PROFILE}" 

#xcodebuild -exportArchive -archivePath "${M_ARCHIVE_PATH}.xcarchive" -exportPath "${M_EXPORT_PATH}" -exportOptionsPlist "Info.plist"

#!/bin/sh
# 参数判断  
 
# 工程路径  
xcode_project_path=${PROJECT_PATH}/BuildIpa/hzq_ci_test
 
# build文件夹路径  
build_path=${PROJECT_PATH}/BuildIpa  
 
archive_path=${build_path}/Archive/AutoBuild.xcarchive
 
# 清理#
xcodebuild clean
echo "-------------------------clean-------------------------------------" 
 
# 编译工程  
cd $xcode_project_path  
xcodebuild || exit  
 
echo "-------------------------archive  start-------------------------------------"
 
xcodebuild archive \
-project ${xcode_project_path}/Unity-iPhone.xcodeproj \
-scheme Unity-iPhone \
-configuration Release \
-archivePath ${archive_path} DEVELOPMENT_TEAM="${DEVELOPMENT_TEAM}" CODE_SIGN_IDENTITY="${M_CODE_SIGN_IDENTITY}" PROVISIONING_PROFILE="${M_PROVISIONING_PROFILE}" 

 
 
echo "-------------------------archive  End---------------------------------------"
 
echo "------------------------exportArchive  start--------------------------------"

 
xcodebuild -exportArchive \
-exportOptionsPlist ${xcode_project_path}/info.plist \
-archivePath ${archive_path} \
-exportPath ${xcode_project_path}