# ============================================ 
#
# Author: Nick De Breuck
# Twitter: @nick_debreuck
# 
# File: run_clang_tools.py
# Copyright (c) Nick De Breuck 2022
#
# ============================================

import os
import argparse
import subprocess
import diagnostics
import util

def run(projectName, compiler, config):
  script_path = os.path.dirname(__file__)
  root_path = util.find_in_parent(script_path, "source")

  util.print_info("Running clang-tidy")
  os.system(f"py {script_path}/run_clang_tidy.py -config-file={root_path}/source/.clang-tidy -p={root_path}/.rex/build/ninja/{projectName}/clang_tools/clang/{config} -header-filter=.* -quiet") # force clang compiler, as clang-tools expect it
  
  util.print_info("Running clang-format")
  os.system(f"py {script_path}/run_clang_format.py -r -i {root_path}/source/{projectName}")

if __name__ == "__main__":
  # arguments setups
  parser = argparse.ArgumentParser(formatter_class=argparse.RawTextHelpFormatter)

  parser.add_argument("-l", "--level", default="info", help="logging level")

  args, unknown = parser.parse_known_args()

 # initialize the logger
  log_level_str = args.level
  log_level = diagnostics.logging_level_from_string(log_level_str)
  logger = diagnostics.StreamLogger("setup", log_level)

 # useful for debugging
  logger.info(f"Executing {__file__}")

 # execute the script
  run()

 # print. We're done.
  logger.info("Done.")

  exit(0)