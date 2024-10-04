using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Semester_Project_TeamBee
{
    public class Game1 : Game
    {
        //graphics and spritebatch created
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //fonts created
        private SpriteFont standardFont;
        private SpriteFont secondaryFont;
        //private SpriteFont cuteFont;
        private SpriteFont numberFont;

        //enum and keyboard state variables
        private GameState gameState = GameState.Menu;
        private KeyboardState previousKBState;
        private KeyboardState keyboardState;
        
        //platform list to hold all known platforms
        private List<Platform> platformList;

        //collectibles list to hold all known collectibles
        private List<Collectible> collectibles;

        //currentFrame to hold the current screen the leve is hold
        private int currentFrame;

        //textures
        private Texture2D leaf;
        private Texture2D endCollectibleTex;
        private Texture2D collectibleTex;
        private Texture2D controlsTexture;
        private Texture2D godModeTexture;
        private Texture2D godModeButtonTexture;
        private Texture2D pauseTexture;
        private Texture2D backgroundTexture;
        private Texture2D logo;


        //Player fields
        
        //bool for if god mode is on or not
        bool godMode;

        //player object
        private Player bert;

        //player texture
        private Texture2D beeTexture;
        
        //starting position
        private Rectangle startPossition = new Rectangle(0, 1520, 40, 40);

        //frame state variable 
        FrameState frameState;

        //level variables for each of the 9 levels
        private Level level1;
        private Level level2;
        private Level level3;
        private Level level4;
        private Level level5;
        private Level level6;
        private Level level7;
        private Level level8;
        private Level level9;

        //list of levels
        private List<Level> levelList;

        //variable for the level index
        private int levelIndexer;

        //current level variable
        private Level currentLevel;
        
        //previous frame variable
        private int previousFrame;

        //timer for falling leaves
        private double gameTimer;

        //texture for buttons
        private Texture2D buttonDesign;

        //button sizes
        const double ButtonRectHeight = 417;
        const double ButtonBaseWidth = 4504;
        const double ButtonSpacerWidth = 20;

        /// <summary>
        /// These are the enum states for the game that we will need for our eventual FSM
        /// </summary>
        enum GameState
        {
            Menu,
            Game,
            LevelChange,
            GameWon,
            Pause
        }

        /// <summary>
        /// Enum state for what the frame currently is
        /// </summary>
        enum FrameState
        {
            Frame0,
            Frame1,
            Frame2,
            Frame3,
            Frame4,
            Frame5,
            Frame6
        }

        /// <summary>
        /// Game1 constructor
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initialize method
        /// </summary>
        protected override void Initialize()
        {
            //platform list is initialized
            platformList = new List<Platform>();

            //screen size is initialized
            _graphics.PreferredBackBufferHeight = 1600;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.ApplyChanges();

            //godmode set to false initially
            godMode = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent method
        /// </summary>
        protected override void LoadContent()
        {
            //spritebatch initialized
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //fonts initialized
            standardFont = Content.Load<SpriteFont>("StandardFont");
            secondaryFont = Content.Load<SpriteFont>("secondaryFont");
            numberFont = Content.Load<SpriteFont>("NumberFont");
            //cuteFont = Content.Load<SpriteFont>("CuteFont");

            //textures loaded for leaf, collectibles, and player
            leaf = Content.Load<Texture2D>("LeafFixedSpriteSheet");
            collectibleTex = Content.Load<Texture2D>("flowerPNG");
            endCollectibleTex = Content.Load<Texture2D>("victoryPollen");
            beeTexture = Content.Load<Texture2D>("BertSpriteSheetFixed");

            //player initialized
            bert = new Player(beeTexture, startPossition, BertState.Walk);

            //individual button textures initialized + control menu texture initialized
            controlsTexture = Content.Load<Texture2D>("controlsMenu");
            godModeTexture = Content.Load<Texture2D>("GodModeButton");
            godModeButtonTexture = Content.Load<Texture2D>("TheGodModeButton");
            pauseTexture = Content.Load<Texture2D>("pauseButton");

            //background initialized
            backgroundTexture = Content.Load<Texture2D>("backgrounddarkerblue1");

            //logo initialized
            logo = Content.Load<Texture2D>("logo");

            //starting frame set to frame 0
            frameState = FrameState.Frame0;

            //A list of all levels for easier indexing
            levelList = new List<Level>();
            level1 = new Level("..\\..\\..\\level1.txt", leaf, collectibleTex, endCollectibleTex);
            level2 = new Level("..\\..\\..\\level2.txt", leaf, collectibleTex, endCollectibleTex);
            level3 = new Level("..\\..\\..\\level3.txt", leaf, collectibleTex, endCollectibleTex);
            level4 = new Level("..\\..\\..\\level4.txt", leaf, collectibleTex, endCollectibleTex);
            level5 = new Level("..\\..\\..\\level5.txt", leaf, collectibleTex, endCollectibleTex);
            level6 = new Level("..\\..\\..\\level6.txt", leaf, collectibleTex, endCollectibleTex);
            level7 = new Level("..\\..\\..\\level7.txt", leaf, collectibleTex, endCollectibleTex);
            level8 = new Level("..\\..\\..\\level8.txt", leaf, collectibleTex, endCollectibleTex);
            level9 = new Level("..\\..\\..\\level9.txt", leaf, collectibleTex, endCollectibleTex);

            //adding all of the levels to the list
            levelList.Add(level1);
            levelList.Add(level2);
            levelList.Add(level3);
            levelList.Add(level4);
            levelList.Add(level5);
            levelList.Add(level6);
            levelList.Add(level7);
            levelList.Add(level8);
            levelList.Add(level9);

            //Indexer for current level 0 = level 1
            levelIndexer = 0;
            currentLevel = levelList[levelIndexer];
            currentLevel.CollectibleSetUp();
            currentFrame = 0;
            collectibles = currentLevel.Collectibles;
            currentLevel.Update(currentFrame);
            platformList = currentLevel.PlatformList;
            gameState = GameState.Menu;

            //initialized the button spreadsheet 
            buttonDesign = Content.Load<Texture2D>("UISpritesheet");
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            //keyboard state gotten
            keyboardState = Keyboard.GetState();

            //player object calls update animation method
            bert.UpdateAnimation(gameTime);
            
            //each platform calls the update method to check if falling is occuring
            foreach (Platform platform in platformList)
            {
                platform.Update(platform.CheckFalling(gameTime, bert));
            }

            //try and catch to handle any potential errors in this section
            try
            {
                //switch based on the current frame
                switch (frameState)
                {
                    //each frame calls similar information
                    //each frame clears the platform list to make it free for the next frame
                    //each frame calls the update method in Level and gets the new platform list
                    //berts position is reset and the frame is changed to the next frame
                    case FrameState.Frame0:
                        if (bert.Y <= 0 && currentLevel.Frame[1] != null)
                        {
                            platformList.Clear();
                            currentFrame = 1;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame1;
                        }
                        break;

                    case FrameState.Frame1:
                        if (bert.Y <= 0 && currentLevel.Frame[2] != null)
                        {
                            platformList.Clear();
                            currentFrame = 2;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame2;
                        }
                        break;
                    case FrameState.Frame2:
                        if (bert.Y <= 0 && currentLevel.Frame[3] != null)
                        {
                            platformList.Clear();
                            currentFrame = 3;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame3;
                        }
                        break;
                    case FrameState.Frame3:
                        if (bert.Y <= 0 && currentLevel.Frame[4] != null)
                        {
                            platformList.Clear();
                            currentFrame = 4;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame4;
                        }
                        break;
                    case FrameState.Frame4:
                        if (bert.Y <= 0 && currentLevel.Frame[5] != null)
                        {
                            platformList.Clear();
                            currentFrame = 5;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame5;
                        }
                        break;
                    case FrameState.Frame5:
                        if (bert.Y <= 0 && currentLevel.Frame[6] != null)
                        {
                            platformList.Clear();
                            currentFrame = 6;
                            currentLevel.Update(currentFrame);
                            platformList = currentLevel.PlatformList;
                            bert.Y = 1600;
                            frameState = FrameState.Frame6;
                        }
                        break;
                }
            }
            catch
            {
                //Blank so that way when the array index checking throws an error it doesnt die
            }

            //bert isn't allowed past the left or right wall
            if (bert.X < 0)
            {
                bert.X = 0;
            }
            if ((bert.X > 760))
            {
                bert.X = 760;
            }


            //Bert States

            //assuming god mode is false
            if (godMode == false)
            {
                //switch statement for bert state
                switch (bert.State)
                {
                    //Side to side movement
                    case BertState.Walk:

                        //falling becomes true
                        //each platform is checked to see if bert is intersecting with them, in which case falling becomes false
                        bool falling = true;
                        for (int i = 0; i < platformList.Count; i++)
                        {
                            if (bert.Intersects(platformList[i]))
                            {
                                falling = false;
                            }
                        }

                        //left arrow moves bert left
                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            bert.VelocityX = -4;
                            bert.X += bert.VelocityX;
                        }

                        //right arrow moves bert right
                        else if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            bert.VelocityX = 4;
                            bert.X += bert.VelocityX;
                        }

                        //up arrow and space make bert jump
                        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space))
                        {
                            bert.State = BertState.Jump;
                            bert.VelocityY = 20;
                        }

                        //if bert is falling he experiences gravity and enters the fall state
                        else if (falling)
                        {
                            bert.State = BertState.Fall;
                            bert.GravityY = .5f;
                        }

                        //otherwise bert is walking
                        else
                        {
                            bert.State = BertState.Walk;
                        }
                        break;

                    //Going up aka jumping
                    case BertState.Jump:

                        //gravity when going up
                        bert.GravityY = 1;
                        bert.Y -= bert.VelocityY;
                        bert.VelocityY -= bert.GravityY;

                        //left arrow causes bert to slightly move to the left
                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            bert.VelocityX = -3;
                            bert.X += bert.VelocityX;
                        }

                        //right arrow causes bert to slightly move to the right
                        else if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            bert.VelocityX = 3;
                            bert.X += bert.VelocityX;
                        }

                        //eventually, bert starts falling and enters the fall state rather than the jump state
                        if (bert.VelocityY <= 0)
                        {
                            bert.State = BertState.Fall;
                            bert.GravityY = 1f;
                        }
                        break;

                    //Going down aka falling
                    case BertState.Fall:

                        //falling gravity
                        bert.Y -= bert.VelocityY;
                        bert.VelocityY -= bert.GravityY;

                        //holding space or up causes bert to float temporarily
                        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space))
                        {
                            bert.GravityY = .15f;
                        }

                        //otherwise gravity continues like normal
                        else
                        {
                            bert.GravityY = 1;
                        }

                        //terminal velocity
                        if (bert.VelocityY < -12)
                        {
                            bert.VelocityY = -12;
                        }

                        //checking to see if bert lands on a platform
                        for (int i = 0; i < platformList.Count; i++)
                        {
                            if (bert.Intersects(platformList[i]))
                            {
                                bert.State = BertState.Walk;
                                bert.VelocityY = 0;
                            }
                        }

                        //left arrow causes bert/the player to move slightly to the left
                        if (keyboardState.IsKeyDown(Keys.Left))
                        {
                            bert.VelocityX = -3;
                            bert.X += bert.VelocityX;
                        }

                        //right arrow causes the player to move slightly to the right
                        else if (keyboardState.IsKeyDown(Keys.Right))
                        {
                            bert.VelocityX = 3;
                            bert.X += bert.VelocityX;
                        }
                        break;
                }
            }
            //GOD MODE
            //Movement is unrestricted by gravity
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    bert.X -= 15;
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    bert.X += 15;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    bert.Y += 15;
                }
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space))
                {
                    bert.Y -= 15;
                }
            }

            //Game Switch Statement
            switch (gameState)
            {
                //menu
                case GameState.Menu:

                    //if esc is pressed the program ends
                    if (SingleKeyPress(Keys.Escape, keyboardState))
                    {
                        Exit();
                    }

                    //start button logistics
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // This just gets our mouse position
                        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                        // Makes rectangle for bounds of button
                        Rectangle startButtonBounds = new Rectangle((_graphics.PreferredBackBufferWidth / 2) - 150, 400, (int)(ButtonBaseWidth * 0.2 * .4), (int)(ButtonRectHeight * .4));

                        //checks if mouse position when clicked, is in the bounds of start button
                        if (startButtonBounds.Contains(mousePosition))
                        {
                            levelIndexer = 0;
                            bert.TotalScore = 0;
                            currentLevel = levelList[levelIndexer];
                            levelIndexer++;
                            platformList.Clear();
                            collectibles.Clear();
                            currentLevel.CollectibleSetUp();
                            currentFrame = 0;
                            currentLevel.Update(currentFrame);
                            frameState = FrameState.Frame0;
                            platformList = currentLevel.PlatformList;
                            collectibles = currentLevel.Collectibles;
                            bert.X = 0;
                            bert.Y = 1480;
                            gameState = GameState.Game;
                        }
                    }

                    /*if (SingleKeyPress(Keys.Enter, keyboardState))
                    {
                        levelIndexer = 0;
                        currentLevel = levelList[levelIndexer];
                        levelIndexer++;
                        platformList.Clear();
                        collectibles.Clear();
                        currentLevel.CollectibleSetUp();
                        currentFrame = 0;
                        currentLevel.Update(currentFrame);
                        frameState = FrameState.Frame0;
                        platformList = currentLevel.PlatformList;
                        collectibles = currentLevel.Collectibles;
                        bert.X = 0;
                        bert.Y = 1480;
                        gameState = GameState.Game;
                    }*/
                    break;

                //game
                case GameState.Game:

                    //pause button logistics
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // This just gets our mouse position
                        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                        // Makes rectangle for bounds of button

                        Rectangle pauseButtonBounds = new Rectangle(720, 175, (int)(pauseTexture.Width * 0.15), (int)(pauseTexture.Height * 0.15));
                        Rectangle godModeBounds = new Rectangle(710, 250, (int)(godModeTexture.Width * 0.25), (int)(godModeTexture.Height * 0.25));

                        //checks if mouse position when clicked, is in the bounds of start button
                        if (pauseButtonBounds.Contains(mousePosition))
                        {
                            gameState = GameState.Pause;
                        }
                    }


                    //going back down a frame reverts to the previous frame
                    if (bert.Y > 1600)
                    {
                        bert.Y = 1;
                        platformList.Clear();
                        currentFrame--;
                        currentLevel.Update(currentFrame);
                        platformList = currentLevel.PlatformList;
                        frameState--;
                    }

                    //pressing enter while in the game causes load content method to run
                    if (SingleKeyPress(Keys.Enter, keyboardState))
                    {
                        this.LoadContent();
                    }

                    //if escape, GameState.Pause (alternate way to pause)
                    if (SingleKeyPress(Keys.Escape, keyboardState))
                    {
                        gameState = GameState.Pause;
                    }

                    //press P to go straight to win (Dev Cheat)
                    if (SingleKeyPress(Keys.P, keyboardState))
                    {
                        gameState = GameState.GameWon;
                    }

                    //Collectible logistics
                    foreach (Collectible collectible in collectibles)
                    {
                        //each of these initializes the collectibles for each frame/level
                        //each if does the same code just slightly tweaked for a different level
                        if (collectible.End == true && collectible.Active == false && currentLevel == level1)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }

                        if (collectible.End == true && collectible.Active == false && currentLevel == level2)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level3)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level4)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level5)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level6)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level7)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }
                        if (collectible.End == true && collectible.Active == false && currentLevel == level8)
                        {
                            currentLevel = levelList[levelIndexer];
                            currentFrame = 0;
                            bert.Y = 1480;
                            levelIndexer++;
                            gameState = GameState.LevelChange;
                            break;
                        }

                        //when the last collectible is gotten the game ends
                        if (collectible.End == true && collectible.Active == false && currentLevel == level9)
                        {
                            gameState = GameState.GameWon;
                            break;
                        }
                    }
                    break;

                //level change state
                case GameState.LevelChange:

                    //lists are cleared
                    platformList.Clear();
                    collectibles.Clear();

                    //methods to set up the next level are called
                    currentLevel.CollectibleSetUp();
                    currentLevel.Update(currentFrame);

                    //platform and collectibles are created
                    platformList = currentLevel.PlatformList;
                    collectibles = currentLevel.Collectibles;

                    //bert position is reset
                    bert.X = 0f;
                    bert.Y = 1480;

                    //frame is reset
                    frameState = FrameState.Frame0;

                    //enter to switch out of this state back to game
                    if (SingleKeyPress(Keys.Enter, keyboardState))
                    {
                        gameState = GameState.Game;
                    }
                    break;

                //game win state
                case GameState.GameWon:

                    //if esc is pressed you go back to the menu
                    if (SingleKeyPress(Keys.Escape, keyboardState))
                    {
                        levelIndexer = 0;
                        gameState = GameState.Menu;
                    }

                    //esc button logistics
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // This just gets our mouse position
                        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                        // Makes rectangle for bounds of button

                        Rectangle buttonBounds = new Rectangle((_graphics.PreferredBackBufferWidth / 2) - 150, 750, (int)(ButtonBaseWidth * 0.2 * .4), (int)(ButtonRectHeight * .4));


                        //checks if mouse position when clicked, is in the bounds of start button
                        if (buttonBounds.Contains(mousePosition))
                        {
                            gameState = GameState.Menu;
                        }

                    }


                    break;

                //pause state
                case GameState.Pause:

                    //start and end button logistics in pause menu
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // This just gets our mouse position
                        Vector2 mousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        Rectangle resumeButtonBounds = new Rectangle(
                        (_graphics.PreferredBackBufferWidth / 2) - 150,
                        575,
                        (int)(ButtonBaseWidth * 0.2 * .4),
                        (int)(ButtonRectHeight * .4));

                        Rectangle menuButtonBounds = new Rectangle(
                        (_graphics.PreferredBackBufferWidth / 2) - 150,
                        750,
                        (int)(ButtonBaseWidth * 0.2 * .4),
                        (int)(ButtonRectHeight * .4));

                        Rectangle godModeButtonBounds = new Rectangle((_graphics.PreferredBackBufferWidth / 2) - (godModeButtonTexture.Width / 4) + 10, 1300,
                        (int)(godModeButtonTexture.Width * 0.5),
                        (int)(godModeButtonTexture.Height * .5));
                        //checks if mouse position when clicked, is in the bounds of start button
                        if (resumeButtonBounds.Contains(mousePosition))
                        {
                            gameState = GameState.Game;
                        }
                        if (menuButtonBounds.Contains(mousePosition))
                        {
                            levelIndexer--;
                            gameState = GameState.Menu;
                        }
                        if (godModeButtonBounds.Contains(mousePosition))
                        {
                            godMode = !godMode;
                            gameState = GameState.Game;
                        }
                    }



                    /*if (SingleKeyPress(Keys.Escape, keyboardState))
                    {
                        levelIndexer--;
                        gameState = GameState.Menu;
                    }
                    else if (SingleKeyPress(Keys.Enter, keyboardState))
                    {
                        gameState = GameState.Game;
                    }*/
                    break;
            }

            //collectibles list is always checked to see if a collectible is intersecting with the player
            foreach (Collectible collectible in collectibles)
            {
                if (bert.IntersectsCollectible(collectible) && collectible.FrameOfExistance == currentFrame)
                {
                    //if the collectible is intersecting and is active then total score is changed and active is set to false
                    if (collectible.Active)
                    {
                        bert.TotalScore = bert.TotalScore + 5;
                    }

                    collectible.Active = false;

                }

            }

            //keyboard information reset
            previousFrame = currentFrame;
            previousKBState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// draw method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightBlue);
            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.Menu:
                    //Main Menu text
                    
                    //main menu text
                    _spriteBatch.DrawString(standardFont, "Main Menu", new Vector2(210, 270), Color.Black);

                    //Controls for main menu
                    _spriteBatch.DrawString(secondaryFont, "Press Start to begin !", new Vector2(320, 358), Color.Black);

                    //introduction to controls when reaching game state
                    //_spriteBatch.DrawString(standardFont, "Use Arrows to Control!", new Vector2(300, 450), Color.White);
                    //_spriteBatch.Draw(leaf,new Rectangle(0,0,100,100),Color.White);

                    //!! draw line that needs to be fixed because its printing in a weird spot
                    _spriteBatch.Draw(logo, new Rectangle(7, -170, 800 , 720), Color.White);


                    //Draws the start button
                    _spriteBatch.Draw(
                        buttonDesign,
                        new Vector2((_graphics.PreferredBackBufferWidth / 4) + 30, 400),
                        new Rectangle(1830,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.4,
                        SpriteEffects.None,
                        (float)0);

                    //Draws the controls
                    _spriteBatch.Draw(
                        controlsTexture,
                        new Vector2(_graphics.PreferredBackBufferWidth / 8, 600),
                        new Rectangle(0,
                        0,
                        756,
                        1123),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.8,
                        SpriteEffects.None,
                        (float)0);

                    //Draws the end button
                    /*_spriteBatch.Draw(
                        buttonDesign,
                        new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, 600),
                        new Rectangle(3660,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(5, 120),
                        (float)0.4,
                        SpriteEffects.None,
                        (float)0);*/

                    break;

                case GameState.Game:
                    //Draw bert only in game state
                    GraphicsDevice.Clear(Color.DarkGreen);

                    //draws the background
                    _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 800, 1600), Color.White);

                    //player draw method is called
                    bert.Draw(_spriteBatch);

                    //draws the score and level
                    _spriteBatch.Draw(
                        buttonDesign,
                        new Vector2(670, 25),
                        new Rectangle(0,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(5, 120),
                        (float)0.15,
                        SpriteEffects.None,
                        (float)0);

                    _spriteBatch.Draw(
                       buttonDesign,
                       new Vector2(670, 100),
                       new Rectangle(915,
                       0,
                       (int)(ButtonBaseWidth * 0.2),
                       (int)(ButtonRectHeight)),
                       Color.White,
                       0,
                       new Vector2(5, 120),
                       (float)0.15,
                       SpriteEffects.None,
                       (float)0);

                    /* _spriteBatch.Draw(
                         godModeTexture,
                         new Vector2(710, 250),
                         new Rectangle(0,
                         0,
                         godModeTexture.Width,
                         godModeTexture.Height),
                         Color.White,
                         0,
                         new Vector2(0, 0),
                         (float)0.25,
                         SpriteEffects.None,
                         (float)0);*/

                    //draws the pause button
                    _spriteBatch.Draw(
                        pauseTexture,
                        new Vector2(720, 175),
                        new Rectangle(0,
                        0,
                        pauseTexture.Width,
                        pauseTexture.Height),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.15,
                        SpriteEffects.None,
                        (float)0);


                    //draws player's score
                    _spriteBatch.DrawString(numberFont, bert.TotalScore.ToString(), new Vector2(740, 97), Color.Black);

                    //draws the current level
                    for (int i = 0; i < levelList.Count; i++)
                    {
                        if (currentLevel == levelList[i])
                        {
                            _spriteBatch.DrawString(numberFont, (i + 1).ToString(), new Vector2(745, 23), Color.Black);
                        }
                    }
                    
                    //draws all the platforms using the platform draw class
                    foreach (Platform platform in platformList)
                    {
                        platform.Draw(_spriteBatch);
                    }

                    //draws each collectible based on each collectible's draw class
                    foreach (Collectible collectible in collectibles)
                    {
                        if (collectible.Active == true && collectible.FrameOfExistance == currentFrame)
                        {
                            collectible.Draw(_spriteBatch, Color.White);
                        }
                    }

                    break;

                case GameState.GameWon:

                    //Win text
                    _spriteBatch.DrawString(standardFont, "You Won!", new Vector2(275, 480), Color.Black);

                    //Draws the end button
                    _spriteBatch.Draw(
                        buttonDesign,
                        new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, 750),
                        new Rectangle(3660,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.4,
                        SpriteEffects.None,
                        (float)0);

                    //Win State controls written out
                    _spriteBatch.DrawString(secondaryFont, "Press Exit to go back to \n            the menu", new Vector2(150, 550), Color.Black);
                    break;

                case GameState.Pause:

                    //Pause text
                    _spriteBatch.DrawString(standardFont, "Pause", new Vector2(320, 475), Color.White);

                    //Draws the resume button
                    _spriteBatch.Draw(
                        buttonDesign,
                        new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, 575),
                        new Rectangle(2745,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.4,
                        SpriteEffects.None,
                        (float)0);
                    //Draws the end button
                    _spriteBatch.Draw(
                        buttonDesign,
                        new Vector2((_graphics.PreferredBackBufferWidth / 2) - 150, 750),
                        new Rectangle(3660,
                        0,
                        (int)(ButtonBaseWidth * 0.2),
                        (int)(ButtonRectHeight)),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.4,
                        SpriteEffects.None,
                        (float)0);

                    //Draws god mode button
                    _spriteBatch.Draw(
                        godModeButtonTexture,
                        new Vector2((_graphics.PreferredBackBufferWidth / 2) - (godModeButtonTexture.Width / 4) + 10, 1300),
                        new Rectangle(0,
                        0,
                        godModeButtonTexture.Width,
                        godModeButtonTexture.Height),
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        (float)0.5,
                        SpriteEffects.None,
                        (float)0);

                    //Pause state controls written out
                    /*_spriteBatch.DrawString(secondaryFont, "Press resume to return to game", new Vector2(100, 135), Color.White);
                    _spriteBatch.DrawString(secondaryFont, "Press exit to return to the menu", new Vector2(100, 180), Color.White);*/
                    break;


                case GameState.LevelChange:
                    //draws level change instructions
                    _spriteBatch.DrawString(standardFont, "Press ENTER\n to advance\n to next level", new Vector2(100, 325), Color.Black);

                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool SingleKeyPress(Keys key, KeyboardState kbstate)
        {
            //if key is down but wasn't previously, return true, otherwise, return false
            if (kbstate.IsKeyDown(key) && previousKBState.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}