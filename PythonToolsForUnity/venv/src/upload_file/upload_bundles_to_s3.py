import requests,random, threading, json,sys,re, time, os
import boto3
import boto3.s3

def upload():
    url = sys.argv[1]

    toUploadBundlePath = sys.argv[2]
    file = open(toUploadBundlePath)
    to_upload_bundle_data = json.load(file)

    AWS_ACCESS_KEY_ID = 'AKIAIPYUWEGMQSRSZK2Q'
    AWS_SECRET_ACCESS_KEY = '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'


    session = boto3.Session(aws_access_key_id=AWS_ACCESS_KEY_ID,aws_secret_access_key=AWS_SECRET_ACCESS_KEY)
    bucket_name = 'slots-bundle'
    s3 = session.resource('s3')

    AppName = ""
    AppVersion = ""
    Platform = ""

    for i in range(0, len(to_upload_bundle_data)):
        AppName = to_upload_bundle_data[i]["AppName"]
        AppVersion = to_upload_bundle_data[i]["AppVersion"]
        Platform = to_upload_bundle_data[i]["Platform"]

        bundle_path_at_s3 = "/StagingAssetBundle/" + AppVersion + "/" + Platform + "/" + to_upload_bundle_data[i]["Bundle"] + ".assetBundle"
        Path = to_upload_bundle_data[i]["Path"]

        info = 'Bundle: ' + bundle_path_at_s3 + 'app name: '+ AppName + ', AppVersion: ' + AppVersion + ', Path: ' + Path + ', Platform: ' + Platform

        print('start upload bundle, ' + info)
        s3.meta.client.upload_file(Path, bucket_name, bundle_path_at_s3)
        print('upload succeed.')


    refresh_session = requests.Session()
    refresh_url = "http://staging.bigbang.games/asset_bundles/import?token=bigbangBigBang!!!@@@"
    refresh_data = {'key': "/StagingAssetBundle/" + AppVersion + "/" + Platform + "/", 'bundle_id': AppName}

    r2 = refresh_session.post(url=refresh_url, data=refresh_data)



upload()