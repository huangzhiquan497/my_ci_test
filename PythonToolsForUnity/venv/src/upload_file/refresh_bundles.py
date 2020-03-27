import requests,random, threading, json,sys,re, time, os


AppVersion = '20190700'
Platform = 'Android'
AppName = 'com.bbg.panda.slots'

refresh_session = requests.Session()
refresh_url = "http://app.bigbang.games/asset_bundles/import?token=bigbangBigBang!!!@@@"
refresh_data = {'key': "AssetBundle/" + AppVersion + "/" + Platform + "/", 'bundle_id': AppName}


r2 = refresh_session.post(url=refresh_url, data=refresh_data)