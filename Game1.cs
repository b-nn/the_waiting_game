using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.IO;
using Button;
using Global;
using Player;
using System.Linq;
using Menu_button;
using slider_class;
using BreakInfinity;

namespace the_waiting_game;

public class Game1 : Game // test
{
    public static string scene = "time";
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    RenderTarget2D target;
    public static List<button> time_buttons;
    public static List<button> flip_buttons;
    public static List<button> delton_buttons;
    public static List<menu_button> menu_buttons;
    public double menu_switch_delay;
    public double menu_scroll_delay;
    public double menu_scroll_position;
    public slider Slider;
    public static void prestige() {
        player.WaitCurrency = 0;
        player.Time = player.BaseTime;
        player.BaseTimePerSeconds = 1;
        player.Multipliers = new List<double>();
        player.Generators = new double[] {0, 0, 0, 0};
        player.GeneratorEfficiencies = new double[] {0, 0, 0, 0};
        player.TimeSinceFlip = 0;
        foreach(button button in time_buttons) {
            button.price = button.base_price;
            button.bought_amounts = 0;
        }
    }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        target = new RenderTarget2D(GraphicsDevice,320,180);

        IsMouseVisible = false;
        G.mouse_texture = Content.Load<Texture2D>("cursor");
        G.button_texture = Content.Load<Texture2D>("button");
        G.button_off_texture = Content.Load<Texture2D>("button_inactive");
        G.blip_texture = Content.Load<Texture2D>("slider_blip");
        G.button_textures.Add(Content.Load<Texture2D>("corner_button"));
        G.button_textures.Add(Content.Load<Texture2D>("edge_button"));
        G.button_textures.Add(Content.Load<Texture2D>("center_button"));

        TtfFontBakerResult fontBakeResult;
        using (var stream = File.OpenRead("fonts/pixeldroidMenuRegular.ttf"))
			{
				// TODO: use this.Content to load your game content here
				fontBakeResult = TtfFontBaker.Bake(stream, 12, 1024, 1024, new [] {CharacterRange.BasicLatin});

				G.font = fontBakeResult.CreateSpriteFont(GraphicsDevice);
			}

        time_buttons = new List<button>() {
            new button(10,1.1,0,"+1 base",new Vector2(2,25), true, "time",100000),
            new button(60,10,1,"+70%",new Vector2(2,45), true, "time",15),
            new button(3600,7.5,2,".gen",new Vector2(222,25), false, "time",10000),
            new button(7200,15,3,":gen",new Vector2(222,45), false, "time",10000),
            new button(14400,30,4,":.gen",new Vector2(222,65), false, "time",10000),
            new button(28800,60,5,"::gen",new Vector2(222,85), false, "time",10000),
            new button(157784630,1,6,"flip(",new Vector2(2,65), true, "time",1000000000)
        };

        flip_buttons = new List<button>() {
            new button(157784630,1,6,"flip(",new Vector2(2,25), true, "time", 100000000),
            new button(7,1,7,"masstime",new Vector2(2,45), true, "flips", 1),
            new button(1,1,8,"baseaccel",new Vector2(2,65), true, "flips", 1),
            new button(1,1,9,"basestart",new Vector2(2,85), true, "flips", 1),
            new button(1,1,10,"autobuy",new Vector2(2,105), true, "flips", 1),
            new button(15,1,11,"unlockdelton",new Vector2(2,125), true, "flips", 1),
            new button(1,1,12,"freeprice",new Vector2(2,145), true, "flips", 1)
        };

        delton_buttons = new List<button>() {
            new button(3.1556926e+17,1,13,"timecap",new Vector2(2,25), true, "time", 1),
            new button(20,1,14,"flipcap",new Vector2(2,45), true, "flips", 1)
        };

        menu_buttons = new List<menu_button>() {
            new menu_button("time", 0),
            new menu_button("flips", 1),
            new menu_button("scale", 2),
            new menu_button("deltons", 3),
            new menu_button("options", 4)
        };
        
