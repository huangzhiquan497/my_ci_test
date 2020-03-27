import os, sys, glob, json, re
from git import *


def save_build_commit(file_name, last_build_commit):
    create_parent_dir(file_name)

    fs = open(file_name, 'w+')
    fs.write(last_build_commit)
    fs.close()


def create_parent_dir(path):
    parentPath = os.path.split(os.path.abspath(path))[0]
    if not os.path.exists(parentPath):
        os.makedirs(parentPath)


def set_modified_bundle_names():
    project_path = sys.argv[1]
    repo = Repo(project_path)

    last_build_commit_file_name = project_path + '/Builds/last_build_commit.txt'

    if (not os.path.exists(last_build_commit_file_name)) or open(last_build_commit_file_name).read().__len__() == 0:
        save_build_commit(last_build_commit_file_name, str(repo.head.object.hexsha))
        with open(project_path + '/Builds/updated_bundles.json', 'w+') as f:
            f.write("")
        return

    lcommit = get_last_commit(last_build_commit_file_name, repo)

    assert (lcommit != None)

    author_name = str(lcommit.author)
    print("##teamcity[setParameter name='env.build.vcsroot.commit.author.last' value='" + author_name + "']")

    diff_index = lcommit.diff(repo.head.commit)

    changed_file_names = get_changed_file_names(diff_index)

    to_update_scene_bundles = []

    for file_name in changed_file_names:
        if is_scene_bundle(file_name):
            to_update_scene_bundles.append(file_name)

    commit_message = str(repo.head.commit.message)

    to_update_widget_bundles, to_update_scene_bundles_in_commit_message = get_changed_bundles(commit_message)

    for changed_scene_bundle in to_update_scene_bundles_in_commit_message:
        if changed_scene_bundle not in to_update_scene_bundles:
            to_update_scene_bundles.append(changed_scene_bundle)

    to_update_bundles = {
        "widget_bundles": to_update_widget_bundles,
        "scene_bundles": to_update_scene_bundles
    }

    with open(project_path + '/Builds/updated_bundles.json', 'w+') as f:
        f.write(json.dumps(to_update_bundles))

    os.remove(last_build_commit_file_name)
    save_build_commit(last_build_commit_file_name, str(repo.head.object.hexsha))


def get_changed_bundles(commit_message):
    to_update_widget_bundles = []
    to_update_scene_bundles = []
    pattern = re.compile(r'(@@@\[)(.*)(\])')
    commit_message_groups = re.findall(pattern, commit_message)
    for commit_message_group in commit_message_groups:
        for i in range(0, len(commit_message_group)):
            single_str = commit_message_group[i]
            if is_update_bundle_array(single_str):
                for changed_bundle in single_str.split(','):
                    if is_scene_bundle(changed_bundle):
                        if changed_bundle not in to_update_scene_bundles:
                            to_update_scene_bundles.append(changed_bundle)
                    else:
                        if changed_bundle not in to_update_widget_bundles:
                            to_update_widget_bundles.append(changed_bundle)

    return to_update_widget_bundles, to_update_scene_bundles


def is_scene_bundle(bundle_name):
    return (bundle_name.endswith('Machine.unity') or bundle_name.endswith('Lobby.unity'))


def is_update_bundle_array(s):
    return (not s.__contains__('@')) and (not s.__contains__('[')) and (not s.__contains__(']'))


def get_changed_file_names(diff_index):
    changed_file_names = []
    for diff_added in diff_index.iter_change_type('A'):
        changed_file_names.append(diff_added.b_path)
    for diff_modified in diff_index.iter_change_type('M'):
        changed_file_names.append(diff_modified.a_path)
    for diff_renamed in diff_index.iter_change_type('R'):
        changed_file_names.append(diff_renamed.b_path)
    return changed_file_names


def get_last_commit(last_build_commit_file_name, repo):
    lcommit = None
    fs = open(last_build_commit_file_name, 'r')
    last_build_commit = fs.read()
    fs.close()
    commits = list(repo.iter_commits('master'))
    for commit in commits:
        if commit.hexsha == last_build_commit:
            lcommit = commit
            break
    return lcommit


set_modified_bundle_names()
