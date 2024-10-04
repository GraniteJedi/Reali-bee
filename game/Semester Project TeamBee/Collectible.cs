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
    internal class Collectible : GameObject
    {
        //field to check to see if a collectible is currently activated
        private bool active;
        private bool end;
        private int frameOfExistance;

        /// <summary>
        /// Constructor for Collectible objects
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="active"></param>
        /// <param name="end"></param>
        /// <param name="frameOfExistance">the frame that logic appplies to this collectible.</param>
        public Collectible(Texture2D texture, Rectangle position, bool active, bool end, int frameOfExistance)
                    : base(texture, position)
        {
            this.active = active;
            this.end = end;
            this.frameOfExistance = frameOfExistance;
        }
        /// <summary>
        /// Returns what frame it should be displayed on 
        /// </summary>
        public int FrameOfExistance
        { get { return frameOfExistance; } }
        /// <summary>
        /// Should the collectible be displayed/has it been collected 
        /// </summary>
        public bool Active 
        { 
            get { return active; }

            set { active = value; } 
        }
        /// <summary>
        /// Is this the end collectible?
        /// </summary>
        public bool End
        { get { return end; }  }
        

    }
}
