# CPPToolChain
A Template to generate C++ projects that supports all compilers, static analyzers and unit tests

# How does it work?

This repository is meant to be downloaded and copied over to where you want your new C++ project to begin.
its main target is Windows but it supports every platform with the help of Ninja.

Run setup.py in the root directory, this only needs to run the first time you download or clone the repository,
after you've run it once and want to create another C++ project with this template, you don't have to run setup.py again.

Setup.py will install python modules and download the tools required to build the C++ project.

Add the binary paths of the tools to your PATH environment variable and you're good to go.

# Generate the project
To generate the project, run generate.py or if you're familiar with Sharpmake, run sharpmake in _build/sharpmake.

By default this generates a Visual Studio 2019 solution and a ninja solution.
Ninja doesn't support solutions and project out of the box, but these have been added to a custom sharpmake version which gets downloaded when running setup.py

# Using the template
You can manually configure the sharpmake scripts to your liking and extend them where needed.

Sharpmake is a build tool like CMake, but it's written in C#, making it easier to debug and also adds the capability of the entire .NET framework (among other things).

It takes a little practice to figure out initially but it's easy to pick up and the source code is freely available on github for you to inspect.

adding files works just the same as you would do normally, the "source" folder provides some initial source code for you to start with.



## Have fun!