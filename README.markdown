Delinearise
-----------

Delinearise is a command line tool for Windows that converts linearised files in an XPS (OPC) file to normal files. I created this to make it easier to debug problems I was encountering while working on XPS files.

Running Delinearise
-------------------

    Delinearise {Source file} {Target file}

for example:

    Delinearise input.xps output.xps

Requirements
------------

Delinearise requires the [.NET 4 Runtime](http://msdn.microsoft.com/en-us/netframework/aa569263) to run.

License
-------

Delinearise is published under the MIT license.

Building
--------

A Visual Studio solution and project is included, there is only once source file so it should be trivial to build with Mono although I haven't tried that. Source can be downloaded from [GitHub](https://github.com/tonyedgecombe/Delinearise).


