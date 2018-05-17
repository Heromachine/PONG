using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

public enum GameState { SPLASH, PLAYGAME, MENU, HELP, PAUSE, WINNER }


namespace PONG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    //==========================================================================================================================================
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState gGameState;

        
        //-----SPLASH SCREEN DATA-----//
        Texture2D splashImage;
        Rectangle splashRect;// to identify the size of the picture and its position. 
        //SOUND
        Song splashSong;
        string Zero = "Zero";
        string One = "One";
        string Two = "Two";
        string Three = "Three";
        string Four = "Four";
        string Five = "Five";
        string Six = "Six";
        string Seven = "Seven";
        string Eight = "Eight";
        string Nine = "Nine";
        
        string sWinner;

        int ScoreP1;
        int ScoreP2;
        int ScoreToWin;

        int iBallSize = 20;
        int iPadSizeP1 = 130;
        int iPadSizeP2 = 130;

        static int iPadPosP1 = 250;
        static int iPadPosP2 = 250;

        
        //-----PLAY GAME DATA-----//
        Rectangle ScoreRect1;
        Rectangle ScoreRect2;
        Rectangle BallRect;
        Rectangle PaddleRectP1;
        Rectangle PaddleRectP2;
        Rectangle gameBgRect;

        Texture2D gameScore1;
        Texture2D gameScore2;
        Texture2D gameBgImage;
        Texture2D gameBall;
        Texture2D gamePaddle1;
        Texture2D gamePaddle2;
        
        Song gameSong;

        SpriteFont font;


        Texture2D ballImage;
        Rectangle ballRect;//the position and size of the ball.
        Rectangle ballVelocity;//the speed of the ball.
        Random         rRand;
        KeyboardState oldkbs;

        //score rectangle
        //==========================================================================================================================================
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        //==========================================================================================================================================
        /// <summary>
        /// Allows the game to perform any initialization it needs to before startping to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        /// 
        
        protected override void Initialize()
        {
         
            ScoreToWin = 10;
            ScoreP1 = 0;
            ScoreP2 = 0;
            // TODO: Add your initialization logic here
            rRand = new Random();
             //-----SPLASH SCREEN DATA-----//
            splashRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
             //-----PLAY GAME DATA-----//
            gameBgRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //BALLRECT
            BallRect = new Rectangle(0, 0, iBallSize, iBallSize);
            PaddleRectP1 = new Rectangle(20, iPadPosP1, 25, iPadSizeP1);
            PaddleRectP2 = new Rectangle(750, iPadPosP2, 25, iPadSizeP2);

            ScoreRect1 = new Rectangle(300, 10, 65, 85);
            ScoreRect2 = new Rectangle(425, 10, 65, 85);
            
             
            //START OF GAME INITALIZE//
            gGameState = GameState.SPLASH;

            //INIT BALL
            ballImage = new Texture2D(GraphicsDevice, 1, 1);
            ballImage.SetData(new Color[] { Color.Red });

            ballRect = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, 20, 20);

            ballVelocity = new Rectangle(0, 0, 0, 0);
            ballVelocity.X = rRand.Next(1, 11);
            ballVelocity.Y = rRand.Next(1, 11);
            
            //====================================================
            base.Initialize();
        }
        //==========================================================================================================================================
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("SpriteFont1");

            // TODO: use this.Content to load your game content here

            //---splash screen content load-----

            splashImage = Content.Load<Texture2D>("PongPaintSplash");
            splashSong  = Content.Load<Song>("VideoGame1");
            


            //----game screen content load ------
            gameBgImage = Content.Load<Texture2D>("GameBackground");
            gameBall    = Content.Load<Texture2D>("Ball");
            gamePaddle1 = Content.Load<Texture2D>("P1Paddle");
            gamePaddle2 = Content.Load<Texture2D>("P2Paddle");
            gameScore1 = Content.Load<Texture2D>("Zero");
            gameScore2 = Content.Load<Texture2D>("Zero");
            
            gameSong    = Content.Load<Song>("GameSongSad");


        }
        //==========================================================================================================================================
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        /// 
        
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
            
            //-----KEY BOARD AND GAME PAD UPDATE------//
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kbs;
            kbs = Keyboard.GetState(); //grabs the current state of keyboard. 
            if (kbs.IsKeyDown(Keys.Escape) == true)
                this.Exit();


          

            //----Splash screen update----//
            if (gGameState == GameState.SPLASH)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(splashSong);
                }
                //PRESS ENTER TO STOP GAME AND END SPLASH
                if (kbs.IsKeyDown(Keys.Enter) == true)
                {
                    MediaPlayer.Stop();
                    gGameState = GameState.PLAYGAME;
                }
                //PRESS X TO STOP GAME AND END SPLASH
                if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed)
                {   //STOP MUSIC
                    MediaPlayer.Stop();
                    gGameState = GameState.PLAYGAME;
                }
            
            }
            
           


            //----------IN GAME PLAY-----------//==================================================================================            
            //START MUSIC FOR GAMEPLAY
            if (gGameState == GameState.PLAYGAME)
            {
                //CONTROLS
                if (kbs.IsKeyDown(Keys.R) && oldkbs.IsKeyUp(Keys.R))
                {
                    ballRect.X = GraphicsDevice.Viewport.Width / 2;
                    ballRect.Y = GraphicsDevice.Viewport.Height / 2;

                    ballVelocity.X = rRand.Next(-4, 4);
                    ballVelocity.Y = rRand.Next(-4, 4);
                }
                if (kbs.IsKeyDown(Keys.W) == true && PaddleRectP1.Y > 0)
                    PaddleRectP1.Y -= 5;
                if (kbs.IsKeyDown(Keys.S) == true && PaddleRectP1.Y < 475)
                    PaddleRectP1.Y += 5;

                if (kbs.IsKeyDown(Keys.I) == true && PaddleRectP2.Y > 0)
                    PaddleRectP2.Y -= 5;
                if (kbs.IsKeyDown(Keys.K) == true && PaddleRectP2.Y < 475)
                    PaddleRectP2.Y += 5;
               
                BallRect.X = ballRect.X;
                BallRect.Y = ballRect.Y;

                
                //PLAY AUDIO
                if (ScoreP1 == 0)
                    gameScore1 = Content.Load<Texture2D>(Zero);
                if (ScoreP1 == 1)
                    gameScore1 = Content.Load<Texture2D>(One);
                if (ScoreP1 == 2)
                    gameScore1 = Content.Load<Texture2D>(Two);
                if (ScoreP1 == 3)
                    gameScore1 = Content.Load<Texture2D>(Three);
                if (ScoreP1 == 4)
                    gameScore1 = Content.Load<Texture2D>(Four);
                if (ScoreP1 == 5)
                    gameScore1 = Content.Load<Texture2D>(Five);
                if (ScoreP1 == 6)
                    gameScore1 = Content.Load<Texture2D>(Six);
                if (ScoreP1 == 7)
                    gameScore1 = Content.Load<Texture2D>(Seven);
                if (ScoreP1 == 8)
                    gameScore1 = Content.Load<Texture2D>(Eight);
                if (ScoreP1 == 9)
                    gameScore1 = Content.Load<Texture2D>(Nine);

                if (ScoreP1 == 0)
                    gameScore2 = Content.Load<Texture2D>(Zero);
                if (ScoreP2 == 1)
                    gameScore2 = Content.Load<Texture2D>(One);
                if (ScoreP2 == 2)
                    gameScore2 = Content.Load<Texture2D>(Two);
                if (ScoreP2 == 3)
                    gameScore2 = Content.Load<Texture2D>(Three);
                if (ScoreP2 == 4)
                    gameScore2 = Content.Load<Texture2D>(Four);
                if (ScoreP2 == 5)
                    gameScore2 = Content.Load<Texture2D>(Five);
                if (ScoreP2 == 6)
                    gameScore2 = Content.Load<Texture2D>(Six);
                if (ScoreP2 == 7)
                    gameScore2 = Content.Load<Texture2D>(Seven);
                if (ScoreP2 == 8)
                    gameScore2 = Content.Load<Texture2D>(Eight);
                if (ScoreP2 == 9)
                    gameScore2 = Content.Load<Texture2D>(Nine);

                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(gameSong);
                }

                //BALL PYSICS
                ballRect.X = ballRect.X + ballVelocity.X;
                ballRect.Y = ballRect.Y + ballVelocity.Y;

                if (ballRect.Y > GraphicsDevice.Viewport.Height - ballRect.Width)
                {
                    ballVelocity.Y = -ballVelocity.Y;
                }
                if (ballRect.X > GraphicsDevice.Viewport.Width - ballRect.Width)
                {
                    ballVelocity.X = -ballVelocity.X;
                    ScoreP1++;
                    if (iPadSizeP1 > 20)
                    {
                        iPadSizeP1 -= 10;
                        iPadPosP1 = PaddleRectP1.Y;
                        PaddleRectP1 = new Rectangle(20, iPadPosP1, 25, iPadSizeP1);
                    }
                }
                if (ballRect.Y < 0)
                {
                    ballVelocity.Y = -ballVelocity.Y;
                }
                if (ballRect.X < 0)
                {
                    ballVelocity.X = -ballVelocity.X;
                    ScoreP2++;
                    if (iPadSizeP2 > 20)
                    {
                        
                        iPadSizeP2 -= 10;
                        iPadPosP2 = PaddleRectP2.Y;
                        PaddleRectP2 = new Rectangle(750, iPadPosP2, 25, iPadSizeP2);
                    }
                }
                if (ballRect.X < PaddleRectP1.X + 20 && ballRect.Y > PaddleRectP1.Y &&
                    ballRect.X > PaddleRectP1.X && ballRect.Y < PaddleRectP1.Y + 130)
                {
                    ballVelocity.X = -ballVelocity.X;
                    
                    
                }

                if (ballRect.X < PaddleRectP2.X + 20 && ballRect.Y > PaddleRectP2.Y &&
                    ballRect.X > PaddleRectP2.X && ballRect.Y < PaddleRectP2.Y + 130)
                {
                    ballVelocity.X = -ballVelocity.X;
                   
                }

                //WINNER
                if (ScoreP1 >= ScoreToWin || ScoreP2 >= ScoreToWin)
                {
                    if (ScoreP1 >= ScoreToWin)                    
                        sWinner = "PLAYER 1 WON THE GAME!!!";                   
                    else
                        sWinner = "Player 2 WON THE GAME!!!";

                    MediaPlayer.Stop();
                    gGameState = GameState.WINNER;

                }
                //PAUSE
                if (kbs.IsKeyDown(Keys.P) && oldkbs.IsKeyUp(Keys.P))
                {
                    gGameState = GameState.PAUSE;
                }
                oldkbs = kbs;
            }
            //===========================================================================================================
            if (gGameState == GameState.WINNER)
            {
                if (kbs.IsKeyDown(Keys.Enter) && oldkbs.IsKeyUp(Keys.Enter))
                {
                    ScoreP1 = ScoreP2 = 0;
                    //ballVelocity.X = ballVelocity.Y = rRand.Next(-4, 4);
                    

                    ballRect.X = GraphicsDevice.Viewport.Width / 2;
                    ballRect.Y = GraphicsDevice.Viewport.Height / 2;

                    ballVelocity.X = rRand.Next(-4, 4);
                    ballVelocity.Y = rRand.Next(-4, 4);

                    gGameState = GameState.PLAYGAME;
                }
                oldkbs = kbs;
            }
            //===========================================================================================================
            if (gGameState == GameState.PAUSE )
            {
                if (kbs.IsKeyDown(Keys.P) && oldkbs.IsKeyUp(Keys.P))
                {
                    gGameState = GameState.PLAYGAME;
                }
                oldkbs = kbs;
            }
            //===========================================================================================================


            
            base.Update(gameTime);
        }
        //==========================================================================================================================================

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            //----GameStateSplash Drawing-----
            if (gGameState == GameState.SPLASH)
            {               
                spriteBatch.Draw(splashImage, splashRect, Color.White);
                spriteBatch.DrawString(font, "Press [Enter] To Begin", new Vector2(200, 550), Color.Red);
                
            }
            //----ENd of State Splash Drawing------


              //----GameState PLAYGAME Drawing-----
            if (gGameState == GameState.PLAYGAME)
            {                              
                spriteBatch.Draw(gameBgImage, gameBgRect, Color.White);
                spriteBatch.Draw(gameScore1, ScoreRect1, Color.White);
                spriteBatch.Draw(gameScore2, ScoreRect2, Color.White);
                //spriteBatch.Draw(ballImage, ballRect, Color.White);
                spriteBatch.Draw(gameBall, BallRect, Color.White);
                spriteBatch.Draw(gamePaddle1, PaddleRectP1, Color.White);
                spriteBatch.Draw(gamePaddle2, PaddleRectP2, Color.White);                
            }

            if (gGameState == GameState.WINNER)
            {
                spriteBatch.DrawString(font, sWinner, new Vector2(100, 200), Color.White); 
            }

            if (gGameState == GameState.PAUSE)
            {                
                spriteBatch.Draw(gameBgImage, gameBgRect, Color.White);
                spriteBatch.DrawString(font, "            PAUSE\n\n\n Press [H] To View Controls", new Vector2(200, 200), Color.Red);
                spriteBatch.Draw(gameScore1, ScoreRect1, Color.White);
                spriteBatch.Draw(gameScore2, ScoreRect2, Color.White);
                // spriteBatch.Draw(ballImage, ballRect, Color.White);
                spriteBatch.Draw(gameBall, BallRect, Color.White);
                spriteBatch.Draw(gamePaddle1, PaddleRectP1, Color.White);
                spriteBatch.Draw(gamePaddle2, PaddleRectP2, Color.White);
            }

            //----ENd of State PLAY GAME Drawing------           

            spriteBatch.End();

            base.Draw(gameTime);
        }
        //==========================================================================================================================================
    }
}
