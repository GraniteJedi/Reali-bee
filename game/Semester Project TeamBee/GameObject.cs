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
    internal class GameObject
    {
        //Texture and position fields to hold game object's designs and locations within the screen
        protected Texture2D currentTexture;
        protected Rectangle currentPosition;

        /// <summary>
        /// CurrentPosition read only property
        /// </summary>
        public Rectangle CurrentPosition
        {
            get { return currentPosition; }
        }

        /// <summary>
        /// GameObject constructor
        /// Is the basis for all GameObjects by using texture and position
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        protected GameObject(Texture2D texture, Rectangle position)
        {
            this.currentPosition = position;
            this.currentTexture = texture;
        }

        /// <summary>
        /// Virtual Draw Method for the game objects
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="tint"></param>
        public virtual void Draw(SpriteBatch sb, Color tint)
        {
            sb.Draw(currentTexture, currentPosition, tint);
        }
    }
}
