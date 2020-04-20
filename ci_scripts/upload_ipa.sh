export PROJECT_PATH=$(dirname $(dirname $0))
M_ARCHIVE_PATH=${PROJECT_PATH}/archive
M_EXPORT_PATH=${PROJECT_PATH}/ipa

xcrun altool --validate-app -f ${M_EXPORT_PATH}/Apps/Unity-iPhone.ipa -t ios --apiKey 79N8CA8YL2 --apiIssuer ef9f0531-4cba-4671-9b29-d44d61f11ba6 --verbose  --output-format xml

xcrun altool --upload-app -f ${M_EXPORT_PATH}/Apps/Unity-iPhone.ipa -t ios --apiKey 79N8CA8YL2 --apiIssuer ef9f0531-4cba-4671-9b29-d44d61f11ba6 --verbose  --output-format xml