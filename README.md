# NuGet package - Selenium WebDriver ChromeDriver

[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v74-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/74.0.3729.6)
[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v75-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/75.0.3770.90)
[![NuGet Package](https://img.shields.io/badge/nuget-for%20Chrome%20v76-blue.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/76.0.3809.12-beta) 


## What's this? / これは何?

This NuGet package install Chrome Driver (Win32, macOS, and Linux64) for Selenium WebDriver into your Unit Test Project.

この NuGet パッケージは、Selenium WebDriver用 Chrome Driver (Win32, macOS, 及び Linux64) を単体テストプロジェクトに追加します。

"chromedriver(.exe)" does not appear in Solution Explorer, but it is copied to bin folder from package folder when the build process.

"chromedriver(.exe)" はソリューションエクスプローラ上には現れませんが、ビルド時にパッケージフォルダから bin フォルダへコピーされます。

NuGet package restoring ready, and no need to commit "chromedriver(.exe)" binary into source code control repository.

NuGet パッケージの復元に対応済み、"chromedriver(.exe)" をソース管理リポジトリに登録する必要はありません。

## How to install? / インストール方法

For example, at the package manager console on Visual Studio, enter following command.  
一例として、Visual Studio 上のパッケージ管理コンソールにて、下記のコマンドを入力してください。

If you are using Chrome version 76:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 76.0.3809.12-beta -IncludePrerelease

If you are using Chrome version 75:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 75.0.3770.90

If you are using Chrome version 74:

    PM> Install-Package Selenium.WebDriver.ChromeDriver -Version 74.0.3729.6

To learn what version of ChromeDriver you need to use, please see also following page:

[http://chromedriver.chromium.org/downloads/version-selection](http://chromedriver.chromium.org/downloads/version-selection)

## Detail / 詳細

### Where is chromedriver.exe saved to? / どこに保存?

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

### How to include the driver file into published files? / ドライバーを発行ファイルに含めるには?

"chromedriver(.exe)" isn't included in published files on default configuration. This behavior is by design.

"chromedriver(.exe)" は、既定の構成では、発行ファイルに含まれません。この挙動は仕様です。

If you want to include "chromedriver(.exe)" into published files, please define `_PUBLISH_CHROMEDRIVER` compilation symbol.

"chromedriver(.exe)" を発行ファイルに含めるには、コンパイル定数 `_PUBLISH_CHROMEDRIVER` を定義してください。

![define _PUBLISH_CHROMEDRIVER compilation symbol](https://raw.githubusercontent.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/master/.asset/define_PUBLISH_CHROMEDRIVER_compilation_symbol.png)

Anoter way, you can define `PublishChromeDriver` property with value is "true" in MSBuild file (.csproj, .vbproj, etc...) to publish the driver file instead of define compilation symbol.

別の方法として、コンパイル定数を定義する代わりに、MSBuild ファイル (.csproj, .vbproj, etc...) 中で `PublishChromeDriver` プロパティを値 true で定義することでドライバーを発行ファイルに含めることができます。 

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

#### Note / 補足 

`PublishChromeDriver` MSBuild property always override the condition of define `_PUBLISH_CHROMEDRIVER` compilation symbol or not. If you define `PublishChromeDriver` MSBuild property with false, then the driver file isn't included in publish files whenever define `_PUBLISH_CHROMEDRIVER` compilation symbol or not.

`PublishChromeDriver` MSBuild プロパティは常に `_PUBLISH_CHROMEDRIVER` コンパイル定数を定義しているか否かの条件を上書きします。もし `PublishChromeDriver` MSBuild プロパティを false で定義したならば、`_PUBLISH_CHROMEDRIVER` コンパイル定数を定義しているか否かによらず、ドライバは発行ファイルに含められません。

## License

The build script (.targets file) in this NuGet package is licensed under [The Unlicense](https://github.com/jsakamoto/nupkg-selenium-webdriver-chromedriver/blob/master/LICENSE).

The binary files of ChromeDriver are licensed under the [BSD-3-Clause](https://cs.chromium.org/chromium/src/LICENSE).