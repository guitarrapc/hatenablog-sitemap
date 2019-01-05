package main

import (
	"fmt"
	"os"

	sitemap "github.com/yterajima/go-sitemap"
)

func main() {
	if len(os.Args) != 2 {
		fmt.Fprintf(os.Stderr, "Usage: %s URL\n", os.Args[0])
		os.Exit(1)
	}

	sitemapURL := os.Args[1] + "/sitemap.xml"
	xml, err := sitemap.Get(sitemapURL, nil)
	if err != nil {
		fmt.Println(err)
		return
	}

	for _, loc := range xml.URL {
		fmt.Println(loc.Loc)
	}
}
