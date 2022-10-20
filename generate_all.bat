thirdparty\sharpmake\sharpmake.application.exe /sources("sharpmake/source/main.sharpmake.cs")
thirdparty\cmake_converter\main.py -s main_solution.sln
cmake -B .rex/vs
cmake -B .rex/ninja/debug -G Ninja -DCMAKE_EXPORT_COMPILE_COMMANDS=ON -DCMAKE_BUILD_TYPE=Debug
cmake -B .rex/ninja/release -G Ninja -DCMAKE_EXPORT_COMPILE_COMMANDS=ON -DCMAKE_BUILD_TYPE=Release