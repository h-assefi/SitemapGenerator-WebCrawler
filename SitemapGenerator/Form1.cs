using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SitemapGenerator
{
    public partial class frmSitemapGenerator : Form
    {
        public frmSitemapGenerator()
        {
            InitializeComponent();
        }

        // Stores discovered links and their URL depth (number of segments)
        List<KeyValuePair<string, int>> linkList = new List<KeyValuePair<string, int>>();
        // Queue for URLs to crawl
        List<string> queueToCrawl = new List<string>();

        /// <summary>
        /// Handles the click event for the "Start Crawling" button.
        /// Crawls the website, collects internal links, and generates a sitemap.xml file.
        /// </summary>
        private void btnStartCrawling_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUri.Text.Trim()))
            {
                HtmlWeb web = new HtmlWeb();

                // Load the initial page
                HtmlAgilityPack.HtmlDocument htmlDoc = web.Load(txtUri.Text);
                Uri uri = new Uri(txtUri.Text.Trim());

                // Extract all anchor tags from the initial page
                var anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                if (anchorNodes == null)
                {
                    MessageBox.Show("No links found on the page.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Filter out already discovered links
                var undiscoveredLinks = anchorNodes
                    .Where(a => a.Attributes["href"] != null && !linkList.Any(l => l.Key.Equals(a.Attributes["href"].Value, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                // Filter and normalize links, exclude social/mailto, and keep only internal/product links
                List<KeyValuePair<string, int>> newLinks = (from link in undiscoveredLinks
                    let href = link.Attributes["href"].Value
                    where
                        !href.Contains("#") &&
                        !href.Contains("twitter") &&
                        !href.Contains("facebook") &&
                        !href.Contains("whatsapp") &&
                        !href.Contains("telegram") &&
                        !href.Contains("mailto") &&
                        (href.Contains(uri.Host) || href.Contains("products?page"))
                    select new KeyValuePair<string, int>(
                        href,
                        new Uri(href.Contains(uri.Host) ? href : (uri.AbsoluteUri + href)).Segments.Count()
                    )).Distinct().ToList();

                // Add discovered links to the list and queue
                linkList.AddRange(newLinks);
                queueToCrawl.AddRange(newLinks.Select(z => z.Key));

                // Crawl all queued URLs
                while (queueToCrawl.Count > 0)
                {
                    // Load the next URL in the queue
                    string nextUrl = queueToCrawl[0].Contains(uri.Host) ? queueToCrawl[0] : (uri.AbsoluteUri + queueToCrawl[0]);
                    htmlDoc = web.Load(nextUrl);
                    anchorNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (anchorNodes != null)
                    {
                        // Filter out already discovered links
                        undiscoveredLinks = anchorNodes
                            .Where(a => a.Attributes["href"] != null && !linkList.Any(l => l.Key.Equals(a.Attributes["href"].Value, StringComparison.OrdinalIgnoreCase)))
                            .ToList();

                        // Filter and normalize links, exclude social/mailto, and keep only internal/product links
                        newLinks = (from link in undiscoveredLinks
                            let href = link.Attributes["href"].Value.Trim().Replace(" ", "").ToLower()
                            where
                                !href.Contains("#") &&
                                !href.Contains("mailto") &&
                                !href.Contains("twitter") &&
                                !href.Contains("facebook") &&
                                !href.Contains("whatsapp") &&
                                !href.Contains("telegram") &&
                                (href.Contains(uri.Host) || href.Contains("products?page"))
                            select new KeyValuePair<string, int>(
                                href,
                                new Uri(href.Contains(uri.Host) ? href : (uri.AbsoluteUri + href)).Segments.Count()
                            )).Distinct().ToList();

                        // Add new links to the list and queue
                        linkList.AddRange(newLinks);
                        queueToCrawl.AddRange(newLinks.Select(z => z.Key));
                    }
                    // Remove the crawled URL from the queue
                    queueToCrawl.RemoveAt(0);
                }

                // Build the sitemap XML
                StringBuilder sb = new StringBuilder();
                string priority;
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>")
                  .AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

                for (int i = 0; i < linkList.Count; i++)
                {
                    // Exclude URLs containing any of the exclude keywords
                    if (!string.IsNullOrEmpty(txtExclude.Text.Trim()))
                    {
                        string[] excludeItems = txtExclude.Text.Trim().Split(',');
                        bool shouldExclude = excludeItems.Any(ex => linkList[i].Key.Contains(ex.ToLower()));
                        if (shouldExclude)
                            continue;
                    }

                    // Assign priority based on URL depth
                    switch (linkList[i].Value)
                    {
                        case 1: priority = "0.86"; break;
                        case 2: priority = "0.79"; break;
                        case 3: priority = "0.74"; break;
                        case 4: priority = "0.69"; break;
                        case 5: priority = "0.64"; break;
                        case 6: priority = "0.59"; break;
                        default: priority = "0.5"; break;
                    }

                    // Write the URL entry to the sitemap
                    string url = linkList[i].Key.Replace("&", "&amp;").Trim();
                    if (url.StartsWith("pro"))
                        url = uri.AbsoluteUri + url;
                    if (!url.EndsWith("/"))
                        url += "/";

                    sb.AppendLine("<url>")
                      .Append("<loc>").Append(url).AppendLine("</loc>")
                      .Append("<lastmod>").Append(DateTime.Now.Date.ToString("yyyy-MM-dd")).AppendLine("</lastmod>")
                      .Append("<priority>").Append(priority).AppendLine("</priority>")
                      .AppendLine("</url>");
                }
                sb.Append("</urlset>");

                // Save the sitemap to a file
                using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\sitemap.xml"))
                {
                    writer.Write(sb.ToString());
                }

                // Notify the user
                MessageBox.Show("Sitemap Generated Successfully", "Site Map Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Show error if no URL is entered
                MessageBox.Show("Enter The Uri", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUri.Focus();
            }
        }
    }
}
