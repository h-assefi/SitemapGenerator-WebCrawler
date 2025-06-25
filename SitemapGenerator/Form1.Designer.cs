namespace SitemapGenerator
{
    partial class frmSitemapGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtUri = new System.Windows.Forms.TextBox();
            this.btnStartCrawling = new System.Windows.Forms.Button();
            this.txtExclude = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Uri To Crawl";
            // 
            // txtUri
            // 
            this.txtUri.Location = new System.Drawing.Point(83, 33);
            this.txtUri.Name = "txtUri";
            this.txtUri.Size = new System.Drawing.Size(278, 20);
            this.txtUri.TabIndex = 1;
            this.txtUri.Text = "https://your-domain.com";
            // 
            // btnStartCrawling
            // 
            this.btnStartCrawling.Location = new System.Drawing.Point(15, 223);
            this.btnStartCrawling.Name = "btnStartCrawling";
            this.btnStartCrawling.Size = new System.Drawing.Size(111, 23);
            this.btnStartCrawling.TabIndex = 2;
            this.btnStartCrawling.Text = "Start Crawling";
            this.btnStartCrawling.UseVisualStyleBackColor = true;
            this.btnStartCrawling.Click += new System.EventHandler(this.btnStartCrawling_Click);
            // 
            // txtExclude
            // 
            this.txtExclude.Location = new System.Drawing.Point(83, 71);
            this.txtExclude.Name = "txtExclude";
            this.txtExclude.Size = new System.Drawing.Size(278, 122);
            this.txtExclude.TabIndex = 3;
            this.txtExclude.Text = "https://domain.com/mag/,page";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 83);
            this.label2.TabIndex = 0;
            this.label2.Text = "Exclude links that contain\r\n\r\nSeprate by Camma(,)";
            // 
            // frmSitemapGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 258);
            this.Controls.Add(this.txtExclude);
            this.Controls.Add(this.btnStartCrawling);
            this.Controls.Add(this.txtUri);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmSitemapGenerator";
            this.Text = "Sitemap Generator - Web Crawler";
            this.Load += new System.EventHandler(this.frmSitemapGenerator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUri;
        private System.Windows.Forms.Button btnStartCrawling;
        private System.Windows.Forms.RichTextBox txtExclude;
        private System.Windows.Forms.Label label2;
    }
}

