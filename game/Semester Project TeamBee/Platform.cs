using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Semester_Project_TeamBee
{
    //enum state for different platform types
    enum PlatformType
    {
        Left,
        Right, 
        Middle,
        BreakableLeft,
        BreakableRight,
        BreakableMiddle
    }
    internal class Platform : GameObject
    {
        //Spritesheet data
        const int LeafRectHeight = 40; 
        const int LeafBaseWidth = 40;
        const int LeafMiddleWidth = 40 ;
        const int LeafEndWidth = 40;
        const int SpacerWidth = 40;

        //Inheritance 
        private Texture2D spriteSheet;
        private Rectangle platLoc;

        //Enum for drawing left middle or right
        private PlatformType platformType;

        //Keep track of a breakable platforms color
        //Should be green by default
        private Color breakableColor = Color.Gold;
        private Color color = Color.GreenYellow ;

        //timer for breakable platforms
        double timer;

        //is a platform active
        bool platformActive;

        /// <summary>
        /// position property for where each platform is based on the vector 2
        /// </summary>
        public Vector2 Position
        {
            get { return new Vector2(platLoc.X,platLoc.Y); }
        }

        /// <summary>
        /// position proerty for where each platform is based on their rectangle
        /// </summary>
        public Rectangle PlatLoc
        {
            get { return new Rectangle (platLoc.X,platLoc.Y,40,10); }
        }

        //Platform type will be decided via the file
        //Different characters mean different ends of platforms
        public PlatformType PlatformType
        {
            get { return platformType; }
            set { platformType = value; }
        }

        //We want game1 to be able to check what color its on
        /// <summary>
        /// what color a falling platform is on property
        /// </summary>
        public Color BreakableColor
        {
            get { return breakableColor; }
            set { breakableColor = value; }
        }

        /// <summary>
        /// timer property for falling platforms
        /// </summary>
        public double Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        /// <summary>
        /// platform constructor
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="active"></param>
        public Platform(Texture2D texture, Rectangle position, bool active)
                   : base(texture, position)
        {
            //texture for platform and location for each platform is obtained
            this.spriteSheet = texture;
            this.platLoc = position;

            //the current platform is set naturally to active
            platformActive = active;
        }

        /// <summary>
        /// draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //switch statement for playform types
            switch (platformType)
            {
                //draws each platform part

                //basic platform
                //left
                case PlatformType.Left:
                    DrawLeft(spriteBatch);
                    break;

                //right
                case PlatformType.Right:
                    DrawRight(spriteBatch);
                    break;

                //middle
                case PlatformType.Middle:
                    DrawMiddle(spriteBatch);
                    break;


                //breakable platform

                //left
                case PlatformType.BreakableLeft:
                    DrawLeftBreak(spriteBatch, breakableColor);
                    break;

                //right
                case PlatformType.BreakableRight:
                    DrawRightBreak(spriteBatch, breakableColor);
                    break;

                //middle
                case PlatformType.BreakableMiddle:
                    DrawMiddleBreak(spriteBatch, breakableColor);
                    break;
            }
        }


        /// <summary>
        /// Draws the leftmost side of the leaf in the spritesheet
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawLeft(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle(0,
                0,
                LeafBaseWidth,
                LeafRectHeight),
                color);
        }


        /// <summary>
        /// Draws the leftmost side of a breakable leaf
        /// Needs to be able to change color
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="color"></param>
        public void DrawLeftBreak(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle(0,
                0,
                LeafBaseWidth,
                LeafRectHeight),
                color);
        }


        /// <summary>
        /// Draws the rightmost section of the leaf
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawRight(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle((200 - LeafEndWidth),
                0,
                LeafEndWidth,
                LeafRectHeight),
                color);
        }


        /// <summary>
        /// Draws the rightmost section of a breakable leaf
        /// Needs to be able to change color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawRightBreak(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle((200 - LeafEndWidth),
                0,
                LeafEndWidth,
                LeafRectHeight),
                color);
        }


        /// <summary>
        /// Draws the middle section of the leaf in the sprite sheet
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMiddle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle((80),
                0,
                LeafBaseWidth,
                LeafRectHeight),
                color);
        }


        /// <summary>
        /// Draws the middle section of a breakable leaf
        /// Needs to be able to change color
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawMiddleBreak(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(
                spriteSheet,
                Position,
                new Rectangle((80),
                0,
                LeafBaseWidth,
                LeafRectHeight),
                color);
        }

        
        /// <summary>
        /// update method
        /// </summary>
        /// <param name="active"></param>
        public void Update(bool active)
        {
            //if a platform isn't active its location is moved to make you fall through it
            if (!active)
            {
                this.platLoc = new Rectangle (-1000,-1000,0,0);
            }
        }

        /// <summary>
        /// check falling method
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool CheckFalling(GameTime gameTime, Player player)
        {
            //if the current platform is any of the breakable types
            if(this.PlatformType == PlatformType.BreakableLeft||
               this.PlatformType == PlatformType.BreakableMiddle ||
               this.PlatformType == PlatformType.BreakableRight)
            {
                //if player is intersecting with platform
                if(player.Intersects(this) && (timer== 0))
                {
                    //timer started
                    timer = gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                //timer isn't 0
                if(timer != 0)
                {
                    //timer is changed based on gameTime
                    timer += gameTime.ElapsedGameTime.TotalMilliseconds;

                    //if timer is above 0
                    if(timer > 0)
                    {
                        //based on the different stage in the timer that it currently is, the platform gets tinted different colors
                        if (timer > 0)
                        {
                            this.BreakableColor = Color.YellowGreen;
                        }
                        if (timer > 500 && timer <= 1000)
                        {
                            this.BreakableColor = Color.Orange;
                        }
                        if (timer > 1000 && timer <= 1500)
                        {
                            this.BreakableColor = Color.Red;
                        }
                        if (timer > 1500)
                        {
                            //once the platform reaches maximum timer, it becomes inactive and disappears
                            this.platformActive = false;
                            timer = 0;
                        }
                    }
                }
            }
            return platformActive;
        }
    }
}
