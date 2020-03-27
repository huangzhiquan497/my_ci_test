import requests,random, threading, json,sys,re, time, os, shutil

def find_latest_file(dir):
    file_lists = os.listdir(dir)
    file_lists.sort(key=lambda fn: os.path.getmtime(dir + "/" + fn)
                    if not os.path.isdir(dir + "/" + fn) else 0)

    file = os.path.join(dir, file_lists[-1])
    return file

def copy():
    fileFolderPath = sys.argv[1]
    latest_file_path = find_latest_file(fileFolderPath)

    tmpArr = latest_file_path.split('/')
    file_name = tmpArr[len(tmpArr) - 1]
    target_path = sys.argv[2] + '/' + file_name
    os.makedirs(target_path)

    try:
        shutil.copy(latest_file_path, target_path)
    except IOError as e:
        print("Unable to copy file. %s" % e)
    except:
        print("Unexpected error:", sys.exc_info())


copy()


