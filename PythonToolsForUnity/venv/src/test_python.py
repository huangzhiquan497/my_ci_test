import os
import shutil
import sys


def find_last_file(dir):
    file_list = os.listdir(dir)
    print(file_list)
    file_list.sort()
    print(file_list)

    file = os.path.join(dir, file_list[-1])
    print(file)
    return file


def get_target_path():
    print("start")
    files = sys.argv[0]
    print(files)
    file = files

    for index in range(len(files)):
        if 'PythonToolsForUnity' in file:
            file = os.path.dirname(file)
    print(file)
    return file


def copy():
    print("copy")
    target_path = get_target_path()
    print(target_path)
    l_file = target_path.__add__("/Build/")
    r_file = target_path.__add__("/Target_path/")
    if not os.path.isdir(r_file):
        os.makedirs(r_file)
    target_file = find_last_file(l_file)

    try:
        shutil.copy(target_file, r_file)
    except IOError as e:
        print("unable to copy file. %s" % e)
    except:
        print("Unexpected error:", sys.exc_info())


copy()
