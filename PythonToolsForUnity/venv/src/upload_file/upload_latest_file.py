import requests,random, threading, json,sys,re, time, os
import boto3
import boto3.s3

def find_latest_file(dir):
    file_lists = os.listdir(dir)
    file_lists.sort(key=lambda fn: os.path.getmtime(dir + "/" + fn)
                    if not os.path.isdir(dir + "/" + fn) else 0)

    file = os.path.join(dir, file_lists[-1])
    return file

def upload():
    fileFolderPath = sys.argv[1]
    latest_file_path = find_latest_file(fileFolderPath)
    tmpArr = latest_file_path.split('/')
    file_name = tmpArr[len(tmpArr) - 1]


    AWS_ACCESS_KEY_ID = 'AKIAIPYUWEGMQSRSZK2Q'
    AWS_SECRET_ACCESS_KEY = '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'


    session = boto3.Session(aws_access_key_id=AWS_ACCESS_KEY_ID,aws_secret_access_key=AWS_SECRET_ACCESS_KEY)
    bucket_name = 'bbg-jp-apks'
    bucket_location = session.client('s3').get_bucket_location(Bucket=bucket_name)

    s3 = session.resource('s3')
    with open(latest_file_path, 'rb') as f:
        s3.Object(bucket_name, file_name).upload_fileobj(f, ExtraArgs={'ACL':'public-read'})

    upload_url = "https://s3-{0}.amazonaws.com/{1}/{2}".format(bucket_location['LocationConstraint'],
                                                               bucket_name,
                                                               file_name)

    print(upload_url)

    return upload_url



upload()


