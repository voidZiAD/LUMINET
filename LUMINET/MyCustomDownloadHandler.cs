using CefSharp;
using System;
using System.IO;
using System.Windows.Forms;

namespace LUMINET
{
    class MyCustomDownloadHandler : IDownloadHandler
    {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;
        }

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (downloadItem.IsValid)
            {
                Console.WriteLine("== File information ========================");
                Console.WriteLine(" File URL: {0}", downloadItem.Url);
                Console.WriteLine(" Suggested FileName: {0}", downloadItem.SuggestedFileName);
                Console.WriteLine(" MimeType: {0}", downloadItem.MimeType);
                Console.WriteLine(" Content Disposition: {0}", downloadItem.ContentDisposition);
                Console.WriteLine(" Total Size: {0}", downloadItem.TotalBytes);
                Console.WriteLine("============================================");
            }

            OnBeforeDownloadFired?.Invoke(this, downloadItem);

            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    string DownloadsDirectoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\LUMINET SERVER DATA\\";

                    if (Directory.Exists(DownloadsDirectoryPath))
                    {

                        callback.Continue(
    Path.Combine(
        DownloadsDirectoryPath,
        ValueSave.ConfName
    ),
    showDialog: false
);

                    }
                    else
                    {

                        Directory.CreateDirectory(DownloadsDirectoryPath);

                        callback.Continue(
    Path.Combine(
        DownloadsDirectoryPath,
        ValueSave.ConfName
    ),
    showDialog: false
);

                    }
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            OnDownloadUpdatedFired?.Invoke(this, downloadItem);

            if (downloadItem.IsValid)
            {
                if (downloadItem.IsInProgress && (downloadItem.PercentComplete != 0))
                {
                    Console.WriteLine(
                        "Current Download Speed: {0} bytes ({1}%)",
                        downloadItem.CurrentSpeed,
                        downloadItem.PercentComplete
                    );
                }

                if (downloadItem.IsComplete)
                {
                    Console.WriteLine("The download has been finished !");
                    ValueSave.ConfSaved = true;
                }
            }
        }
    }
}
