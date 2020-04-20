#M_PROVISIONING_PROFILE="a8efop6a-33pt-5522-pokq-52d6d800aubv"#证书配置
#M_CODE_SIGN_IDENTITY="iPhone Distribution: XXXXXX Technology INC. (XXXXXX)"#证书配置
export PROJECT_PATH=$(dirname $(dirname $0))
M_XCODE_PATH=${PROJECT_PATH}/BuildIpa/hzq_ci_test  #XCODE工程目录路径
M_XCODE_NAME="Unity-iPhone"
M_ARCHIVE_PATH=${PROJECT_PATH}/archive
M_EXPORT_PATH=${PROJECT_PATH}/ipa
cd ${M_XCODE_PATH}

xcodebuild clean archive -scheme ${M_XCODE_NAME} -target ${M_XCODE_NAME} -archivePath ${M_ARCHIVE_PATH} -configuration Release
#PROVISIONING_PROFILE="${M_PROVISIONING_PROFILE}" CODE_SIGN_IDENTITY="${M_CODE_SIGN_IDENTITY}"

xcodebuild -exportArchive -archivePath "${M_ARCHIVE_PATH}.xcarchive" -exportPath "${M_EXPORT_PATH}" -exportOptionsPlist "Info.plist" -allowProvisioningUpdates