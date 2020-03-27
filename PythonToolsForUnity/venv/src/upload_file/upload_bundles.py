import requests,random, threading, json,sys,re, time, os, hashlib


for i in range(0, len(sys.argv)):
    print(sys.argv[i])


# def md5sum(filename):
#     fd = open(filename,"r")
#     fcont = fd.r
#     fd.close()
#     fmd5 = hashlib.md5(fcont)
#     return fmd5


def upload():
    url = sys.argv[1]

    toUploadBundlePath = sys.argv[2]
    file = open(toUploadBundlePath)
    to_upload_bundle_data = json.load(file)

    session = requests.Session()

    for i in range(0, len(to_upload_bundle_data)):
        Bundle = to_upload_bundle_data[i]["Bundle"]
        AppName = to_upload_bundle_data[i]["AppName"]
        AppVersion = to_upload_bundle_data[i]["AppVersion"]
        Path = to_upload_bundle_data[i]["Path"]
        Platform = to_upload_bundle_data[i]["Platform"]
        info = 'Bundle: ' + Bundle + 'app name: '+ AppName + ', AppVersion: ' + AppVersion + ', Path: ' + Path + ', Platform: ' + Platform
        print('start upload bundle, ' + info)
        full_url = url + "/new" + '?token=bigbangBigBang!!!@@@'

        r = session.get(full_url)
        content = r.content.decode('utf-8')
        reg = r'<input type="hidden" name="authenticity_token" value="(.*)" />'
        pattern = re.compile(reg)
        authenticity_token = pattern.findall(content)[0]

        files = {'asset_bundle[file]': open(Path, 'rb')}
        data = {'asset_bundle[bundle_id]': AppName, 'asset_bundle[app_version]': AppVersion, 'asset_bundle[platform]': Platform,
                'authenticity_token': authenticity_token}
        r2 = session.post(url=url + '?token=bigbangBigBang!!!@@@', data=data, files=files)
        print('upload bundle succeed. ' + info)




upload()


