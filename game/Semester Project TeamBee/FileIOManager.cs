using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester_Project_TeamBee
{
    internal class FileIOManager
    {
        //information about each level's logistics such as name, how many rows, and number of screens are all put into variables
        private string levelName;
        private int columns;
        private int rowsTotal;
        private int numberOfScreens;
        private int rowsPerFrame;

        //a 1d array, of 2d arrays
        private string[][,] frame;

        /// <summary>
        /// NumberOfScreens property that gives other classes access to how many screens a level has
        /// </summary>
        public int NumberOfScreens
        { get { return numberOfScreens; } }

        /// <summary>
        /// Constuctor of the FileIO system
        /// </summary>
        /// <param name="textfileName">..\\..\\..\\filename.txt</param>
        public FileIOManager(string textfileName)
        {
            try
            {
                StreamReader input = new StreamReader(textfileName);
                string line = null;
                //seperates the very first line so we can use the information provided.
                String[] seperation = input.ReadLine().Split(',');
                levelName = seperation[0];
                columns = int.Parse(seperation[1]);
                rowsTotal = int.Parse(seperation[2]);
                numberOfScreens = int.Parse(seperation[3]);
                int indexer = 0;
                rowsPerFrame = rowsTotal / numberOfScreens; ;
                frame = new string[numberOfScreens][,];
                //determines the size of the 2darray to be equal to [rowsPer LevelArray, comluns]
                for (int k = 0; k < numberOfScreens; k++)
                {
                    frame[k] = new string[rowsPerFrame, columns];
                }
                while ((line = input.ReadLine()) != null)
                {
                    //for every symbol in the line, put it into the system, seperated every 40 rows into a new frame for display
                    string[] spots = line.Split(",");
                    for (int i = 0; i < spots.Length; i++)
                    {

                        if (indexer < 40)
                        {
                            frame[0][indexer, i] = spots[i];
                        }
                        if (indexer >= 40 && indexer < 80)
                        {
                            frame[1][(indexer - 40), i] = spots[i];
                        }
                        if (indexer >= 80 && indexer < 120)
                        {
                            frame[2][(indexer - 80), i] = spots[i];
                        }
                        if (indexer >= 120 && indexer < 160)
                        {
                            frame[3][(indexer - 120), i] = spots[i];
                        }
                        if (indexer >= 160 && indexer < 200)
                        {
                            frame[4][(indexer - 160), i] = spots[i];
                        }
                        if (indexer >= 200 && indexer < 240)
                        {
                            frame[5][(indexer - 200), i] = spots[i];
                        }



                    }
                    indexer++;
                }
                input.Close();
            }
            catch (Exception exception)
            {
                //Spritebatch call TODO
            }
        }

        /// <summary>
        /// Returns the level name
        /// </summary>
        public string LevelName
        {
            get { return levelName; }
        }

        /// <summary>
        /// Returns the frame array
        /// </summary>
        public string[][,] LevelArray
        {
            get { return frame; }
        }
    }
}
