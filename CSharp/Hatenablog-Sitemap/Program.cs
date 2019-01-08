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

        private static async Task<string[]> GetHatenaBlogEntries(string url)
        {
            var client = new HttpClient();
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            async Task<string[]> FetchSitemapAsync(string dest)
            {
                var content = await client.GetStringAsync(dest);
                return XElement.Parse(content).Descendants(ns + "loc").Select(x => x.Value).ToArray();
            }

            var sitemaps = await FetchSitemapAsync(url);
            var urls = await Task.WhenAll(sitemaps.Select(x => FetchSitemapAsync(x)));
            var result = urls.SelectMany(x => x).ToArray();
            return result;
        }
    }
}
