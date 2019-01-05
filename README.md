## hatenablog-sitemap

Retrieve sitemaps for hatenablog and others via multiple languages.

### Supported

* C# (C# 8.0 w/.NETCore 2.2)
* PowerShell (PowerShell 6.1 w/.NETCore 2.1)
* Golang

### CSharp

Sample:

```csharp
cd CSharp/Hatenablog-Sitemap
dotnet publish -o bin/publish
cd bin/publish
dotnet Hatenablog-Sitemap.dll http://tech.guitarrapc.com
```

### PowerShell

```powershell
pwsh
cd PowerShell
. ./Get-SitemapUrl.ps1
Get-SitemapUrl -Url http://tech.guitarrapc.com
```

### Golang

```golang
cd Golang/nopackage
go build
./nopackage http://tech.guitarrapc.com
```