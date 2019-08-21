# NuGet package - Selenium WebDriver ChromeDriver

[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v74-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/74.0.3729.6)
[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v75-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/75.0.3770.140)
[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v76-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/76.0.3809.6801) 


## What's this?

This NuGet package install Chrome Driver (Win32, macOS, and Linux64) for Selenium WebDriver into your Unit Test Project.

"chromedriver(.exe)" does not appear in Solution Explorer, but it is copied to bin folder from package folder when the build process.

NuGet package restoring ready, and no need to commit "chromedriver(.exe)" binary into source code control repository.

## How to install?

For example, at the package manager console on Visual Studio, enter following command.

If you are using Chrome version 76:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 76.0.3809.6801

If you are using Chrome version 75:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 75.0.3770.140

If you are using Chrome version 74:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 74.0.3729.6

To learn what version of ChromeDriver you need to use, please see also following page:

[https://chromedriver.chromium.org/downloads/version-selection](https://chromedriver.chromium.org/downloads/version-selection)

## Cross platform building and publishing

### By default - it depends on the OS running the build process

By default, the platform type of the web driver file copied to the output folder depends on the OS running the build process.

- When you build the project which references the nuget package of chromedriver **on Windows OS**, **win32 version** of chromedriver wil be copied to the output folder.
- When you build it **on macOS**, **macOS x64 version** of chromedriver wil be copied to the output folder.
- When you build it on **any Linux distributions**, **linux x64 version** of chromedriver wil be copied to the output folder.

### Method 1 - Specify "Runtime Identifier"

When you specify the "Runtime Identifier (**RID**)" explicitly, the platform type of the driver file is same to the RID which you specified. (it doesn't depends on the which OS to use for build process.)

You can specify RID as a MSBuild property in aproject file,

```xml
<PropertyGroup>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
</PropertyGroup>
```

or, as a command line `-r` option for dotnet build command.

```shell
> dotnet build -r:osx.10.12-x64
```

- When the RID that **starts with "win"** is specified, **win32 version** of chromedriver will be copied to the output folder.
- When the RID that **starts with "osx"** is specified, , **macOS x64 version** of chromedriver wil be copied to the output folder.
- When the RID that **starts with "linux"** is specified, **linux x64 version** of chromedriver wil be copied to the output folder.

If you specify another pattern of RID like "ubuntu.18.04-x64", the platform type of the web driver file which will be copied to the output folder depends on the OS running the build process. (default behavior.)

### Method 2 - Specify "WebDriverPlatform" msbuild property

You can control which platform version of chromedriver will be copied by specifying "WebDriver Platform" msbuild property.

"WebDriverPlatform" MSBuild property can take one of the following values:

- "win32"
- "mac64"
- "linux64"

You can specify "WebDriverPlatform" MSBuild property in aproject file,

```xml
<PropertyGroup>
  <WebDriverPlatform>win32</WebDriverPlatform>
</PropertyGroup>
```

or, command line `-p` option for dotnet build command.

```shell
> dotnet build -p:WebDriverPlatform=mac64
```

The specifying "WebDriverPlatform" MSBuild property is most high priority method to control which platform version of chromedriver will be copied.

If you run the following command on Windows OS,

```shell
> dotnet build -r:ubuntu.18.04-x64 -p:WebDriverPlatform=mac64
```

The driver file of macOS x64 version will be copied to the output folder.

## How to include the driver file into published files?

"chromedriver(.exe)" isn't included in published files on default configuration. This behavior is by design.

If you want to include "chromedriver(.exe)" into published files, please define `_PUBLISH_CHROMEDRIVER` compilation symbol.

![define _PUBLISH_CHROMEDRIVER compilation symbol](https://raw.githubusercontent.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/master/.asset/define_PUBLISH_CHROMEDRIVER_compilation_symbol.png)

Anoter way, you can define `PublishChromeDriver` property with value is "true" in MSBuild file (.csproj, .vbproj, etc...) to publish the driver file instead of define compilation symbol.

```xml
  <Project ...>
    ...
    <PropertyGroup>
      ...
      <PublishChromeDriver>true</PublishChromeDriver>
      ...
    </PropertyGroup>
...
</Project>
```

You can also define `PublishChromeDriver` property from command line `-p` option for `dotnet publish` command.

```shell
> dotnet publish -p:PublishChromeDriver=true
```

#### Note

`PublishChromeDriver` MSBuild property always override the condition of define `_PUBLISH_CHROMEDRIVER` compilation symbol or not. If you define `PublishChromeDriver` MSBuild property with false, then the driver file isn't included in publish files whenever define `_PUBLISH_CHROMEDRIVER` compilation symbol or not.

## Appendix

### The numbering of the package version

The rule of the version number of this package is:

`chromedriver version MAJOR.MINOR.BUILD.PATCH` + `package version (2 digit)`

For example, 2nd package release for the chromedriver ver.1.2.3.4, the package version is `1.2.3.4` + `02` → `1.2.3.402`.

Sometime multiple packages for same chromedriver version may be released by following example reasons.

- Packaging miss. (the package included invalid version of the driver files)
- Fixing bug of the build script, or improving the build script.

### Where is chromedriver.exe saved to?

chromedriver(.exe) exists at  
" _{solution folder}_ /packages/Selenium.WebDriver.ChromeDriver. _{ver}_ /**driver**/ _{platform}_"  
folder.

     {Solution folder}/
      +-- packages/
      |   +-- Selenium.WebDriver.ChromeDriver.{version}/
      |       +-- driver/
      |       |   +-- win32
      |       |       +-- chromedriver.exe
      |       |   +-- mac64
      |       |       +-- chromedriver
      |       |   +-- linux64
      |       |       +-- chromedriver
      |       +-- build/
      +-- {project folder}/
          +-- bin/
              +-- Debug/
              |   +-- chromedriver(.exe) (copy from above by build process)
              +-- Release/
                  +-- chromedriver(.exe) (copy from above by build process)

 And package installer configure msbuild task such as .csproj to
 copy chromedriver(.exe) into output folder during build process.

## License

The build script (.targets file) in this NuGet package is licensed under [The Unlicense](https://github.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/blob/master/LICENSE).

The binary files of ChromeDriver are licensed under the [BSD-3-Clause](https://cs.chromium.org/chromium/src/LICENSE).