# SitemapGenerator-WebCrawler

## Overview

SitemapGenerator-WebCrawler is a Windows Forms application that allows users to generate XML sitemaps for websites. The tool crawls a given website URL, collects all accessible links, and outputs a `sitemap.xml` file in the application's directory. This is useful for SEO purposes and for webmasters who need to submit sitemaps to search engines.

The application uses the HtmlAgilityPack library for HTML parsing and link extraction.

## Features

- Simple graphical interface for entering the website URL.
- Crawls the website and collects all internal links.
- Generates a standard `sitemap.xml` file.
- Displays success and error messages to guide the user.

## How to Use

1. **Build the Project**

   - Open `SitemapGenerator.sln` in Visual Studio.
   - Restore NuGet packages if prompted (HtmlAgilityPack).
   - Build the solution.

2. **Run the Application**

   - Start the application from Visual Studio or run `SitemapGenerator.exe` from the `bin/Debug` folder.

3. **Generate a Sitemap**
   - Enter the root URL of the website you want to crawl.
   - Click the "Start Crawling" button.
   - After crawling, a `sitemap.xml` file will be created in the application's directory.
   - A message box will confirm successful generation.

## Requirements

- .NET Framework (version as specified in the project)
- Windows OS
- [HtmlAgilityPack](https://html-agility-pack.net/) (included via NuGet)

## License

See [LICENSE](LICENSE) for details.
