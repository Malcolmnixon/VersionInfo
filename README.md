# VersionInfo
The VersionInfo utility generates a report of file information.


# Command Line Arguments
To run the utility; execute:
```
    VersionInfo [options] <patterns>

    -h -? --help           Print help about this program
    -v --version           Print the version of this program
    -s --size[=on/off]     Include/exclude file-size
    -t --time[=on/off]     Include/exclude file-time
    -r --recurse[=on/off]  Enable/disable recurse into folders
    -c --crc32[=on/off]    Enable/disable file CRC32
    -m --md5[=on/off]      Enable/disable file MD5
    -s1 --sha-1[=on/off]   Enable/disable file SHA-1
    -s2 --sha-2[=on/off]   Enable/disable file SHA-2
    --summary=on           Add summary to report
    --summary=only         Only show summary
    --emit=text            Emit Text report
    --emit=verbose         Emit Verbose report
    --emit=csv             Emit CSV report
    --                     End of options

    @<options-file>        Provide command line arguments from file
```


# Example Text Report
The following is an example Text report:
```
    Name             Size  Time                Title       Version Product
    test.dll         2048  2019/03/15 15:31:35 Example DLL 1.2.3.4 Example DLL
    test.jar         372   2019/03/15 15:29:28 Test Title  1.2.3   -
    test.msi         20480 2019/03/15 17:11:43 MSI Test    2.3.4   -
    test1.txt        0     2019/03/15 15:29:05 -           -       -
    test2.txt        43    2019/03/15 15:29:05 -           -       -
    test3.txt        112   2019/03/15 15:29:05 -           -       -
```


# Example Verbose Report
The following is an example Verbose report:
```
    test.dll
      Size: 2048
      Time: 2019/03/15 15:31:35
      MD5: 25d73c514a3230aaec3f942b527a58d2
      Title: Example DLL
      Version: 1.2.3.4
      Product: Example DLL
      Author: DEMA Consulting
      Copyright: Copyright (c) DEMA Consulting 2018
    test.jar
      Size: 372
      Time: 2019/03/15 15:29:28
      MD5: e9c20902140ca6820afcd389c8e666ab
      Title: Test Title
      Version: 1.2.3
      Author: DEMA Consulting
    test.msi
      Size: 20480
      Time: 2019/03/15 17:11:43
      MD5: 32d94f660f1eabae9eede62f05ade914
      Title: MSI Test
      Version: 2.3.4
      Author: DEMA Consulting
    test1.txt
      Size: 0
      Time: 2019/03/15 15:29:05
      MD5: d41d8cd98f00b204e9800998ecf8427e
    test2.txt
      Size: 43
      Time: 2019/03/15 15:29:05
      MD5: 9e107d9d372bb6826bd81d3542a419d6
    test3.txt
      Size: 112
      Time: 2019/03/15 15:29:05
      MD5: 03dd8807a93175fb062dfb55dc7d359c
```


# Example CSV Report
The following is an example CSV report:
```
    Name,Size,Time,CRC32,MD5,SHA1,SHA2,Title,Version,Product,Author,Copyright
    test.dll,2048,2019/03/15 15:31:35,,25d73c514a3230aaec3f942b527a58d2,,,Example DLL,1.2.3.4,Example DLL,DEMA Consulting,Copyright (c) DEMA Consulting 2018
    test.jar,372,2019/03/15 15:29:28,,e9c20902140ca6820afcd389c8e666ab,,,Test Title,1.2.3,,DEMA Consulting,
    test.msi,20480,2019/03/15 17:11:43,,32d94f660f1eabae9eede62f05ade914,,,MSI Test,2.3.4,,DEMA Consulting,
    test1.txt,0,2019/03/15 15:29:05,,d41d8cd98f00b204e9800998ecf8427e,,,,,,,
    test2.txt,43,2019/03/15 15:29:05,,9e107d9d372bb6826bd81d3542a419d6,,,,,,,
    test3.txt,112,2019/03/15 15:29:05,,03dd8807a93175fb062dfb55dc7d359c,,,,,,,
```
