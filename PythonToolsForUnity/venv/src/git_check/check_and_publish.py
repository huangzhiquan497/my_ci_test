import shutil,os, sys, glob, json, re
from git import *


def find_latest_file(dir):
    file_lists = os.listdir(dir)
    file_lists.sort(key=lambda fn: os.path.getmtime(dir + "/" + fn)
                    if not os.path.isdir(dir + "/" + fn) else 0)

    file = os.path.join(dir, file_lists[-1])
    return file

def copy(source_path, target_path):
    latest_file_path = find_latest_file(source_path)
    tmpArr = latest_file_path.split('/')
    file_name = tmpArr[len(tmpArr) - 1]

    if not os.path.exists(target_path):
        os.makedirs(target_path)
    target_path = target_path + '/' + file_name

    try:
        shutil.copy(latest_file_path, target_path)
    except IOError as e:
        print("Unable to copy file. %s" % e)
    except:
        print("Unexpected error:", sys.exc_info())

def check_and_publish():
    project_path = sys.argv[1]
    print('project path ' + project_path)
    build_apk_folder = sys.argv[2]
    print('build apk folder ' + build_apk_folder)
    ci_publish_path = sys.argv[3]
    print('cu publish path ' + ci_publish_path)
    repo = Repo(project_path)
    assert (repo.head.commit != None)

    is_publish = (str(sys.argv[4]) == 'true')
    print('is publish ' + str(is_publish))
    commit_message = str(repo.head.commit.message)
    is_publish |= commit_message.__contains__('@@@publish')

    if is_publish:
        copy(build_apk_folder, ci_publish_path)


check_and_publish()
