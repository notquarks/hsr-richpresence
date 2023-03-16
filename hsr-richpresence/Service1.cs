using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DiscordRPC;

namespace hsr_richpresence
{
    public partial class HSRservice : ServiceBase
    {
        private DiscordRpcClient client;
        private string gameProcessName = "StarRail";
        private bool isGameRunning = false;


        public HSRservice()
        {
            InitializeComponent();
            client = new DiscordRpcClient("1085848800448483358");

        }

        protected override void OnStart(string[] args)
        {
            client.Initialize();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 5000; // 5 seconds
            timer.Elapsed += CheckGameStatus;
            timer.Start();
        }
        
        private void CheckGameStatus(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool isGameRunningNow = Process.GetProcessesByName(gameProcessName).Length > 0;
            if (isGameRunningNow != isGameRunning)
            {
                if (isGameRunningNow)
                {
                    // Game started
                    client.SetPresence(new RichPresence()
                    {
                        Assets = new Assets()
                        {
                            LargeImageKey = "hsr",
                            LargeImageText = "H:SR"
                        },
                        Timestamps = new Timestamps()
                        {
                            Start = DateTime.UtcNow
                        }

                    });
                }
                else
                {
                    // Game stopped
                    client.ClearPresence();
                }
                isGameRunning = isGameRunningNow;
            }
        }
        
        protected override void OnStop()
        {
            client.Dispose();
        }
    }
}
