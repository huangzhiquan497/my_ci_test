M_PROVISIONING_PROFILE="71ccbdf6-4562-451e-be64-11487f52fff4" #证书配置
M_CODE_SIGN_IDENTITY="iPhone Distribution: TECHNOLOGY IN PLAY LTD (K36DYL578K)" #证书配置
export PROJECT_PATH=$(dirname $(dirname $0))
M_XCODE_PATH=${PROJECT_PATH}/BuildIpa/hzq_ci_test  #XCODE工程目录路径
M_XCODE_NAME="Unity-iPhone"
M_ARCHIVE_PATH=${PROJECT_PATH}/archive
M_EXPORT_PATH=${PROJECT_PATH}/ipa
cd ${M_XCODE_PATH}

xcodebuild clean archive -scheme ${M_XCODE_NAME} -target ${M_XCODE_NAME} -archivePath ${M_ARCHIVE_PATH} -configuration Release PROVISIONING_PROFILE="${M_PROVISIONING_PROFILE}" CODE_SIGN_IDENTITY="${M_CODE_SIGN_IDENTITY}"

xcodebuild -exportArchive -archivePath "${M_ARCHIVE_PATH}.xcarchive" -exportPath "${M_EXPORT_PATH}" -exportOptionsPlist "Info.plist" -allowProvisioningUpdates