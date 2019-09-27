/*MZ_Ping_Fps_Kicker, Kontrols Laggi Players
Copyright(C) 27.09.2019  MasterZyper 🐦
Contact: masterzyper @reloaded-server.de
 You like to get a FiveM-Server?
 Visit ZapHosting*: https://zap-hosting.com/a/17444fc14f5749d607b4ca949eaf305ed50c0837

Support us on Patreon: https://www.patreon.com/gtafivemorg

For help with this Script visit: https://gta-fivem.org/

This program is free software; you can redistribute it and/or modify it under the terms of the
GNU General Public License as published by the Free Software Foundation; either version 3 of
the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
See the GNU General Public License for more details.
You should have received a copy of the GNU General Public License along with this program; 
if not, see <http://www.gnu.org/licenses/>.

*Affiliate-Link: Euch entstehen keine Kosten oder Nachteile. Kauf über diesen Link erwirtschaftet eine kleine prozentuale Provision für mich.
*/
using CitizenFX.Core;
using CitizenFX.Core.Native;
using mz_ping_fps_kicker_shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mz_ping_fps_kicker
{
    public class Ping_FPS_Checker : BaseScript
    {
        int highest_ping = 100;
        int ping_attemps = 5;
        int fps_attemps = 5;
        float lowest_fps = 15;
        int check_interval = 3000;

        string lg_kicked_high_ping = "HighPing";
        string lg_kicked_low_fps = "LowFPS";
        string lg_kicked_slow_response_time = "Slow Response Time";

        public Ping_FPS_Checker()
        {
            string resource_name = API.GetCurrentResourceName();
            string resource_author = "MasterZyper";
            Debug.Write($"{resource_name} by {resource_author} started");
            try
            {
                highest_ping = Convert.ToInt32(API.GetResourceMetadata(resource_name, "highest_ping", 0));
                ping_attemps = Convert.ToInt32(API.GetResourceMetadata(resource_name, "ping_attemps", 0));
                fps_attemps = Convert.ToInt32(API.GetResourceMetadata(resource_name, "fps_attemps", 0));
                lowest_fps = Convert.ToInt32(API.GetResourceMetadata(resource_name, "lowest_fps", 0));
                lg_kicked_high_ping = API.GetResourceMetadata(resource_name, "lg_kicked_high_ping", 0);
                lg_kicked_low_fps = API.GetResourceMetadata(resource_name, "lowest_fps", 0);
                lg_kicked_slow_response_time = API.GetResourceMetadata(resource_name, "lg_kicked_slow_response_time", 0);
            }
            catch (Exception)
            {
                Debug.Write($"Error while loading {resource_name} ");
            }

            Tick += CheckPing;
            Tick += SendConnectionInfos;

            EventHandlers.Add("MZ:SendCurrentFPSToServer", new Action<Player, float >(SendCurrentFPSToServer));
            EventHandlers.Add("MZ:ClientConnectionComplete", new Action<Player, bool>(ClientConnectionComplete));
        }

        readonly List<PlayerData> player_data = new List<PlayerData>();

        private void ClientConnectionComplete([FromSource]Player player, bool b)
        {
            foreach (PlayerData data in player_data)
            {
                if (data.Player_handle.Equals(player.Handle))
                {
                    data.ConnectionComplete = true;
                    data.Last_Response = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;                    
                    break;
                }
            }
        }
        private async Task SendConnectionInfos()
        {
            await Delay(500);
            List<dynamic> list = new List<dynamic>();
            foreach (PlayerData data in player_data) {
                PlayerTransfer dataset = new PlayerTransfer();
                dataset.name = data.nick;
                dataset.ping = data.Last_ping[ping_attemps - 1];
                dataset.fps = Convert.ToSingle(Math.Round(data.Last_fps[fps_attemps - 1], 2));
                list.Add(dataset);
            }
            foreach (Player player in Players)
            {
                player.TriggerEvent("MZ:SendCurrentConectionInfos", list);
            }
        }
     
        private void SendCurrentFPSToServer([FromSource]Player player, float fps)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            foreach(PlayerData data in player_data)
            {
                if (data.Player_handle.Equals(player.Handle))
                {
                    if (data.ConnectionComplete)
                    {
                        long time_difference = milliseconds - data.Last_Response;
                        if (time_difference > check_interval * 2 + 500)
                        {
                            API.DropPlayer(data.Player_handle, lg_kicked_slow_response_time);                            
                        }
                        data.Last_Response = milliseconds;
                        for (int i = 0; i < fps_attemps - 1; i++)
                        {
                            data.Last_fps[i] = data.Last_fps[i + 1];
                        }
                        data.Last_fps[fps_attemps - 1] = fps;
                    }
                }
            }
        }
        private string GetPlayerName(string handle) {
            foreach (Player player in Players)
            {
                if (player.Handle.Equals(handle))
                {
                    return player.Name;
                }
            }
            return "NAME NOT FOUND";
        }
        private void AddPlayerPing(string player_handle, int Ping)
        {
            bool success = false;
            foreach (PlayerData data in player_data)
            {
                if (data.Player_handle.Equals(player_handle))
                {
                    if (data.ConnectionComplete)
                    {
                        for (int i = 0; i < ping_attemps - 1; i++)
                        {
                            data.Last_ping[i] = data.Last_ping[i + 1];
                        }
                        data.Last_ping[ping_attemps - 1] = Ping;
                        success = true;
                    }
                    break;
                }
            }
            if (!success)
            {
                PlayerData data = new PlayerData();
                data.Player_handle = player_handle;
                data.nick = GetPlayerName(player_handle);
                data.Last_ping = new int[ping_attemps];
                data.Last_ping[ping_attemps-1] = Ping;
                data.Last_Response = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                data.Last_fps = new float[fps_attemps];
                for (int i = 0; i < fps_attemps; i++)
                {
                    data.Last_fps[i] = 1000;
                }
                player_data.Add(data);
            }
        }

        private void CheckPlayerPings()
        {
            foreach (PlayerData data in player_data)
            {
                int strikes = 0;
                foreach (int value in data.Last_ping) {
                    if (value > highest_ping) {
                        strikes++;
                    }
                }
                if (strikes >= ping_attemps)
                {
                    API.DropPlayer(data.Player_handle, lg_kicked_high_ping);
                }
            }
        }

        private void CheckPlayerFPS()
        {
            foreach (PlayerData data in player_data)
            {
                int strikes = 0;
                foreach (int value in data.Last_fps)
                {
                    if (value < lowest_fps)
                    {
                        strikes++;
                    }
                }
                if (strikes >= fps_attemps)
                {
                    API.DropPlayer(data.Player_handle, lg_kicked_low_fps);
                }
            }
        }
        private void RemoveOflinePlayers()
        {
            List<PlayerData> data_to_remove = new List<PlayerData>();
            foreach (PlayerData data in player_data)
            {
                bool success = false;
                foreach (Player player in Players)
                {
                    if (data.Player_handle.Equals(player.Handle))
                    {
                        success = true;
                        break;
                    }  
                }
                if (!success)
                {
                    data_to_remove.Add(data);
                }
            }
            foreach (PlayerData data in data_to_remove)
            {
                player_data.Remove(data);
            }
        }
        private void RequestFPSFromAllPlayer()
        {
            foreach (Player player in Players)
            {
                player.TriggerEvent("MZ:RequestPlayerFPS", true);
            }
        }
        private async Task CheckPing()
        {
            RemoveOflinePlayers();
            //Collect Pings
            foreach (Player player in Players)
            {
                AddPlayerPing(player.Handle, player.Ping);                
            }
            CheckPlayerPings();
            CheckPlayerFPS();
            RequestFPSFromAllPlayer();

            await Delay(check_interval);
        }
    }
}
