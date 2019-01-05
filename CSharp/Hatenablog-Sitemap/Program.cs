using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hatenablog_Sitemap
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentOutOfRangeException($"Usage: {System.Reflection.Assembly.GetExecutingAssembly().Location} URL");
            }

            var url = args[0] + "/sitemap.xml";
            var urls = await GetHatenaBlogEntries(url);
            foreach (var item in urls)
            {
                Console.WriteLine(item);
            }
        }

        static async Task<string[]> GetHatenaBlogEntries(string url)
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
    }
}