        Slider = new slider(0, new Vector2(2,22), "mouse size: ", 2, 10);

        base.Initialize();
        
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        double dt = gameTime.ElapsedGameTime.TotalSeconds;

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // PROCESSING

        if(player.Time < 0) {
            player.Time = 0;
        }
        
        player.TimeSinceFlip += dt;
        player.Time += player.TotalTimePerSeconds * dt;

        G.mouse = Mouse.GetState();
        G.ratios.X = _graphics.PreferredBackBufferWidth/(float)target.Width;
        G.ratios.Y = _graphics.PreferredBackBufferHeight/(float)target.Height;
        G.mouse_position.X = G.mouse.X / G.ratios.X;
        G.mouse_position.Y = G.mouse.Y / G.ratios.Y;

        player.TotalTimePerSeconds = player.BaseTimePerSeconds + player.WaitCurrency;
        foreach(float mutliplier in player.Multipliers) {
            player.TotalTimePerSeconds *= mutliplier;
        }
        player.TotalTimePerSeconds *= (BigDouble.Pow(3,player.Flips));
        player.TotalTimePerSeconds /= (BigDouble.Pow(5,player.Scale));

        for(int i = 0; i < player.FlipUpgrades.Length; i++){
            if(!player.FlipUpgrades[i]) {
                continue;
            }
            switch(i) {
                case 0:
                    if(BigDouble.IsNaN(BigDouble.Log10(player.Time / 31556926)) || BigDouble.Log10(player.Time / 31556926) < 0) {
                        continue;
                    }
                    player.TotalTimePerSeconds += player.TotalTimePerSeconds * BigDouble.Log10(player.Time / 31556926);
                    continue;
                case 1:
                    player.TotalTimePerSeconds *= (BigDouble.Pow(0.9,player.TimeSinceFlip) * 30) + 1;
                    continue;
                case 3:
                    foreach(button button in time_buttons) {
                        if(button.type > 0 && button.type < 6) {
                            button.buy();
                        }
                    }
                    continue;
                    
            }
        }

        player.DeltonCapModifier = 0;
        player.DeltonCapModifier += player.FlipUpgrades[6] ? BigDouble.Log2(player.Time) : 0;
        player.DeltonCapModifier += player.FlipUpgrades[7] ? player.Flips.ToDouble() : 0;

        if(player.Deltons < 100 + player.DeltonCapModifier) {
            player.Deltons += player.Deltons * dt;
        }
        else {
            player.Deltons += (player.Deltons / Math.Pow(1.05,player.Deltons-(100 + player.DeltonCapModifier))) * dt;
        }

        for(int i = 0; i < 4; i++) {
            if(i == 0) {
                player.WaitCurrency += player.Generators[i] * player.GeneratorEfficiencies[0] * Math.Log10(player.Deltons < 10 ? 10 : player.Deltons) * dt;
            }
            else {
                player.Generators[i-1] += player.Generators[i] * player.GeneratorEfficiencies[i] * Math.Log10(player.Deltons < 10 ? 10 : player.Deltons) * dt;
            }
        }

        if(G.mouse_position.Y < 20) {
            if(menu_switch_delay < 0.1) {
                menu_switch_delay += dt;
            }
            else {
                menu_switch_delay = 0.1;
            }
        }
        else {
            if(menu_switch_delay > 0) {
                menu_switch_delay -= dt;
            }
            else {
                menu_switch_delay = 0;
            }
        }

        foreach(menu_button button in menu_buttons) {
            button.collision(new Vector2(G.mouse.ScrollWheelValue/8, (float)G.smooth_lerp(menu_switch_delay*4,1)*20));
        }

        switch(scene) {
            case "time":
                foreach(button button in time_buttons) {
                    button.collision();
                }
                break;
            case "flips":
                foreach(button button in flip_buttons) {
                    button.collision();
                }
                break;
            case "deltons":
                foreach(button button in delton_buttons) {
                    button.collision();
                }
                break;

            case "options":
                Slider.collision();
                break;
        }
        

