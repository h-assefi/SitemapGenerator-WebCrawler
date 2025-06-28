using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SitemapGenerator
{
    public partial class frmSitemapGenerator : Form
    {
        public frmSitemapGenerator()
        {
            InitializeComponent();
        }

        // Stores discovered links and their URL depth
        List<KeyValuePair<string, int>> linkList = new List<KeyValuePair<string, int>>();
        // Queue for URLs to crawl
        List<string> queueToCrawl = new List<string>();

        // Main event handler for starting the crawling and sitemap generation
        private void btnStartCrawling_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUri.Text.Trim()))
            {
                HtmlWeb web = new HtmlWeb();

                // Load the initial page
                HtmlAgilityPack.HtmlDocument htmlDoc = web.Load(txtUri.Text);
                Uri uri = new Uri(txtUri.Text.Trim());

                // Extract all anchor tags from the initial page
                var node = htmlDoc.DocumentNode.SelectNodes("//a");
                // Filter out already discovered links
                var x = node.ToList().Where(s => !linkList.Any(s2 => s2.Key.ToLower().Equals(s.Attributes["href"].Value.ToLower()))).ToList();
                // Filter and normalize links, exclude social/mailto, and keep only internal/product links
                List<KeyValuePair<string, int>> y = (from q in x
                                                     where
                                                     q.Attributes["href"] != null && !q.Attributes["href"].Value.Contains("#") && !q.Attributes["href"].Value.Contains("twitter") && !q.Attributes["href"].Value.Contains("facebook") &&
                                !q.Attributes["href"].Value.Contains("whatsapp") && !q.Attributes["href"].Value.Contains("telegram") &&
                                !q.Attributes["href"].Value.Contains("mailto") &&
                                (q.Attributes["href"].Value.Contains(uri.Host) || q.Attributes["href"].Value.Contains("products?page"))
                                                     select new KeyValuePair<string, int>(q.Attributes["href"].Value, new Uri(q.Attributes["href"].Value.Contains(uri.Host) ? q.Attributes["href"].Value : (uri.AbsoluteUri + q.Attributes["href"].Value)).Segments.Count())).Distinct().ToList();

                // Add discovered links to the list and queue
                linkList.AddRange(y);
                queueToCrawl.AddRange(y.Select(z => z.Key));

                // Crawl all queued URLs
                while (queueToCrawl.Count > 0)
                {
                    // Load the next URL in the queue
                    htmlDoc = web.Load(queueToCrawl[0].Contains(uri.Host) ? queueToCrawl[0] : (uri.AbsoluteUri + queueToCrawl[0]));
                    node = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (node != null)
                    {
                        // Filter out already discovered links
                        x = node.ToList().Where(s => !linkList.Any(s2 => s2.Key.ToLower().Equals(s.Attributes["href"]?.Value.ToLower()))).ToList();
                        // Filter and normalize links, exclude social/mailto, and keep only internal/product links
                        y = (from q in x

                             where 
                             q.Attributes["href"] != null && 
                             !q.Attributes["href"].Value.Contains("#") &&
                                !q.Attributes["href"].Value.Contains("mailto") && !q.Attributes["href"].Value.Contains("twitter") && !q.Attributes["href"].Value.Contains("facebook") &&
                                !q.Attributes["href"].Value.Contains("whatsapp") && !q.Attributes["href"].Value.Contains("telegram") &&
                                (q.Attributes["href"].Value.Contains(uri.Host) || q.Attributes["href"].Value.Contains("products?page"))
                             select new KeyValuePair<string, int>(q.Attributes["href"].Value.Trim().Replace(" ", "").ToLower(), new Uri(q.Attributes["href"].Value.Trim().Replace(" ", "").Contains(uri.Host) ? q.Attributes["href"].Value.Trim().Replace(" ", "") : (uri.AbsoluteUri + q.Attributes["href"].Value.Trim().Replace(" ", ""))).Segments.Count())).Distinct().ToList();
                        // Add new links to the list and queue
                        linkList.AddRange(y);
                        queueToCrawl.AddRange(y.Select(z => z.Key));
                    }
                    // Remove the crawled URL from the queue
                    queueToCrawl.RemoveAt(0);
                }

                // Build the sitemap XML
                StringBuilder sb = new StringBuilder();
                string priority;
                sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>").AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                for (int i = 0; i < linkList.Count; i++)
                {
                    // Exclude URLs containing any of the exclude keywords
                    if (!string.IsNullOrEmpty( txtExclude.Text.Trim()))
                    {
                        string[] excludeItems = txtExclude.Text.Trim().Split(',');
                        for (int z = 0; z < excludeItems.Length; z++)
                        {
                            if (linkList[i].Key.Contains(excludeItems[z].ToLower()))
                            {
                                goto End;
                            }
                        }
                    }
                    // Assign priority based on URL depth
                    if (linkList[i].Value == 1)
                        priority = "0.86";
                    else if (linkList[i].Value == 2)
                        priority = "0.79";
                    else if (linkList[i].Value == 3)
                        priority = "0.74";
                    else if (linkList[i].Value == 4)
                        priority = "0.69";
                    else if (linkList[i].Value == 5)
                        priority = "0.64";
                    else if (linkList[i].Value == 6)
                        priority = "0.59";
                    else
                        priority = "0.5";

                    // Write the URL entry to the sitemap
                    sb.AppendLine("<url>").Append("<loc>").Append(linkList[i].Key.Replace("&", "&amp;").Trim().StartsWith("pro") ? (uri.AbsoluteUri + linkList[i].Key.Replace("&", "&amp;").Trim()) : linkList[i].Key.Replace("&", "&amp;").Trim()).Append(linkList[i].Key.Replace("&", "&amp;").Trim().EndsWith("/") ? "" : "/").AppendLine("</loc>")
                        .Append("<lastmod>").Append(DateTime.Now.Date.ToString("yyyy-MM-dd")).AppendLine("</lastmod>")
                        .Append("<priority>").Append(priority).AppendLine("</priority>")
                        .AppendLine("</url>");

                End:;
                }
                sb.Append("</urlset>");

                // Save the sitemap to a file
                StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + @"\sitemap.xml");
                writer.Write(sb.ToString());
                writer.Close();

                // Notify the user
                MessageBox.Show("Sitemap Generated Successfully", "Site Map Genetator", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
