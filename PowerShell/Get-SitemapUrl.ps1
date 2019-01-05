#Requires -Version 6.0
Set-StrictMode -Version Latest
function Get-SitemapUrl {
    [OutputType([string[]])]
    param(
        [Parameter(Mandatory = $true)]
        [string]$Url
    )

    $res = Invoke-Webrequest "$Url/sitemap.xml"
    if ($res.StatusCode -ne 200) {
        throw $res
    }

    [xml]$index = $res.Content
    $sitemaps = $index.sitemapindex.sitemap.loc
    [string[]]$urls = $sitemaps | Foreach-Object {
        $eachRes = Invoke-Webrequest $_
        [xml]$page = $eachRes.Content
        Write-Output $page.urlset.url.loc
    }
    Write-Output $urls
}