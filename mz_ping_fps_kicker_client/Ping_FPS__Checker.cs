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

namespace mz_ping_fps_kicker_client
{
    public class Ping_FPS__Checker : BaseScript
    {
        string cmd_show_fps = "show_fps";
        float display_x = 0.8f; //Links/rechts
        float display_y = 0.025f; //Höhe
        float text_offset = 0.02f;
        float text_size = 0.25f;

        public Ping_FPS__Checker()
        {
            string resource_name = API.GetCurrentResourceName();
            string resource_author = "MasterZyper";
            Debug.Write($"{resource_name} by {resource_author} started");
            try
            {
                cmd_show_fps = API.GetResourceMetadata(resource_name, "cmd_show_fps", 0);
                display_x = Convert.ToSingle(API.GetResourceMetadata(resource_name, "ui_display_x", 0));
                display_y = Convert.ToSingle(API.GetResourceMetadata(resource_name, "ui_display_y", 0));
                text_offset = Convert.ToSingle(API.GetResourceMetadata(resource_name, "ui_text_offset", 0));
                text_size = Convert.ToSingle(API.GetResourceMetadata(resource_name, "ui_text_size", 0));
            }
            catch (Exception)
            {
                Debug.Write($"Error while loading {resource_name} ");
            }

            EventHandlers.Add("MZ:RequestPlayerFPS", new Action<bool>(RequestPlayerFPS));
            EventHandlers.Add("MZ:SendCurrentConectionInfos", new Action<List<dynamic>>(SendCurrentConectionInfos));
            Tick += DrawPlayerInfos;
            API.RegisterCommand(cmd_show_fps, new Action<int, List<object>, string>(async (player, value, raw) =>
            {
                show_fps = !show_fps;               
            }), false);
        }

        private void RequestPlayerFPS(bool b)
        {
            TriggerServerEvent("MZ:SendCurrentFPSToServer", Game.FPS);
        }

        private void SendCurrentConectionInfos(List<dynamic> data)
        {
            dataset = data;
        }

        bool show_fps = false;
        List<dynamic> dataset = new List<dynamic>();
        bool login_reported = false;
        private async Task DrawPlayerInfos()
        {
            if (!Game.IsLoading)
            {
                if (!login_reported)
                {
                    login_reported = true;
                    TriggerServerEvent("MZ:ClientConnectionComplete", true);
                }
            }
            if (!show_fps)
            {
                await Delay(1000);
            }
            else
            {
                Render.DrawTextCenter("Player", 0, text_size, display_x, display_y - text_offset);
                Render.DrawTextCenter("FPS", 0, text_size, display_x + 0.05f, display_y - text_offset);
                Render.DrawTextCenter("PING", 0, text_size, display_x + 0.1f, display_y - text_offset);
                int i = 0;
                foreach (dynamic player_data in dataset)
                {
                    Render.DrawTextCenter(player_data.name, 0, text_size, display_x, display_y + i * text_offset);
                    Render.DrawTextCenter(player_data.fps, 0, text_size, display_x + 0.05f, display_y + i * text_offset);
                    Render.DrawTextCenter(player_data.ping, 0, text_size, display_x + 0.1f, display_y + i * text_offset);
                    i++;
                }
            }
        }
    }
}
