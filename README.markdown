Deinterleave
------------

Deinterleave is a command line tool for Windows that converts interleaved files in an XPS (OPC) file to normal files. I created this to make it easier to debug problems I was encountering while working on XPS files.

Fonts are deobfuscated on the way through and their MIME type changed although the file extension remains the same.

Running Deinterleave
--------------------

    Deinterleave {Source file} {Target file}

for example:

    Deinterleave input.xps output.xps

Requirements
------------

Deinterleave requires the [.NET 4 Runtime](http://msdn.microsoft.com/en-us/netframework/aa569263) to run.

License
-------

Deinterleave is published under the MIT license.

Building
--------

A Visual Studio solution and project is included. Source can be downloaded from [GitHub](https://github.com/tonyedgecombe/Deinterleave).


