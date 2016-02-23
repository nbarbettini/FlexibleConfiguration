# FlexibleConfiguration
FlexibleConfiguration is a small library that makes it easy to gather application configuration from a variety of sources.

Currently, these sources are supported:
* Manually-specified key-value pairs
* Newline-delimited strings in the format `property = value`
* Environment variables
* JSON (from a string or file)
* YAML (from a string or file)

## Supported Platforms

* .NET Framework 3.5 and higher
* CoreCLR (`dotnet5.4`/`netstandard1.3`)
* DNX (`dnxcore50`)
* Windows Store & UWP (`netcore50`)

## Getting the Code

* ~~Via nuget: `install-package FlexibleConfiguration`~~ (not yet available)
* Cloning this repository: `git clone git@github.com:nbarbettini/FlexibleConfiguration.git`

## How to Use

First, define a POCO that represents your configuration tree:

```csharp
class MyAppConfiguration
{
  public ConnectionConfig Connection { get; private set; }

  public string AppTitle { get; private set; }
}

class ConnectionConfig
{
  public int Timeout { get; private set; }

  public string ApiUrl { get; private set; }
}
```

Next, build the configuration up by adding sources:

```csharp
var configBuilder = new FlexibleConfiguration<MyAppConfiguration>();
configBuilder.Add("connection.apiUrl = http://default-value-here/");
configBuilder.AddJsonFile("settings.json");
```

Then, call `Build()` to compile all the sources and populate your POCO:

```csharp
var config = configBuilder.Build();
```

## Post-Processing Actions

Coming soon!

## Custom Validation

Coming soon!
