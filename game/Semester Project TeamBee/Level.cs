using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester_Project_TeamBee
{
    internal class Level
    {
        //Fields

        //current frame information
        private string[][,] frame;

        //fileiomanger field
        private FileIOManager level1;

        //size of the grid
        private int gridSize = 40;

        //list of platforms, collectibles, and falling platforms
        private List<Platform> platformList;
        private List<Collectible> collectibles;
        private List<Platform> fallingPlatformList;

        //timer for falling platforms
        private double gameTimer;

        //textures
        private Texture2D leaf;
        private Texture2D endCollectibleTex;
        private Texture2D collectibleTex;

        /// <summary>
        /// Constructor for the level class which calls the fileIOManger. 
        /// </summary>
        /// <param name="fileName">The ..\\..\\..\\ name of file.txt</param>
        /// <param name="leaf">The texture2D for leaf, will eventually also need the collecitble texture</param>
        /// <param name="collectibleTex"> texture for normal collectible</param>
        /// <param name="endCollectibleTex"></param> end collectible texture 
        public Level(string fileName, Texture2D leaf, Texture2D collectibleTex ,  Texture2D endCollectibleTex)
        {
            //level takes info from file io manager
            this.level1 = new FileIOManager(fileName);

            //current frame set to the level array from the file io
            frame = level1.LevelArray;

            //textures for leaf and collectible are gotten
            this.leaf = leaf;
            this.collectibleTex = collectibleTex;

            //platform, collectible, and falling platform lists are initialized
            platformList = new List<Platform>();
            collectibles = new List<Collectible>();
            fallingPlatformList = new List<Platform>();

            //end collectible texture is gotten
            this.endCollectibleTex = endCollectibleTex;
        }
        /// <summary>
        /// Returns the frame level array
        /// </summary>
        public string[][,] Frame { get { return frame; } }

        /// <summary>
        /// returns the platformList
        /// </summary>
        public List<Platform> PlatformList
        {
            get { return platformList; }
        }
        
        //collectible list property
        public List<Collectible> Collectibles { get { return collectibles; } }

        /// <summary>
        /// Sets up the collectibles for a specific frame
        /// </summary>
        public void CollectibleSetUp()
        {
            //iterates through the screens of the current level
            for (int i = 0; i < level1.NumberOfScreens; i++)
            {
                //iterates through the rows and columns
                for (int j = 0; j < frame[i].GetLength(1); j++)
                {
                    for (int l = 0; l < frame[i].GetLength(0); l++)
                    {
                        //if P or C is in the level it is replaced with collectibles
                        if (Frame[i][l, j] == "P")
                        {
                            Collectible collectible = new Collectible(endCollectibleTex, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true, true, i);
                            collectibles.Add(collectible);
                        }
                        if (Frame[i][l, j] == "C")
                        {
                            Collectible collectible = new Collectible(collectibleTex, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true, false, i);
                            collectibles.Add(collectible);
                        }


                    }
                }

            }
        }


        /// <summary>
        /// Clears then updates the platformlist  
        /// </summary>
        /// <param name="currentFrame">The current frame the game is on</param>
        public void Update(int currentFrame)
        {
            //platform list is cleared
            platformList.Clear();

            //iterates through rows and columns
            for (int j = 0; j < frame[currentFrame].GetLength(1); j++)
            {
                for (int l = 0; l < frame[currentFrame].GetLength(0); l++)
                {
                    //left part of basic platform
                    if (frame[currentFrame][l, j] == "<")
                    {
                        // always start with a <
                        Platform newPlatformLeft = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformLeft.PlatformType = PlatformType.Left;
                        platformList.Add(newPlatformLeft);
                    }

                    //middle part of basic platform
                    else if ((frame[currentFrame][l, j]) == "~")
                    {
                        Platform newPlatformMiddle = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformMiddle.PlatformType = PlatformType.Middle;
                        platformList.Add(newPlatformMiddle);
                    }

                    //right part of basic platform
                    else if ((frame[currentFrame][l, j]) == ">")
                    {
                        Platform newPlatformEnd = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformEnd.PlatformType = PlatformType.Right;
                        platformList.Add(newPlatformEnd);
                    }

                    //left part of falling platforms
                    else if (frame[currentFrame][l, j] == "(")
                    {
                        Platform newPlatformLeft = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformLeft.PlatformType = PlatformType.BreakableLeft;
                        platformList.Add(newPlatformLeft);
                    }

                    //middle part of falling platforms
                    else if ((frame[currentFrame][l, j]) == "-")
                    {
                        Platform newPlatformMiddle = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformMiddle.PlatformType = PlatformType.BreakableMiddle;
                        platformList.Add(newPlatformMiddle);
                    }

                    //right part of falling platforms
                    else if ((frame[currentFrame][l, j]) == ")")
                    {
                        Platform newPlatformEnd = new Platform(leaf, new Rectangle(j * gridSize, l * gridSize, gridSize, gridSize), true);
                        newPlatformEnd.PlatformType = PlatformType.BreakableRight;
                        platformList.Add(newPlatformEnd);
                    }
                }
            }
        }
    }
}
