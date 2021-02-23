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
Can keep or remove indirect assembly references, the latter to simplify the resultant graph.
Use Visual Studio DGML Editor (can be installed from Visual Studio installer / Individual components) to view amnipulate the output.

# Workflow / How to use
I assume that your sources are cloned next to each other, in a single folder.
To help working with a large number of code repos, I suggest using the [Meta tool](https://github.com/mateodelnorte/meta).
Copy the above tools with their supporting .dll files to that root directory.

If you want to find the projects
 - where nuget packages are built
 - that use different build target platforms
run the AssInfo tool, and open the resulting text file in Excel.
Check in the resultant file to git, so that you can monitor changes over time.

If you want to understand assembly dependencies in your services, then
 - edit the BuildGraphs.cmd file so that it runs assembly analysis on your service host assemblies
 - run the BuildGraphs.cmd tool. When run without arguments, it creates a directory named "dependency-graphs"
 - run the MergeGraphs tool like this
   MergeGraphs -indir="dependency-graphs"
 - open the resultant Merged.dgml with Visual Studio 2017+. Make sure that the DGML editor individual component is installed.
 - re-layout, re-arrange, massage the output any way you like to understand assembly relationships.
