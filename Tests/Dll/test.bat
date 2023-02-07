@ECHO OFF

REM Compile the resource file
rc test.rc

REM Build the test DLL
link /out:test.dll /dll /machine:x86 /noentry test.res

REM Scan the test DLL and capture the output
..\..\VersionInfo\bin\Release\VersionInfo --time=off --size=off --emit-verbose test.dll > actual.txt