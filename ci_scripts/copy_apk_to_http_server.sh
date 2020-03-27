#!/bin/sh
echo "start"

export projectPath=$(dirname $(dirname $0))
export projectParentPath=$(dirname $(dirname $(dirname $0)))
echo $projectPath

export python_exe=${projectPath}/PythonToolsForUnity/venv/bin/python3.7
export upload_script_path=${projectPath}/PythonToolsForUnity/venv/src/test_python.py

$python_exe $upload_script_path