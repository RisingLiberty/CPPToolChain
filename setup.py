import os
from pathlib import Path
import shutil

RequiredTools = ["cl", "link", "clang", "g++", "ninja", "sharpmake.application"]

def ask_for_input(question):
  print(question)
  return input()

def affirmative_response(response : str):
  return response.lower() == "y" or response.lower() == "yes"

def has_tools_downloaded():
  envPath = os.environ["PATH"]
  paths = envPath.split(os.pathsep)

  stemsToFind = RequiredTools

  for path in paths:
    if not os.path.exists(path):
      continue

    subFilesOrFolders = os.listdir(path)
    for fileOrFolder in subFilesOrFolders:
      absPath = os.path.join(path, fileOrFolder)
      if os.path.isfile(absPath):
        stem = Path(absPath).stem
        if (stem in stemsToFind):
          stemsToFind.remove(stem)

          if len(stemsToFind) == 0:
            return True
  
  print("Not all tools were found")
  print(f"Tools that weren't found: {stemsToFind}")
  return False

def main():
  if not has_tools_downloaded():
    print("Not all tools were found, downloading tools..")
    os.system("py _build/scripts/download_tools.py")

    response = ask_for_input("Tools are now downloaded.\nWould you like to copy these to a user directory so they're not in this repository? (Y/N)")
    if affirmative_response(response):
      response = ask_for_input("Please enter the path where you'd like to store them")
      print("Copying files..")
      shutil.copytree("_build/tools", os.path.join(response, "tools"))
      print("Done")
      
    print("Please add the directory provided to PATH environment variable or you might be unable to build using this template")
    ask_for_input("When finished, press Enter")

if __name__ == "__main__":
  main()