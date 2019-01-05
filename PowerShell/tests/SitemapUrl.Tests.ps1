$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace '\.Tests\.', '.'
. "$here\$sut"

$result = Get-SitemapUrl -Url "http://guitarrapc-tech.hatenablog.com"

Describe "SitemapUrl" {
    It "each item should be string" {
        $result | Should -BeOfType System.String
    }
    It "contains expected url" {
        $result | Should -Contain "http://tech.guitarrapc.com/about"
    }
}
