using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Global;
using Player;
using System.Collections.Generic;
using the_waiting_game;

namespace Menu_button
{ 
    /// <summary>
    /// 
    /// </summary>
    public class menu_button {
        public Vector2 position {get; set;}
        public string text;
        int index;
        Vector2 text_size = new Vector2();
        public menu_button(string Text, int Index)
        {
            position = new Vector2(2, -18);
            text = Text;
            index = Index;
            text_size = G.font.MeasureString(text);
        }

        public void draw(SpriteBatch spriteBatch, Vector2 offset) {
            if(index == 0) {
                position = new Vector2(2, -18);
            }
            else {
                position = new Vector2(Game1.menu_buttons[index-1].position.X + 11 + G.font.MeasureString(Game1.menu_buttons[index - 1].text).X, Game1.menu_buttons[index-1].position.Y);
            }
            G.draw_button(position + offset, (int)text_size.X + 9, (int)text_size.Y + 6, spriteBatch);
            spriteBatch.DrawString(G.font, text, position + offset + new Vector2(4,3), G.colors["fg_1"]);
        }
        public void collision(Vector2 offset) {
            if(G.mouse_position.X < position.X + offset.X|| G.mouse_position.X > position.X + (int)text_size.X + 9 + offset.X) {
                return;
            }
            if(G.mouse_position.Y < position.Y + 20 || G.mouse_position.Y > position.Y + (int)text_size.Y + 6 + 20) {
                return;
            }
            if(G.mouse_clicked) {
                click();
            }
        }
        public void click() {
            Game1.scene = text;
        }
    }
}
