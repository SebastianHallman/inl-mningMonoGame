using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;


namespace EgnaSpel
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // variabler
        Texture2D player;
        Vector2 player_pos;
        Texture2D platform;
        int windowWidth;
        int windowHeight;
        int gravity = 5;
        int player_speed = 7;
        bool hit;
        bool grounded = false;
        bool jumped = true;
        int jump_force = 10;
        int jump_counter = 0;
        List<Vector2> platforms_pos = new List<Vector2>();
        List<Rectangle> platform_hitbox = new List<Rectangle>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            player_pos.X = 200;
            player_pos.Y = 100;
           
            windowWidth = Window.ClientBounds.X;
            windowHeight = Window.ClientBounds.Y;
            for (int i = 1; i<3; i++)
            {
                if (i==0)
                {

                    Vector2 pos = new Vector2(0, 452);
                    platforms_pos.Add(pos);
                }
                else
                {
                    Vector2 pos = new Vector2(i * 128, 452);
                    platforms_pos.Add(pos);
                }
                
                
            }
            Vector2 testplatform = new Vector2(400, 352);
            platforms_pos.Add(testplatform);
            Vector2 testplatform2 = new Vector2(300, 252);
            platforms_pos.Add(testplatform2);
            foreach (Vector2 plat in platforms_pos)
            {
                Rectangle hitbox = new Rectangle();
                hitbox.X = Convert.ToInt32(plat.X);
                hitbox.Y = Convert.ToInt32(plat.Y);
                hitbox.Width = 128;
                hitbox.Height = 32;
                platform_hitbox.Add(hitbox);
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = Content.Load<Texture2D>("Sprites/player");
            platform = Content.Load<Texture2D>("Sprites/platform");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // gravitation
            if (grounded == false || jumped == true)
            {
                player_pos.Y += gravity;
                jump_counter--;
            }
            // Kollar om spelaren är på en plattform
            foreach (Rectangle pf in platform_hitbox)
            {
                if (player_pos.Y < pf.Y-33 && player_pos.Y > pf.Y-20 && player_pos.X >= pf.X - 32 && player_pos.X <= pf.X + 128)
                {
                    player_pos.Y = pf.Y - 32;
                }

                if (player_pos.Y <= pf.Y+35 && player_pos.Y >= pf.Y + 27 && player_pos.X >= pf.X - 32 && player_pos.X <= pf.X + 128)
                {
                    grounded = true;
                }

                if (player_pos.Y == pf.Y - 32 && player_pos.X >= pf.X - 32 && player_pos.X <= pf.X + 128)
                {
                    grounded = true;
                    jumped = false;
                    jump_counter = 20;
                    break;
                }
                else
                {
                    grounded = false;
                    jumped = true;
                }
            }
            // Förflyttning
            KeyboardState Key = Keyboard.GetState();

            if (Key.IsKeyDown(Keys.Left))
            {
                player_pos.X -= player_speed;
            }
            if (Key.IsKeyDown(Keys.Right))
            {
                player_pos.X += player_speed;
            }
            if (Key.IsKeyDown(Keys.Space))
            {
                if (jump_counter > 0)
                {
                    player_pos.Y -= jump_force;
                    
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public bool CheckCollision(Rectangle player, Rectangle Object)
        {
            return player.Intersects(Object);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(player, player_pos, Color.White);
            // rita ut plattformarna
            foreach (Vector2 plat in platforms_pos)
            {
                spriteBatch.Draw(platform, plat, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
