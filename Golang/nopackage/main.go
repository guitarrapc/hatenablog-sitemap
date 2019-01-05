package main

import (
	"encoding/xml"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
)

func main() {
	if len(os.Args) != 2 {
		fmt.Fprintf(os.Stderr, "Usage: %s URL\n", os.Args[0])
		os.Exit(1)
	}
	// sitemapindex
	sitemapURL := os.Args[1] + "/sitemap.xml"
	index, err := FetchSitemapIndex(sitemapURL)
	if err != nil {
		fmt.Println(err)
		return
	}

	// sitemaps
	urls := make([]Url, 0)
	for _, loc := range index.SitemapLocations {
		r, err := FetchSitemap(loc.Loc)
		if err != nil {
			fmt.Println(err)
			return
		}
		urls = append(urls, r.Urls...)
	}

	// output
	for _, url := range urls {
		fmt.Println(url.Loc)
	}
}

func fetchXML(url string) (data []byte, err error) {
	response, err := http.Get(url)
	if err != nil {
		log.Fatal(err)
	} else {
		defer response.Body.Close()
		data, err = ioutil.ReadAll(response.Body)
		if err != nil {
			log.Fatal(err)
		}
	}
	return
}

// FetchSitemapIndex godoc
// @summary : Fetch a remote sitemap index
func FetchSitemapIndex(url string) (s *SitemapIndex, err error) {
	xmlData, err := fetchXML(url)
	if err != nil {
		return
	}

	s = &SitemapIndex{}
	err = xml.Unmarshal(xmlData, s)
	if err != nil {
		return
	}

	return
}

// FetchSitemap godoc
// @summary : Fetch a sitemap
func FetchSitemap(url string) (s *Sitemap, err error) {
	xmlData, err := fetchXML(url)
	if err != nil {
		return
	}

	s = &Sitemap{}
	err = xml.Unmarshal(xmlData, s)
	if err != nil {
		return
	}

	return
}

// SitemapIndex godoc
// @summary sitemapindex
type SitemapIndex struct {
	XMLName          xml.Name          `xml:"sitemapindex"`
	SitemapLocations []SitemapLocation `xml:"sitemap"`
}

// SitemapLocation godoc
// @summary element within sitemapindex
type SitemapLocation struct {
	Loc        string `xml:"loc"`
	Lastmod    string `xml:"lastmod"`
	Changefreq string `xml:"changefreq"`
	Priority   string `xml:"priority"`
}

// Sitemap godoc
// @summary urlsets
type Sitemap struct {
	XMLName xml.Name `xml:"urlset"`
	Urls    []Url    `xml:"url"`
}

// Url godoc
// @summary url element within urlset
type Url struct {
	Loc     string `xml:"loc"`
	Lastmod string `xml:"lastmod"`
}

func (s *SitemapLocation) String() string {
	return s.Loc
}

func (u *Url) String() string {
	return u.Loc
}
