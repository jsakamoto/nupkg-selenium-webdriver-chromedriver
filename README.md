# NuGet package - Selenium WebDriver ChromeDriver

[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v126-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/126.0.6478.18200) [![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v127-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/127.0.6533.11900) [![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v128-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/128.0.6613.11900)

## What's this?

This NuGet package installs Chrome Driver (Win32, macOS, macOS arm64, and Linux64) for Selenium WebDriver into your Unit Test Project.

"chromedriver(.exe)" does not appear in Solution Explorer, but it is copied to the output folder from the package source folder when the build process.

NuGet package restoring ready, and no need to commit "chromedriver(.exe)" binary into source code control repository.

> **Warning**  
> Since Selenium WebDriver version 4.6 was released in November 2022 or later, it has contained ["Selenium Manager"](https://www.selenium.dev/blog/2022/introducing-selenium-manager/), which will automatically download the most suitable version and platform WebDriver executable file. So now, **you can run applications that use Selenium and manipulates web browsers without this package.** However, due to compatibility and some offline scenarios, we intend to keep this package for the time being.

## How to install?

For example, at the package manager console on Visual Studio, enter the following command.

If you are using Chrome version 128:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 128.0.6613.11900

If you are using Chrome version 127:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 127.0.6533.11900

If you are using Chrome version 126:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 126.0.6478.18200

To learn what version of ChromeDriver you need to use, please see also the following page:

[https://chromedriver.chromium.org/downloads/version-selection](https://chromedriver.chromium.org/downloads/version-selection)

## Cross-platform building and publishing

### By default - it depends on the OS running the build process

By default, the platform type of the web driver file copied to the output folder depends on the OS running the build process.

- When you build the project which references the NuGet package of chromedriver **on Windows OS**, **win32 version** of chromedriver will be copied to the output folder.
- When you build it **on macOS on Intel CPU hardware**, **macOS x64 version** of chromedriver will be copied to the output folder.
- When you build it **on macOS on Apple CPU hardware**, **macOS Arm64 version** of chromedriver will be copied to the output folder.
- When you build it on **any Linux distributions**, **Linux x64 version** of chromedriver will be copied to the output folder.

### Method 1 - Specify "Runtime Identifier"

When you specify the "Runtime Identifier (**RID**)" explicitly, the platform type of the driver file is the same to the RID which you specified. (it doesn't depends on the which OS to use for build process.)

You can specify RID as a MSBuild property in a project file,

```xml
<PropertyGroup>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>
</PropertyGroup>
```

or, as a command-line `-r` option for dotnet build command.

```shell
> dotnet build -r:osx-x64
```

- When the RID that **starts with "win"** is specified, **win32 version** of chromedriver will be copied to the output folder.
- When the RID that **starts with "osx"** and **ends with "x64"** is specified, **macOS x64 version** of chromedriver will be copied to the output folder.
- When the RID that **starts with "osx"** and **ends with "arm64"** is specified, **macOS Arm64 version** of chromedriver will be copied to the output folder.
- When the RID that **starts with "linux"** is specified, **Linux x64 version** of chromedriver will be copied to the output folder.

If you specify another pattern of RID like "linux-x64", the platform type of the web driver file which will be copied to the output folder depends on the OS running the build process. (default behavior.)

### Method 2 - Specify "WebDriverPlatform" msbuild property

You can control which platform version of chromedriver will be copied by specifying "WebDriverPlatform" MSBuild property.

"WebDriverPlatform" MSBuild property can take one of the following values:

- "win32"
- "mac64"
- "mac64arm"
- "linux64"

You can specify "WebDriverPlatform" MSBuild property in a project file,

```xml
<PropertyGroup>
  <WebDriverPlatform>win32</WebDriverPlatform>
</PropertyGroup>
```

or, command-line `-p` option for dotnet build command.

```shell
> dotnet build -p:WebDriverPlatform=mac64
```

The specifying "WebDriverPlatform" MSBuild property is the highest priority method to control which platform version of the chromedriver will be copied.

If you run the following command on Windows OS,

```shell
> dotnet build -r:linux-x64 -p:WebDriverPlatform=mac64
```

The driver file of macOS x64 version will be copied to the output folder.

## How to include the driver file into published files?

"chromedriver(.exe)" isn't included in published files on default configuration. This behavior is by design.

If you want to include "chromedriver(.exe)" into published files, please define `_PUBLISH_CHROMEDRIVER` compilation symbol.

![define _PUBLISH_CHROMEDRIVER compilation symbol](https://raw.githubusercontent.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/master/.asset/define_PUBLISH_CHROMEDRIVER_compilation_symbol.png)

Another way, you can define `PublishChromeDriver` property with value is "true" in MSBuild file (.csproj, .vbproj, etc...) to publish the driver file instead of define compilation symbol.

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

You can also define `PublishChromeDriver` property from the command line `-p` option for `dotnet publish` command.

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

Sometime multiple packages for the same chromedriver version may be released by following example reasons.

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
      |       |   +-- mac64arm
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

And package installer configure MSBuild task such as .csproj to
copy chromedriver(.exe) into the output folder during the build process.

## License

The build script (.targets file) in this NuGet package is licensed under [The Unlicense](https://github.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/blob/master/LICENSE).

The binary files of ChromeDriver are licensed under the [BSD-3-Clause](https://cs.chromium.org/chromium/src/LICENSE).
