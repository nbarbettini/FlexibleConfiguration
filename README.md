# FlexibleConfiguration
FlexibleConfiguration is a small library that makes it easy to gather application configuration from a variety of sources.

Currently, these sources are supported:
* Manually-specified key-value pairs
* Newline-delimited strings in the format `property.subProperty = value`
* Environment variables
* JSON (from a string or file)
* YAML (from a string or file)
* An existing object instance or anonymous type containing properties and values

## Background

This project borrows heavily from code at [aspnet/Configuration](https://github.com/aspnet/Configuration), which is a great library currently under development. I wanted to use the Configuration packages in my project, but it doesn't have a stable release yet, and is only compatible with .NET 4.5.1 or higher (or CoreCLR).

## Supported Platforms

* .NET Framework 4.5 and higher
* .NET Core (`netstandard1.3`)

## Getting the Code

* Via nuget: `install-package FlexibleConfiguration`
* Cloning this repository: `git clone git@github.com:nbarbettini/FlexibleConfiguration.git`
