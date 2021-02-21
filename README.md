# MetaDependencies
Tools to analyse dotnet assembly dependencies in a micro-services environment.

## AssInfo
AssInfo crawls all subdirectories and find .csproj files. It'll then output information about those assemblies, including build output type (exe/dll), nuget packaging properties, assembly description and target frameworks, etc.
it supports old style (VS2017 and before) and SDK style .csproj files.

Usage:
   Add AssInfo to yuor path, or copy AssInfo and AssInfo.Logic to the folder where you start your search.
   To display assembly info on screen:
      AssInfo
   To write assembly info to a text file:
      AssInfo > out.txt