        if(G.mouse.LeftButton == ButtonState.Pressed) {
            G.mouse_clicked = G.mouse_pressed ? false : true;
            G.mouse_pressed = true;
        }
        else {
            G.mouse_pressed = false;
        }

        Console.WriteLine(Math.Log10(player.Deltons < 10 ? 10 : player.Deltons));

        // RENDERING

        GraphicsDevice.SetRenderTarget(target);
        GraphicsDevice.Clear(G.colors["bg_1"]);
        _spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

        switch(scene) {
            case "time":
                if(player.Time < 60) {
                    _spriteBatch.DrawString(G.font,$"you have {Math.Round(player.Time.ToDouble() * 10) / 10} seconds", new Vector2(2,0),G.colors["fg_2"]);
                }
                else {
                    _spriteBatch.DrawString(G.font,$"you have {G.format(player.Time,0)}", new Vector2(2,0),G.colors["fg_2"]);
                }
                _spriteBatch.DrawString(G.font,$"you're getting {G.format(player.TotalTimePerSeconds,0)}/s", new Vector2(2,10),G.colors["fg_1"]);

                foreach(button button in time_buttons) {
                    button.draw(_spriteBatch);
                }
                break;
            case "flips":
                _spriteBatch.DrawString(G.font,$"you have {G.format(player.Flips,1)} tflips", new Vector2(2,0),G.colors["fg_2"]);
                if(BigDouble.Log((player.Time/(int)G.conversions.year),5) / Math.Pow(2,player.Scale) - player.Flips > 0) {
                    _spriteBatch.DrawString(G.font,$"if you flip you'll gain {BigDouble.Round((BigDouble.Log((player.Time/(int)G.conversions.year),5) / Math.Pow(2,player.Scale) - player.Flips) * 100) / 100} tflips", new Vector2(2,10),G.colors["fg_1"]);
                }
                else {
                    _spriteBatch.DrawString(G.font,$"if you flip you'll lose {BigDouble.Round((player.Flips - BigDouble.Log((player.Time/(int)G.conversions.year),5) / Math.Pow(2,player.Scale)) * 100) / 100} tflips", new Vector2(2,10),G.colors["fg_1"]);
                }

                foreach(button button in flip_buttons) {
                    button.draw(_spriteBatch);
                }
                break;
            case "scale":
                break;
            case "deltons":
                _spriteBatch.DrawString(G.font,$"you have {player.Deltons} deltons", new Vector2(2,0),G.colors["fg_2"]);
                _spriteBatch.DrawString(G.font,$"multiplying generators by {Math.Log10(player.Deltons < 10 ? 10 : player.Deltons)}", new Vector2(2,10),G.colors["fg_1"]);

                foreach(button button in delton_buttons) {
                    button.draw(_spriteBatch);
                }
                break;
            case "options":
                Slider.draw(_spriteBatch);
                _spriteBatch.DrawString(G.font, "options", new Vector2(2,0), G.colors["fg_2"]);
                break;
        }

        foreach(menu_button button in menu_buttons) {
            button.draw(_spriteBatch, new Vector2(G.mouse.ScrollWheelValue/8, (float)G.smooth_lerp(menu_switch_delay*10,1)*20));
        }

        _spriteBatch.End();

        upscale();

        base.Update(gameTime);
    }
    public void upscale() {
        GraphicsDevice.SetRenderTarget(null);

        _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

        _spriteBatch.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);
        _spriteBatch.Draw(target, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        _spriteBatch.Draw(G.mouse_texture,G.mouse.Position.ToVector2(),null,G.colors["accent"],0f,Vector2.Zero,(float)Slider.value,SpriteEffects.None,0f);
        _spriteBatch.End();
    }

    protected override void Draw(GameTime gameTime)
    {
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
