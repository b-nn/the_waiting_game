using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using BreakInfinity;

namespace Global
{
    /// <summary>
    /// 
    /// </summary>
    public static class G {
        public static Dictionary<string, Color> colors = new Dictionary<string, Color>{
        {"bg_1",new Color(19, 21, 21)},
        {"bg_2",new Color(43, 44, 40)},
        {"fg_1",new Color(51, 153, 137)},
        {"fg_2",new Color(125, 226, 209)},
        {"accent",new Color(255, 250, 251)}};
        public enum conversions {
            second = 1,
            minute = 60,
            hour = 360,
            day = 86400,
            year = 31556926
        }
        public static SpriteFont font;
        public static List<Texture2D> button_textures = new List<Texture2D>(); // 0 is corner 1 is edge 2 is center
        public static Texture2D button_texture;
        public static Texture2D button_off_texture;
        public static Texture2D mouse_texture;
        public static Texture2D blip_texture;
        public static MouseState mouse;
        public static Vector2 mouse_position;
        public static Vector2 ratios;
        public static bool mouse_pressed;
        public static bool mouse_clicked;
        public static string format(BigDouble value, int type) {
            switch(type){
                case 0:
                if(value < 60) {
                    return($"{Math.Round(value.ToDouble(),1)}s");
                }
                else if (value < 3600) {
                    return($"{Math.Round(value.ToDouble()/60,1)}m");
                }
                else if (value < 86400) {
                    return($"{Math.Round(value.ToDouble()/3600,2)}h");
                }
                else if (value < 31556926) {
                    return($"{Math.Round(value.ToDouble()/86400,2)}d");
                }
                else {
                    value = value / 31556926;
                    return($"{value.ToString("G2").ToLowerInvariant()}y");
                }
                case 1:
                    return($"{value.ToString("G2").ToLowerInvariant()}");
            }
            return("PLEASE DEFINE TYPE");
        }
        public static void draw_button(Vector2 position, int width, int height, SpriteBatch spriteBatch) {
            spriteBatch.Draw(button_textures[0], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[0], new Vector2(position.X + width, position.Y), null, Color.White, (float)Math.PI/2, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[0], new Vector2(position.X, position.Y + height), null, Color.White, -(float)Math.PI/2, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[0], new Vector2(position.X + width, position.Y + height), null, Color.White, (float)Math.PI, new Vector2(0,0), 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(button_textures[1], new Vector2(position.X + 5, position.Y), null, Color.White, 0f, Vector2.Zero, new Vector2(width - 10, 1), SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[1], new Vector2(position.X + width - 5, position.Y + height), null, Color.White, (float)Math.PI, Vector2.Zero, new Vector2(width - 10, 1), SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[1], new Vector2(position.X, position.Y + height - 5), null, Color.White, -(float)Math.PI/2, Vector2.Zero, new Vector2(height - 10, 1), SpriteEffects.None, 0f);
            spriteBatch.Draw(button_textures[1], new Vector2(position.X + width, position.Y + 5), null, Color.White, (float)Math.PI/2, Vector2.Zero, new Vector2(height - 10, 1), SpriteEffects.None, 0f);

            spriteBatch.Draw(button_textures[2], new Vector2(position.X + 5, position.Y + 5), null, Color.White, 0f, Vector2.Zero, new Vector2(width - 10, height - 10), SpriteEffects.None, 0f);
        }
        public static double smooth_lerp(double t, int max)
        {
            return(max * t * t * (3 - 2 * t));
        }
    }
}