# FlexibleConfiguration

> :warning: When possible, use [.NET Configuration Providers](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers) instead! This package was created before Configuration Providers were ready for primetime. This package is **no longer actively maintained**.

FlexibleConfiguration is a small library that makes it easy to gather application configuration from a variety of sources.

These sources are supported:
* Manually-specified key-value pairs
* Newline-delimited strings in the format `property.subProperty = value`
* Environment variables
* JSON (from a string or file)
* YAML (from a string or file)
* An existing object instance or anonymous type containing properties and values

## Supported Platforms

* .NET Framework 4.5 and higher
* .NET Core (`netstandard1.3`)

## Getting the Code

* Nuget: `install-package FlexibleConfiguration` or `dotnet add package FlexibleConfiguration`
* Source code: `git clone git@github.com:nbarbettini/FlexibleConfiguration.git`
