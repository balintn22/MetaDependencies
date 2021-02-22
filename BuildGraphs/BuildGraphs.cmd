@echo off

set RDA=ReferenceConflictAnalyzer.CommandLine.exe
set OUTDIR=dependency-graphs

if not exist %outdir% md %OUTDIR%
del %OUTDIR%\*.dgml

REM Timeouts make sure that output files do not conflict.
ECHO Building dependency graphs in %OUTDIR%

call :do1 "..\AssInfo\bin\Debug\AssInfo.exe"
call :do1 "..\MergeGraphs\bin\Debug\MergeGraphs.exe"

exit /b

REM Subroutines
:do1
ECHO Processing %1
%RDA% -output="%OUTDIR%" -file=%1
timeout /t 1
exit /b