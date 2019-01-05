<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    var url = "http://tech.guitarrapc.com";
    var res = await GetHatenaBlogEntries(url + "/sitemap.xml");
    res.Dump();
}

async Task<string[]> GetHatenaBlogEntries(string url)
{
    var client = new HttpClient();
    var res = await client.GetStringAsync(url);
    XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
    var sitemaps = XElement.Parse(res).Descendants(ns + "loc").Select(x => x.Value).ToArray();
    var urls = await Task.WhenAll(sitemaps.Select(async x =>
    { 
        var eachRes = await client.GetStringAsync(x);
        return XElement.Parse(eachRes).Descendants(ns + "loc").Select(y => y.Value).ToArray();
    }));
    var result = urls.SelectMany(x => x).ToArray();
    return result;
}