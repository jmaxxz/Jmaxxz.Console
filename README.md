Jmaxxz.Console.Options ![Jmaxxz.Console.Options](https://img.shields.io/nuget/v/Jmaxxz.Console.Options.svg) ![Jmaxxz.Console.Options](https://img.shields.io/nuget/dt/Jmaxxz.Console.Options.svg)
-------------------------

[![Join the chat at https://gitter.im/jmaxxz/Jmaxxz.Console](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/jmaxxz/Jmaxxz.Console?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
```shell
PM> Install-Package Jmaxxz.Console.Options 
```

Jmaxxz.Console.Shell ![Jmaxxz.Console.Shell](https://img.shields.io/nuget/v/Jmaxxz.Console.Shell.svg) ![Jmaxxz.Console.Shell](https://img.shields.io/nuget/dt/Jmaxxz.Console.Shell.svg)
-------------------------
```shell
PM> Install-Package Jmaxxz.Console.Shell 
```

Examples
-----
Let's assume that we have an app that validates json against a json schema. Our app expects 2 files: the first file contains the json and the second file contains the json schema.

Here's an example usage:

    $ validate.exe JsonFile.json SchemaFile.schema

You can parse this with the following code:

```csharp
static void Main(string[] args)
{
  string jsonFile = null;
  string schemaFile = null;

  var options = new Options
  {
    new Option(new string[] {}, s => jsonFile = s, "jsonFile",
      "The file that contains the json you wish to validate"),
    new Option(new string[] {}, s => schemaFile = s, "schemaFile",
      "The file that contains the schema to use for validation")
  };

  if (!options.Parse(args) || jsonFile == null || schemaFile == null)
  {
    options.PrintUsage();
    return;
  }

  // Use jsonFile and schemaFile in your app...
}
```

If you wish to see the usage type

```shell
> validate /?
Usage: validate [-?] <jsonFile> <schemaFile>
================================================================================
Flags                              |  Descriptions
________________________________________________________________________________
-?                                 |  Prints usage for validate.vshost.exe
--help                             |
--------------------------------------------------------------------------------
<jsonFile>                         |  The file that contains the json you wish t
                                   |  o validate
--------------------------------------------------------------------------------
<schemaFile>                       |  The file that contains the schema to use f
                                   |  or validation
--------------------------------------------------------------------------------
```

License
-------
Jmaxxz.Console is distributed under the following license:

DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
Version 2, December 2004
 
Copyright (C) 2004 Sam Hocevar <sam@hocevar.net>

Everyone is permitted to copy and distribute verbatim or modified
copies of this license document, and changing it is allowed as long
as the name is changed.
 
  DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE
  TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION
 
 0. You just DO WHAT THE FUCK YOU WANT TO.
