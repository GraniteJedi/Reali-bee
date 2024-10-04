using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester_Project_TeamBee
{
    enum BertState
    {
        Walk,
        Jump,
        Fall
    }
    internal class Player : GameObject
    {
        //Inheritance
        private Texture2D spriteSheet;
        private Vector2 playerLoc;

        //Physics fields
        private Vector2 velocity = Vector2.Zero;
        private Vector2 gravity = new Vector2(0, 1);

        //Animation fields
        private int frame;             
        private double timeCounter;     
        private double fps;             
        private double timePerFrame;

        //FSM and movement
        private BertState state;

        //Collision
        private Rectangle hitbox;
        private int totalScore;

        //Possitional properties
        public float X
        {
            get { return this.playerLoc.X; }
            set { this.playerLoc.X = value; }
        }
        //y property for player location 
        public float Y
        {
            get { return this.playerLoc.Y; }
            set { this.playerLoc.Y = value; }
        }

        //Physics properties (moving left and right and how fast)
        public float VelocityX
        {
            get { return this.velocity.X; }
            set { this.velocity.X = value; }
        }
        //Velocity in the y
        public float VelocityY
        {
            get { return this.velocity.Y; }
            set { this.velocity.Y = value; }
        }

        //Gravity will be different in different scenarios
        //Should only be able to change the Y, gravity should never be in the X direction
        public float GravityY
        {
            get { return this.gravity.Y; }
            set { this.gravity.Y = value; }
        }

        //bert state property for which state bert is in
        public BertState State
        {
            get { return state; }
            set { state = value; }  
        }

        //score property for how much score the player currently has
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }



        //previous keyboard state to hold the previous keyboard for SingleKeyPress method
        KeyboardState previousKBState;
        
        const int FallFrameCount = 3;
        const int BertRectHeight = 40;//placeholder     
        const int BertRectWidth = 40;//placeholder
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="position"></param>
        /// <param name="starringSate"></param>
        public Player(Texture2D spriteSheet, Rectangle position, BertState starringSate)
            : base(spriteSheet, position)
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc.Y = position.Y;
            this.playerLoc.X = position.X;
            this.state = starringSate;
            hitbox = new Rectangle((int)playerLoc.X, (int)playerLoc.Y, 40, 40);
            fps = 10.0;
            timePerFrame = 1.0 / fps;
        }

        /// <summary>
        /// Method do handle animation
        /// Works at 10 frames per second
        /// 3 frames total
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateAnimation(GameTime gameTime)
        {
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeCounter >= timePerFrame)
            {
                frame += 1;                    
                if (frame > FallFrameCount)     
                    frame = 1;                 
                timeCounter -= timePerFrame;                                 
            }
        }

        /// <summary>
        /// Draws differently based on the state
        /// Moving side to side and moving down should look the same
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (State)
            {
                case BertState.Walk:
                case BertState.Jump:
                    if(VelocityX >= 0)
                    {
                        DrawStationary(SpriteEffects.None,spriteBatch);
                    }
                    else
                    {
                        DrawStationary(SpriteEffects.FlipHorizontally,spriteBatch);
                    }
                break;
                case BertState.Fall:
                    if(VelocityX >= 0)
                    {
                        DrawFalling(SpriteEffects.None,spriteBatch);
                    }
                    else
                    {
                        DrawFalling(SpriteEffects.FlipHorizontally, spriteBatch);
                    }
                break;
            }
        }
        /// <summary>
        /// For drawing when walking or jumping
        /// No animation
        /// </summary>
        /// <param name="flipSprite"></param>
        /// <param name="spriteBatch"></param>
        public void DrawStationary(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,                   
                playerLoc,                      
                new Rectangle(                  
                    0,                          
                    0,           
                    BertRectWidth,            
                    BertRectHeight),          
                Color.White,                  
                0,                             
                Vector2.Zero,                   
                1.0f,                           
                flipSprite,                     
                0);                             
        }
        /// <summary>
        /// Falling animation
        /// 3 frames of bert
        /// Only thing that changes between frames is the propeller hat's rotation
        /// </summary>
        /// <param name="flipSprite"></param>
        /// <param name="spriteBatch"></param>
        private void DrawFalling(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                playerLoc,
                new Rectangle(
                    frame * BertRectWidth*2,
                    0,
                    BertRectWidth,
                    BertRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                flipSprite,
                0);
        }
        /// <summary>
        /// SingleKeyPress method
        /// Checks to see if the current keyboard has a key pressed down that previously wasn't pressed down
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kbState"></param>
        /// <returns></returns>
        public bool SingleKeyPress (Keys key, KeyboardState kbState)
        {
            return (kbState.IsKeyDown(key) && previousKBState.IsKeyUp(key));
        }

        /// <summary>
        /// Intersecting method for platforms
        /// Checks to see if the player is intersecting with a platform 
        /// </summary>
        /// <param name="collectible"></param>
        /// <returns></returns>
        /// 
        public bool Intersects(Platform platform)
        {
            hitbox = new Rectangle((int)playerLoc.X, (int)playerLoc.Y+30, 40,10);
            if (hitbox.Intersects(platform.PlatLoc) == true)
            {
                return true;
            }
            return false;
        }
        
        //method for intersecting with collectibles
        public bool IntersectsCollectible(Collectible collectible)
        {
            hitbox = new Rectangle((int)X, (int)Y, 40, 40);
            if (hitbox.Intersects(collectible.CurrentPosition) == true)
            {
                return true;
            }
            return false;
        }
    }
}
