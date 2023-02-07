@ECHO OFF

REM Scan the test files and capture the output
..\..\VersionInfo\bin\Release\VersionInfo --time=off --crc32 --md5 --sha-1 --sha-2 --emit-verbose test*.txt > actual.txt