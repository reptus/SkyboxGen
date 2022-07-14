using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace SkyboxGen
{
    public class SkyboxGen
    {
        private static int imageWidth, imageHeight, tileWidth, tileHeight;
        private static string path = "", filename = "";
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    Console.WriteLine("Configuration type, directory and file name are required.");
                    Console.WriteLine("Usage: Skyboxgen skyboxType[0..3] file");
                    Console.WriteLine(@"Example: Skyboxgen 0 c:\temp\myImage.png");
                    return;
                }
                _ = int.TryParse(args[0], out int configType);
                var filePath = args[1];
                var index = GetIndexFilename(filePath);
                path = filePath.Substring(0, filePath.IndexOf(@"\", index));
                filename = filePath.Substring(filePath.IndexOf(@"\", index) + 1);
                filename = filename.Substring(0, filename.IndexOf("."));
                var image = new Bitmap(filePath);
                imageWidth = image.Width;
                imageHeight = image.Height;
                tileWidth = imageWidth / 4;
                tileHeight = imageHeight / 3;

                GenerateTiles(image, configType);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Generate skybox tiles for an image and a config type
       /// </summary>
        /// <param name="image"></param>
        /// <param name="configType"></param>
        private static void GenerateTiles(Bitmap image, int configType)
        {
            switch (configType)
            {
                case 0:
                    CreateTile(image, 0, tileHeight, "Front");
                    CreateTile(image, 2 * tileWidth, tileHeight, "Back");
                    CreateTile(image, 3 * tileWidth, tileHeight, "Left");
                    CreateTile(image, tileWidth, tileHeight, "Right");
                    CreateTile(image, 0, 0, "Up");
                    CreateTile(image, 0, 2 * tileHeight, "Down");
                    break;
                case 1:
                default:
                    CreateTile(image, tileWidth, tileHeight, "Front");
                    CreateTile(image, 3 * tileWidth, tileHeight, "Back");
                    CreateTile(image, 0, tileHeight, "Left");
                    CreateTile(image, 2 *tileWidth, tileHeight, "Right");
                    CreateTile(image, tileWidth, 0, "Up");
                    CreateTile(image, tileWidth, 2 * tileHeight, "Down");
                    break;
                case 2:
                    CreateTile(image, 2 * tileWidth, tileHeight, "Front");
                    CreateTile(image, 0, tileHeight, "Back");
                    CreateTile(image, tileWidth, tileHeight, "Left");
                    CreateTile(image, 3 * tileWidth, tileHeight, "Right");
                    CreateTile(image, 2 * tileWidth, 0, "Up");
                    CreateTile(image, 2 * tileWidth, 2 * tileHeight, "Down");
                    break;
                case 3:
                    CreateTile(image, 3 * tileWidth, tileHeight, "Front");
                    CreateTile(image, tileWidth, tileHeight, "Back");
                    CreateTile(image, 2 * tileWidth, tileHeight, "Left");
                    CreateTile(image, 0, tileHeight, "Right");
                    CreateTile(image, 3 * tileWidth, 0, "Up");
                    CreateTile(image, 3 * tileWidth, 2 * tileHeight, "Down");
                    break;                
            }            
        }

        /// <summary>
        /// Create a new tile file reading pixels starting at (posX, posY)
        /// Extension for the file is png
        /// </summary>
        /// <param name="image"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="tileName"></param>
        private static void CreateTile(Bitmap image, int posX, int posY, string tileName)
        {
            var tile = new Bitmap(tileWidth, tileHeight);
            int j = 0;
            for(int y = posY; y < posY + tileHeight; y++)
            {
                int i = 0;
                for (int x = posX; x < posX + tileWidth; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    tile.SetPixel(i, j, pixelColor);
                    i ++;
                }
                j ++;
            }
            string name = path + "\\" + filename + "_"+ tileName + ".png";
            tile.Save(name, System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Find the string index for the last backslash in the filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static int GetIndexFilename(string filename)
        {
            int i = 0, pos = 1;
            while (pos > 0)
            {
                pos = filename.IndexOf(@"\", i);
                i++;
            }
            return i - 2; 
        }
    }
}
