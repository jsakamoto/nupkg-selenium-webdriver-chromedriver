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

### How to configure HTTP proxy? / プロクシの設定

This package downloading "chromedriver.exe" on the fly with using
HTTP proxy settings from following order.

1. At first, try to get from HTTP_PROXY system environment variable specified the format like a ```http://username:password@myproxy:8080```.
2. Second, try to get from NuGet.config file in ```%APPDATA%\NuGet``` folder. You can setup proxy settings for NuGet by follow command line:  
```> nuget.exe config -set http_proxy=http://myproxy:8080 http_proxy.user=user http_proxy.password=passwrd```
3. At last, try to get from system default proxy settings that Control Panle - Internet Options.

このパッケージは、下記の順序で HTTP プロクシ設定を使用して "chromedriver.exe" をその場でダウンロードします。

1. はじめに、HTTP_PROXY システム環境変数に設定された、```http://username:password@myproxy:8080``` 形式の指定の取得を試みます。
2. 次に、```%APPDATA%\NuGet``` フォルダにある NuGet.config ファイルからの取得を試みます。NuGet のプロクシ設定は、次のコマンドラインで設定できます:  
```> nuget.exe config -set http_proxy=http://myproxy:8080 http_proxy.user=user http_proxy.password=passwrd```
3. 最後に、コントロールパネル - インターネット設定の、システム既定のプロクシ設定の取得を試みます。

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
 
