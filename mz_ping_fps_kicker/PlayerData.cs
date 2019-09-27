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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mz_ping_fps_kicker
{
    class PlayerData
    {
        public string nick { get; set; }
        public string Player_handle { get; set;}
        public int[] Last_ping { get; set; }
        public float[] Last_fps { get; set; }
        public long Last_Response { get; set; }
        public bool ConnectionComplete { get; set; }
    }
}
