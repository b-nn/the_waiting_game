using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Global;
using Player;
using System.Collections.Generic;
using the_waiting_game;
using BreakInfinity;

namespace Button
{
    /// <summary>
    /// 
    /// </summary>
    public class button {
        public string currency;
        public BigDouble price {get; set;}
        public double price_multiplier {get; set;}
        public int type {get; set;}
        public string text {get; set;}
        public Vector2 position {get; set;}
        public Vector2 text_position;
        public Vector2 price_position;
        public Texture2D texture;
        public Texture2D texture_off;
        public bool price_side;
        public double base_price;
        public int cap;
        public int bought_amounts = 0;
        public button(double Price, double Price_multiplier, int Type, string Text, Vector2 Position, bool Price_side, string Currency, int Cap)
        {
            price = Price;
            base_price = Price;
            price_multiplier = Price_multiplier;
            type = Type;
            text = Text;
            position = Position;
            texture = G.button_texture;
            texture_off = G.button_off_texture;
            text_position = new Vector2(Position.X+4,Position.Y+3);
            price_side = Price_side;
            currency = Currency;
            cap = Cap;
            if(Price_side) {
                price_position = new Vector2(Position.X + 2 + texture.Width, Position.Y+3);
            }
        }

        public void draw(SpriteBatch spriteBatch) {
            if(!price_side) {
                price_position = new Vector2(position.X - G.font.MeasureString(G.format(price,0)).X - 2, position.Y+3);
            }

            switch(currency) {
                case "time":
                    if(bought_amounts >= cap || price > player.Time) {
                        spriteBatch.Draw(texture_off, position, Color.White);
                    }
                    else{
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                    break;
                case "flips":
                    if(bought_amounts >= cap || price > player.Flips) {
                        spriteBatch.Draw(texture_off, position, Color.White);
                    }
                    else{
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                    break;
            }
            if(bought_amounts >= cap) {
                spriteBatch.DrawString(G.font, "capped", price_position, G.colors["fg_2"]);
            }
            else {
                spriteBatch.DrawString(G.font, G.format(price,0), price_position, G.colors["fg_2"]);
            }

            switch(type)
            {
                case 0: // +1 base
                    spriteBatch.DrawString(G.font, text + $" ({G.format(player.BaseTimePerSeconds,0)})", text_position, G.colors["fg_1"]);
                    break;
                case 1: // x2 total
                    double i = 1;
                    foreach(double j in player.Multipliers) {
                        i *= j;
                    }
                    spriteBatch.DrawString(G.font, text + $" ({Math.Round(i,1)})", text_position, G.colors["fg_1"]);
                    break;
                case 2: // .generator
                    spriteBatch.DrawString(G.font, text + $" ({Math.Round(player.Generators[0],1)})", text_position, G.colors["fg_1"]);
                    break;
                case 3: // :generator
                    spriteBatch.DrawString(G.font, text + $" ({Math.Round(player.Generators[1],1)})", text_position, G.colors["fg_1"]);
                    break;
                case 4: // :.generator
                    spriteBatch.DrawString(G.font, text + $" ({Math.Round(player.Generators[2],1)})", text_position, G.colors["fg_1"]);
                    break;
                case 5: // ::generator
                    spriteBatch.DrawString(G.font, text + $" ({Math.Round(player.Generators[3],1)})", text_position, G.colors["fg_1"]);
                    break;
                case 6: // prestige
                    spriteBatch.DrawString(G.font, text + $"{G.format(BigDouble.Log((player.Time/(int)G.conversions.year),5) / (Math.Pow(2,player.Scale)),1)})", text_position, G.colors["fg_1"]);
                    break;
                case 7: // prestige upgrade 1
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[0]}", text_position, G.colors["fg_1"]);
                    break;
                case 8: // prestige upgrade 2
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[1]}", text_position, G.colors["fg_1"]);
                    break;
                case 9: // prestige upgrade 3
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[2]}", text_position, G.colors["fg_1"]);
                    break;
                case 10: // prestige upgrade 4
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[3]}", text_position, G.colors["fg_1"]);
                    break;
                case 11: // prestige upgrade 5
                    spriteBatch.DrawString(G.font, text, text_position, G.colors["fg_1"]);
                    break;
                case 12: // prestige upgrade 6
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[5]}", text_position, G.colors["fg_1"]);
                    break;
                case 13: // delton upgrade 1
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[6]}", text_position, G.colors["fg_1"]);
                    break;
                case 14: // delton upgrade 2
                    spriteBatch.DrawString(G.font, text + $"{player.FlipUpgrades[7]}", text_position, G.colors["fg_1"]);
                    break;
            }
        }
        public void collision() {
            if(G.mouse_position.X < position.X || G.mouse_position.X > position.X + texture.Width) {
                return;
            }
            if(G.mouse_position.Y < position.Y || G.mouse_position.Y > position.Y + texture.Height) {
                return;
            }
            if(G.mouse_clicked) {
                buy();
            }
        }
        public void buy() {
            if(bought_amounts >= cap) {
                return;
            }

            switch(currency) {
                case "time":
                    if(price > player.Time) {
                        return;
                    }
                    if(!player.FlipUpgrades[5]) {
                        player.Time -= price;
                    }
                    break;
                case "flips":
                    if(price > player.Flips) {
                        return;
                    }
                    player.Flips -= price;
                    break;
            }
            price *= price_multiplier;
            bought_amounts += 1;

            switch(type)
            {
                case 0: // +1 base
                    player.BaseTimePerSeconds += 1;
                    return;
                case 1: // x2 total
                    player.Multipliers.Add(1.7);
                    return;
                case 2: // .generator
                    player.Generators[0] += 1;
                    player.GeneratorEfficiencies[0] = 1-((1-player.GeneratorEfficiencies[0])/1.05);
                    return;
                case 3: // :generator
                    player.Generators[1] += 1;
                    player.GeneratorEfficiencies[1] = 1-((1-player.GeneratorEfficiencies[1])/1.05);
                    return;
                case 4: // :.generator
                    player.Generators[2] += 1;
                    player.GeneratorEfficiencies[2] = 1-((1-player.GeneratorEfficiencies[2])/1.05);
                    return;
                case 5: // ::generator
                    player.Generators[3] += 1;
                    player.GeneratorEfficiencies[3] = 1-((1-player.GeneratorEfficiencies[3])/1.05);
                    return;
                case 6: // prestige
                    player.Time += price;
                    player.Flips = BigDouble.Log((player.Time/(int)G.conversions.year),5) / (Math.Pow(2,player.Scale));
                    Game1.prestige();
                    return;
                case 7: // prestige upgrade 1
                    player.FlipUpgrades[0] = true;
                    return;
                case 8: // prestige upgrade 2
                    player.FlipUpgrades[1] = true;
                    return;
                case 9: // prestige upgrade 3
                    player.FlipUpgrades[2] = true;
                    player.BaseTime = (int)G.conversions.year * 5;
                    return;
                case 10: // prestige upgrade 4
                    player.FlipUpgrades[3] = true;
                    return;
                case 11: // prestige upgrade 5
                    player.Deltons += 1;
                    return;
                case 12: // prestige upgrade 6
                    player.FlipUpgrades[5] = true;
                    return;
                case 13: // delton upgrade 1
                    player.FlipUpgrades[6] = true;
                    return;
                case 14: // delton upgrade 2
                    player.FlipUpgrades[7] = true;
                    return;
            }
        }
    }
}
