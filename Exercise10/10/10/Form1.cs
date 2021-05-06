﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _10
{
    public partial class Form1 : Form
    {
        BindingSource resultBindingSource = new BindingSource();
        SimpleCrawler crawler = new SimpleCrawler();

        public Form1()
        {
            InitializeComponent();
            dgvResult.DataSource = resultBindingSource;
            crawler.PageDownloaded += Crawler_PageDownloaded;
            crawler.CrawlerStopped += Crawler_CrawlerStopped;
        }

        private void Crawler_CrawlerStopped(SimpleCrawler obj)
        {
            Action action = () => lblInfo.Text = "爬虫已停止";
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Crawler_PageDownloaded(SimpleCrawler crawler, string url, string info)
        {
            var pageInfo = new { Index = resultBindingSource.Count + 1, URL = url, Status = info };
            Action action = () => { resultBindingSource.Add(pageInfo); };
            if (this.InvokeRequired)
            {
                this.Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            resultBindingSource.Clear();
            crawler.StartURL = txtUrl.Text;

            Match match = Regex.Match(crawler.StartURL, SimpleCrawler.urlParseRegex);
            if (match.Length == 0) return;
            string host = match.Groups["host"].Value;
            crawler.HostFilter = "^" + host + "$";
            crawler.FileFilter = "((.html?|.aspx|.jsp|.php)$|^[^.]+$)";
            //crawler.Start();
            _ = crawler.Start();
            //Task.Run(() => crawler.Start());
            lblInfo.Text = "爬虫已启动....";

        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
