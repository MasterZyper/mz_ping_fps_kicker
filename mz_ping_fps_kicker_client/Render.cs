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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mz_ping_fps_kicker_client
{
    public class Render : BaseScript
    {

        public static void DrawTextCenter(float text, int font, float fontsize, float display_x, float display_y, int r = 255, int g = 255,
         int b = 255, int a = 255, bool blink = false)
        {
            DrawTextCenter(text + "", font, fontsize, display_x, display_y, r, g, b, a, blink);
        }
        public static void DrawTextCenter(string text, int font, float fontsize, float display_x, float display_y, int r = 255, int g = 255,
        int b = 255, int a = 255, bool blink = false)
        {
            if (blink)
            {
                a = Convert.ToInt32(Math.Abs(Math.Cos(DateTime.Now.Millisecond / 500) * 100000) % 255);
            }
            API.SetTextFont(font);
            API.SetTextProportional(true);
            API.SetTextScale(0.0f, fontsize);
            API.SetTextDropshadow(1, 0, 0, 0, a);
            API.SetTextEdge(1, 255, 0, 0, a);
            API.SetTextColour(r, g, b, a);
            API.SetTextDropShadow();
            API.SetTextOutline();
            API.SetTextEntry("STRING");
            API.SetTextCentre(true);
            API.AddTextComponentString(text);
            API.DrawText(display_x, display_y);
        }

        public static void DrawText(string text, int font, float fontsize, float display_x, float display_y, int r = 255, int g = 255,
            int b = 255, int a = 255, bool blink = false)
        {
            if (blink)
            {
                a = Convert.ToInt32(Math.Abs(Math.Cos(DateTime.Now.Millisecond / 500) * 100000) % 255);
            }
            API.SetTextFont(font);
            API.SetTextProportional(true);
            API.SetTextScale(0.0f, fontsize);
            API.SetTextDropshadow(1, 0, 0, 0, a);
            API.SetTextEdge(1, 0, 0, 0, a);
            API.SetTextColour(r, g, b, a);
            API.SetTextDropShadow();
            API.SetTextOutline();
            API.SetTextEntry("STRING");
            API.SetTextCentre(false);
            API.AddTextComponentString(text);
            API.DrawText(display_x, display_y);
        }

        public static void DrawRectangle(float display_x, float display_y, float width, float height, int r, int g, int b, int alpha)
        {
            float render_x = display_x + (width / 2);
            float render_y = display_y + (height / 2);
            API.DrawRect(render_x, render_y, width, height, r, g, b, alpha);
        }

        public static void DrawRectangleWithText(string text, float display_x, float display_y, float width, float height, int r, int g, int b, int alpha, bool blink)
        {
            if (blink)
            {
                alpha = alpha = Convert.ToInt32(Math.Abs(Math.Cos(DateTime.Now.Millisecond / 500) * 100000) % 255);
            }
            DrawRectangle(display_x, display_y, width, height, r, g, b, alpha);
            float pos = width / 2;
            DrawTextCenter(text, 4, 0.55f, display_x + pos, display_y, 255, 255, 255, alpha);
        }
        public static void DrawLifeBar(string text, float percent, float display_x, float display_y, float width, float height, int r, int g, int b, int alpha, bool blink)
        {
            //Hintergrund:
            DrawRectangle(display_x, display_y, width, height, r / 2, g / 2, b / 2, alpha);
            //Vordergund
            float new_heigh = height;
            float new_width = width * percent;
            if (blink)
            {
                alpha = alpha = Convert.ToInt32(Math.Abs(Math.Cos(DateTime.Now.Millisecond / 500) * 100000) % 255);
            }
            DrawRectangle(display_x, display_y, new_width, new_heigh, r, g, b, alpha);
            float pos = width / 2;
            DrawTextCenter(text, 4, 0.398f, display_x + pos, display_y - 0.005f, 255, 255, 255, alpha);
        }
        public static void DrawLifeBarWithAutoColor(string text, float percent, float display_x, float display_y, float width, float height, int alpha)
        {
            if (percent > 0.70)
            {
                DrawLifeBar(text, percent, display_x, display_y, width, height, 0, 170, 20, alpha, false);
            }
            else
            {
                if (percent > 0.35)
                {
                    DrawLifeBar(text, percent, display_x, display_y, width, height, 255, 200, 0, alpha, false);
                }
                else
                {
                    if (percent > 0.20)
                    {
                        DrawLifeBar(text, percent, display_x, display_y, width, height, 255, 20, 20, alpha, false);
                    }
                    else
                    {
                        DrawLifeBar(text, percent, display_x, display_y, width, height, 255, 20, 20, alpha, true);
                    }
                }
            }
        }

        public static void DrawHudElement(string hud_elem, float pos_x, float pos_y, float width, float height)
        {
            if (!API.HasStreamedTextureDictLoaded("rgc_hud"))
            {
                API.RequestStreamedTextureDict("rgc_hud", true);
            }
            else
            {

                API.DrawSprite("rgc_hud", hud_elem, pos_x, pos_y, width, height, 0f, 255, 255, 255, 200);
            }
        }

        public static void DrawHudElement(string hud_elem, float pos_x, float pos_y, float size,
            int r = 255, int g = 255, int b = 255, int alpha = 200, bool blink = false)
        {
            int display_x = 0;
            int display_y = 0;
            API.GetActiveScreenResolution(ref display_x, ref display_y);
            float scale = display_y / (float)display_x;
            float height = size;
            float width = size * scale;

            if (blink)
            {
                alpha = alpha = Convert.ToInt32(Math.Abs(Math.Cos(DateTime.Now.Millisecond / 500) * 100000) % 255);
            }
            if (!API.HasStreamedTextureDictLoaded("rgc_hud"))
            {
                API.RequestStreamedTextureDict("rgc_hud", true);
            }
            else
            {
                API.DrawSprite("rgc_hud", hud_elem, pos_x, pos_y, width, height, 0f, r, g, b, alpha);
            }
        }
    }
}