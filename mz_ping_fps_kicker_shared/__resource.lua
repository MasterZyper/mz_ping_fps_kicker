--[[
MZ_Ping_Fps_Kicker, Kontrols Laggi Players
Copyright (C) 27.08.2019  MasterZyper 🐦
Contact: masterzyper@reloaded-server.de
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

]]
resource_manifest_version '44febabe-d386-4d18-afbe-5e627f4af937'

server_script 'mz_ping_fps_kicker_shared.net.dll';
client_script 'mz_ping_fps_kicker_shared.net.dll'

server_script 'mz_ping_fps_kicker.net.dll';
client_script 'mz_ping_fps_kicker_client.net.dll'

--Konfig:
--CMD zum Anzeigen der Spieler FPS/Ping
cmd_show_fps 'show_fps'
--Check-Settings
highest_ping '100'
lowest_fps '15'
ping_attemps '5'
fps_attemps '5'
check_interval '3000'
--UI Settings, für FPS Anzeige
ui_display_x '0.8'
ui_display_y '0.025'
ui_text_offset '0.02'
ui_text_size '0.25'
--Language
lg_kicked_high_ping 'You are kicked because of high ping!'
lg_kicked_low_fps 'You are kicked because of low fps!'
lg_kicked_slow_response_time 'You have a to slow response time'