using System;
using Crystals.Core.Configs;
using DiscordRPC;
using DiscordRPC.Logging;
using Terraria.ModLoader;

namespace Crystals.Core.Systems.DiscordSystem
{
    public class DiscordRPC : ModSystem
    {
        
        private static LogLevel logLevel = LogLevel.Trace;
		
        private static int discordPipe = -1;

        public static DiscordRpcClient gloClient;

        public bool Enabled => ModContent.GetInstance<MiscConfig>().Enabled;

        public override void PreUpdateTime()
        {
            if (!Enabled)
            {
                if (gloClient != null)
                {
                    gloClient.ClearPresence();
                }
            }
        }

        public override void OnModLoad()
        {
            var client = new DiscordRpcClient("1021527593503166504", pipe: discordPipe)
            {
                Logger = new ConsoleLogger(logLevel , true)
            };

            gloClient = client;
			
            client.OnReady += (sender, msg) =>
            {
                //Create some events so we know things are happening
                Console.WriteLine("Connected to discord with user {0}", msg.User.Username);
            };

            client.OnPresenceUpdate += (sender, msg) =>
            {
                //The presence has updated
                Console.WriteLine("Presence has been updated! ");
            };

            // == Initialize
            client.Initialize();

            if (Enabled)
            {
                client.SetPresence(new RichPresence()
                {
                    State = "In Menu",
                    Assets = new Assets()
                    {
                        LargeImageKey = "standartcrystal"
                    }
                });
            }
            else
            {
                client.ClearPresence();
            }
        }


        public override void OnWorldLoad()
        {
            if (gloClient != null && Enabled)
            {
                gloClient.SetPresence(new RichPresence()
                {
                    State = "In Game",
                    Timestamps = Timestamps.Now,
                    Assets = new Assets()
                    {
                        LargeImageKey = "shinycrystal"
                    }
                });
            }
        }

        public override void OnWorldUnload()
        {
            if (gloClient != null && Enabled)
            {
                gloClient.SetPresence(new RichPresence()
                {
                    State = "In Menu",
                    Assets = new Assets()
                    {
                        LargeImageKey = "standartcrystal"
                    }
                });
            }
        }
        

        public override void OnModUnload()
        {
            if (gloClient != null && Enabled)
            {
                gloClient.ClearPresence();
            }
        }
        
        
    }
}