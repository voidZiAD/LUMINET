/* Project developed & created by voidZiAD on GitHub
 * Copyright 2023
 * Trademark LUMINET VPN
 * What's LUMINET: An Automated & Secure VPN that assures connections with speed and security, cost-free.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinBlur;
using static WinBlur.UI;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Handler;
using CefSharp.DevTools.Page;
using System.IO;
using static Vanara.PInvoke.IpHlpApi;
using WireGuardNT_PInvoke;
using WireGuardNT_PInvoke.WireGuard;
using System.Diagnostics;

namespace LUMINET
{
    public partial class LUMINET : Form
    {

        string DownloadsDirectoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\LUMINET SERVER DATA\\";
        Random random = new Random();
        ChromiumWebBrowser chromiumWebBrowser1;

        public LUMINET()
        {
            InitializeComponent();
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParams = base.CreateParams;
                handleParams.ExStyle |= 0x02000000;
                return handleParams;
            }
        }

        private void LUMINET_Load(object sender, EventArgs e)
        {

            CefSettings settings = new CefSettings();

            Cef.Initialize(settings);

            chromiumWebBrowser1 = new ChromiumWebBrowser();
            chromiumWebBrowser1.DownloadHandler = new MyCustomDownloadHandler();
            chromiumWebBrowser1.Location = new Point(399, 34);

            pnLUMINET.Controls.Add(chromiumWebBrowser1);

        }

        private async void LoopTest_Tick(object sender, EventArgs e)
        {

            LoopTest.Interval = 100;

            var script = @"(function() { if (document.getElementsByClassName('alert alert-success')[0] != null) { return true;} else { return false; } })();";
            JavascriptResponse response = await chromiumWebBrowser1.EvaluateScriptAsync(script);

            if ((bool)response.Result == true)
            {

                LoopTest.Stop();

                chromiumWebBrowser1.ExecuteScriptAsync(@"document.getElementsByClassName('btn btn-primary')[6].click();");

                chromiumWebBrowser1.ExecuteScriptAsync(@"document.getElementsByClassName('btn btn-primary btn-sm')[0].click();");

                LoopForConnect.Start();

            }
            
        }

        private void ConnectToWireGuardUsingConfig(string configPath)
        {

            /* Implement Connection using Config
             * Also, server checks need to be implemented. How it'll work:
             * 
             * When a server is created and the conf is downloaded, another file is created specially for that .conf file. Inside the file is written the date of expiry for the server.
             * Whenever the VPN attempts to connect to the server using the .conf on your computer in the %AppData% folder, it must check if the expiry date is today. If so, it deletes the .conf, creates a new server, downloads the new .conf, then connects to it.
             * The only thing that needs to be added is the connection after the server checks, the downloading part and everything else is done.
             * 
             * I have tried using the WireGuardNT_PInvoke Library to create a WireGuard client, but it keeps giving me the exception: "No such host exists", or something like that.
             * I've tried using the configs on the WireGuard app itself, and it works perfectly fine, so I'm not exactly sure what the issue is.
             * If you want to play around with the library and try to make it work, here's the GitHub Link: https://github.com/damob-byun/WireGuardNTSharp
             *
             * Also check the LoopForConnect_Tick, I've provided a note for something else needed to be implemented.
             */

            // Code that I used for WireGuardNT_PInvoke Library to create a client & connect using the .conf file:

            WgConfig WgConfig = new WgConfig();

            var baseName = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var configFile = System.IO.Path.Combine(baseName, DownloadsDirectoryPath + ValueSave.ConfName);

            var configAllLinesLine = File.ReadAllLines(configFile);

            var adapterName = "LUMINET Tunnel";
            var tunnelType = "LUMINET Tunnel";

            var adapter = new Adapter(adapterName, tunnelType);
            adapter.ParseConfFile(configAllLinesLine, out WgConfig);
            adapter.SetConfiguration(WgConfig);
            adapter.SetStateUp();

        }

        private void btnUKConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnUKConnect.Checked)
            {

                lbStatus.Text = "Connecting to the UK";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\UK.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\UK.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 306, 387, 477, 549 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "UK.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnNLConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnNLConnect.Checked)
            {

                lbStatus.Text = "Connecting to the NL";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\NL.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\NL.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 197 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "NL.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnFRConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnFRConnect.Checked)
            {

                lbStatus.Text = "Connecting to the FR";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\FR.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\FR.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 131, 139, 330, 339, 412, 493, 502 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "FR.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnGRConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnGRConnect.Checked)
            {

                lbStatus.Text = "Connecting to the GR";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\GR.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\GR.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 123, 347, 420, 509 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "GR.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnPLConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnPLConnect.Checked)
            {

                lbStatus.Text = "Connecting to the PL";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\PL.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\PL.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 116, 355, 429 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "PL.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnCAConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnCAConnect.Checked)
            {

                lbStatus.Text = "Connecting to the CA";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\CA.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\CA.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 163, 234, 323, 404, 485 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "CA.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnAUConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnAUConnect.Checked)
            {

                lbStatus.Text = "Connecting to the AU";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\AU.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\AU.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 171, 314, 396 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "AU.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void btnSGConnect_Click(object sender, EventArgs e)
        {

            LoopTest.Stop();
            LoopTest.Interval = 10000;

            if (btnSGConnect.Checked)
            {

                lbStatus.Text = "Connecting to the SG";
                lbStatus.ForeColor = Color.DarkOrchid;
                lbSmallStatus.Text = "ON";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

                if (File.Exists(DownloadsDirectoryPath + $"\\SG.conf"))
                {

                    ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + $"\\SG.conf");

                }
                else
                {

                    List<int> integerList = new List<int> { 217, 226, 147, 155, 107, 242, 250, 258, 266, 274, 282, 290, 298, 363, 371, 379, 437, 445, 453, 461, 517, 525, 533, 541 };
                    int randomIndex = random.Next(0, integerList.Count);
                    int selectedInteger = integerList[randomIndex];

                    chromiumWebBrowser1.LoadUrl($"https://vpn.sshs8.com/accounts/WIREGUARD/{selectedInteger}");

                    chromiumWebBrowser1.ExecuteScriptAsyncWhenPageLoaded(@"
const passwordInput = document.getElementById('password');

passwordInput.value = '" + RandomString(6) + @"';

const event = new Event('input', {bubbles: true,
  cancelable: true,
});
passwordInput.dispatchEvent(event);
document.getElementsByClassName('btn btn-primary')[0].click();");

                    ValueSave.ConfName = "SG.conf";

                    LoopTest.Start();

                }

            }
            else
            {

                lbStatus.Text = "You're not connected.";
                lbStatus.ForeColor = Color.Silver;
                lbSmallStatus.Text = "OFF";
                lbSmallStatus.Left = (guna2Panel2.ClientSize.Width - lbSmallStatus.Size.Width) / 2;

            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Cef.Shutdown();
            Application.Exit();
        }

        private void btnAutoConnect_Click(object sender, EventArgs e)
        {
            btnSGConnect.PerformClick(); // This has the most servers, so it's probably the best so I won't make it any harder for myself and code ping checks.
        }

        private void LoopForConnect_Tick(object sender, EventArgs e)
        {
            if (ValueSave.ConfName == "UK.conf" && File.Exists(DownloadsDirectoryPath + ValueSave.ConfName) && ValueSave.ConfSaved == true)
            {

                LoopForConnect.Stop();
                ValueSave.ConfSaved = false;

                ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + ValueSave.ConfName);

            }
            else if (ValueSave.ConfName == "NL.conf" && File.Exists(DownloadsDirectoryPath + ValueSave.ConfName) && ValueSave.ConfSaved == true)
            {

                LoopForConnect.Stop();
                ValueSave.ConfSaved = false;

                ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + ValueSave.ConfName);

            }
            else if (ValueSave.ConfName == "SG.conf" && File.Exists(DownloadsDirectoryPath + ValueSave.ConfName) && ValueSave.ConfSaved == true)
            {

                LoopForConnect.Stop();
                ValueSave.ConfSaved = false;

                ConnectToWireGuardUsingConfig(DownloadsDirectoryPath + ValueSave.ConfName);

            }

            // Implement handling for other locations
        }
    }
}
