import requests,random, threading, json,sys,re


for i in range(0, len(sys.argv)):
    print(sys.argv[i])


def upload():
    url = sys.argv[1]
    toUploadJsonPath = sys.argv[2]
    file = open(toUploadJsonPath)
    toUploadImgData = json.load(file)

    session = requests.Session()

    for i in range(0, len(toUploadImgData)):
        name = toUploadImgData[i]["Name"]
        placement = toUploadImgData[i]["Placement"]
        version = toUploadImgData[i]["Version"]
        locale = toUploadImgData[i]["Locale"]
        info = 'name: ' + name + ', placement: ' + placement + ', version: ' + str(version) + ', locale: ' + locale
        print('start upload img, ' + info)
        r = session.get(url + '/new')
        content = r.content.decode('utf-8')
        reg = r'<input type="hidden" name="authenticity_token" value="(.*)" />'
        pattern = re.compile(reg)
        authenticity_token = pattern.findall(content)[0]
        files = {'image[file]': open(toUploadImgData[i]["File"], 'rb')}
        data = {'image[name]': name, 'image[locale]': locale, 'image[placement]': placement,
                'authenticity_token': authenticity_token, 'image[version]': version}
        r = session.post(url=url, data=data, files=files)
        print('upload img succeed. ' + info)


upload()


