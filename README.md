# NuGet package - Selenium WebDriver ChromeDriver

[![NuGet Package](https://img.shields.io/nuget/v/Selenium.WebDriver.ChromeDriver.svg)](https://www.nuget.org/packages/Selenium.WebDriver.ChromeDriver/)

## What's this? / これは何?

This NuGet package install Chrome Driver(Win32) for Selenium WebDriver into your Unit Test Project.

この NuGet パッケージは、Selenium WebDriver用 Chrome Driver(Win32) を単体テストプロジェクトに追加します。

"chromedriver.exe" does not appear in Solution Explorer, but it is copied to bin folder from package folder when the build process.

"chromedriver.exe" はソリューションエクスプローラ上には現れませんが、ビルド時にパッケージフォルダから bin フォルダへコピーされます。

NuGet package restoring ready, and no need to commit "chromedriver.exe" binary into source code control repository.

NuGet パッケージの復元に対応済み、"chromedriver.exe" をソース管理リポジトリに登録する必要はありません。

## How to install? / インストール方法

For example, at the package manager console on Visual Studio, enter following command.  
一例として、Visual Studio 上のパッケージ管理コンソールにて、下記のコマンドを入力してください。

    PM> Install-Package Selenium.WebDriver.ChromeDriver

## Detail / 詳細

### Where is chromedriver.exe saved to? / どこに保存?

chromedriver.exe exists at  
" _{solution folder}_ /packages/Selenium.WebDriver.ChromeDriver. _{ver}_ /**driver**"  
folder.

     {Solution folder}/
      +-- packages/
      |   +-- Selenium.WebDriver.ChromeDriver.{version}/
      |       +-- driver/
      |       |   +-- chromedriver.exe
      |       +-- build/
      +-- {project folder}/
          +-- bin/
              +-- Debug/
              |   +-- chromedriver.exe (copy from above by build process)
              +-- Release/
                  +-- chromedriver.exe (copy from above by build process)

 And package installer configure msbuild task such as .csproj to
 copy chromedriver.exe into output folder during build process.
