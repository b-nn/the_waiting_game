using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Global;
using Player;
using System.Collections.Generic;

namespace slider_class
{ 
    /// <summary>
    /// 
    /// </summary>
    public class slider {
        public int type {get; set;}
        public Vector2 position {get; set;}
        public double blip_position = 82;
        public string text;
        public bool button_held;
        public double offset;
        public double value;
        public double max_value;
        public double min_value;
        public slider(int Type, Vector2 Position, string Text, int MinValue, int MaxValue)
        {
            type = Type;
            position = Position;
            text = Text;
            max_value = MaxValue;
            min_value = MinValue;
            value = MaxValue;
        }

        public void draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(G.button_texture, position, Color.White);
            spriteBatch.Draw(G.blip_texture, position + new Vector2((float)Math.Round(blip_position), 2), Color.White);
            spriteBatch.DrawString(G.font, text + value, position + new Vector2(G.button_texture.Width + 2, 2), G.colors["fg_1"]);
        }
        public void collision() {
            value = min_value + ((blip_position-2)/80) * (max_value - min_value);

            if(button_held) {
                click();
            }
            if(G.mouse_position.X < blip_position + position.X|| G.mouse_position.X > blip_position + position.X + G.blip_texture.Width) {
                return;
            }
            if(G.mouse_position.Y < position.Y+2 || G.mouse_position.Y > position.Y+2 + G.blip_texture.Height) {
                return;
            }
            if(G.mouse_clicked) {
                button_held = true;
                offset = G.mouse_position.X - blip_position;
            }
        }
        public void click() {
            if(!G.mouse_pressed) {
                button_held = false;
            }
            blip_position = G.mouse_position.X - offset;
            if(blip_position < 2) {
                blip_position = 2;
            }
            if(blip_position > 82) {
                blip_position = 82;
            }
        }
    }
}
