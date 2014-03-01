# NuGet package - Selenium WebDriver ChromeDriver

## What's this? / これは何?

This NuGet package download and install Chrome Driver(Win32) for Selenium WebDriver into your Unit Test Project on the fly.  
(This package dose not contain chromedriver.exe, but add the avility of automatically downloading chromedriver.exe form official site to your project.)

この NuGet パッケージは、Selenium WebDriver用 Chrome Driver(Win32) をその場でダウンロードし単体テストプロジェクトに追加します。  
(この NuGet パッケージには chromedriver.exe は含まれません、公式サイトから chromedriver.exe を自動でダウンロードする能力をプロジェクトに追加します)

"chromedriver.exe" added as a linked project item, and copied to bin folder at the build.

"chromedriver.exe" はリンクされたアイテムとしてプロジェクトに追加され、ビルド時に bin フォルダにコピーされます。

NuGet package restoring ready, and no need to commit "chromedriver.exe" binary into source code control repository.

NuGet パッケージの復元に対応済み、"chromedriver.exe" をソース管理リポジトリに登録する必要はありません。

## How to install? / インストール方法

For example, at the package manager console on Visual Studio, enter following command.  
一例として、Visual Studio 上のパッケージ管理コンソールにて、下記のコマンドを入力してください。

    PM> Install-Package Selenium.WebDriver.ChromeDriver
    
## Detail / 詳細

### Where is chromedriver.exe saved to? / どこに保存?

chromedriver.exe is downloaded from official web site, and saved to  
" _{solution folder}_ /packages/Selenium.WebDriver.ChromeDriver. _{ver}_ /content"  
folder at installing this package or building a project.

     {Solution folder}/
      +-- packages/
      |   +-- Selenium.WebDriver.ChromeDriver.{version}/
      |       +-- content/
      |       |   +-- chromedriver.exe (download by package installer or build process)
      |       +-- tools/
      +-- {project folder}/
          +-- bin/
              +-- Debug/
              |   +-- chromedriver.exe (copy from above by build process)
              +-- Release/
                  +-- chromedriver.exe (copy from above by build process)
 
 And package installer configure msbuild task such as .csproj to
 copy chromedriver.exe into output folder during build process.
 
