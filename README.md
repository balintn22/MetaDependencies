# MetaDependencies
Tools to analyse dotnet assembly dependencies in a micro-services environment.

## AssInfo
AssInfo crawls all subdirectories and find .csproj files. It'll then output information about those assemblies, including build output type (exe/dll), nuget packaging properties, assembly description and target frameworks, etc.
it supports old style (VS2017 and before) and SDK style .csproj files.

Usage:
   Add AssInfo to your path, or copy AssInfo and AssInfo.Logic to the folder where you start your search.
   To display assembly info on screen:
      AssInfo
   To write assembly info to a text file:
      AssInfo > out.txt

## BuildGraphs.cmd
It is an example batch file to demonstrate how to generate .dgml dependency graps using the ReferenceConflictAnalyser too (see https://github.com/marss19/reference-conflicts-analyzer).
It contains ReferenceConflictAnalyser.exe to generate dependency graphs for a number of assemblies, and places them in a shared output directory.
It uses ReferenceConflictAnalyser.CommnadLine.exe and ReferenceConflictAnalyser.dll from the above link.
They should be placed next to BuildGRaphs.cmd or in the system path.

## MergeGraphs
This tool loads all .dgml dependency graphs from a directory, merges them, and creates a merged output graph as a .dgml.