import os
from pathlib import Path

def per_config_format(project, compiler, config):
  return f"{project}_{config}_{compiler}"

def per_config_folder_format(compiler, config):
  return os.path.join(compiler, config)

def print_error(msg):
  print(f"[Error] {msg}")

def print_warn(msg):
  print(f"[Warn] {msg}")

def print_info(msg):
  print(f"[Info] {msg}")

def find_in_parent(path, toFind):
  curr_path = path

  while toFind not in os.listdir(curr_path):
    if Path(curr_path).parent == curr_path:
      print_error(f"{toFind} not found in parents of {path}")
      return path

    curr_path = Path(curr_path).parent

  return curr_path