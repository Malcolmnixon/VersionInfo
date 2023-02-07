@ECHO OFF

REM Build the test JAR from the manifest
jar cvfm test.jar MANIFEST.MF

REM Scan the test JAR and capture the output
..\..\VersionInfo\bin\Release\VersionInfo --time=off --size=off --emit-verbose test.jar > actual.txt