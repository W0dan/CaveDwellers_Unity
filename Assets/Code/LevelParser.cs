using System.Collections.Generic;
using Assets.Code.Common;
using UnityEngine;

namespace Assets.Code
{
    public class LevelParser
    {
        //special flags
        private readonly Color _spawnPointColor = Colors.FromArgb(255, 255, 255, 255);
        private readonly Color _keystoneColor = Colors.FromArgb(255, 0, 0, 255);
        private readonly Color _triggerColor = Colors.FromArgb(255, 0, 0, 254);
        private readonly Color _hiddenColor = Colors.FromArgb(255, 0, 0, 253);

        //background sprites
        private readonly Color _floorColor = Colors.FromArgb(255, 255, 0, 0);
        private readonly Color _wallColor = Colors.FromArgb(255, 0, 0, 255);

        //badguys
        private readonly Color _badGuy1Color = Colors.FromArgb(255, 255, 255, 0);
        private readonly Color _badGuy2Color = Colors.FromArgb(255, 255, 255, 1);

        //sprites
        public Transform BadGuy2 { get; set; }
        public Transform BadGuy1 { get; set; }
        public GameObject FloorTile { get; set; }
        public GameObject WallTile { get; set; }

        public IEnumerable<TileInfo> GetLevelData(Texture2D levelTexture)
        {
            var levelHeight = levelTexture.height / 4;
            var levelWidth = levelTexture.width / 4;

            var pixels = levelTexture.GetPixels();

            //all information of 1 tile is composed of a square of 4x4 pixels
            for (var y = 0; y < levelHeight; y++)
            {
                for (var x = 0; x < levelWidth; x++)
                {
                    var tilePixels = GetTilePixels(pixels, x, y, levelTexture.width);

                    var tileInfo = new TileInfo
                    {
                        X = x,
                        Y = y,
                        BackgroundTile = GetBackgroundSprite(tilePixels),
                        Badguy = GetBadguyTransform(tilePixels)
                    };

                    var specialFlagsColor = tilePixels[2];
                    if (specialFlagsColor == _keystoneColor)
                    {
                        tileInfo.IsKeystone = true;
                        tileInfo.NextLevelLocation = CreatePointFromColor(tilePixels[3]);
                    }
                    if (specialFlagsColor == _spawnPointColor)
                    {
                        tileInfo.IsSpawnPoint = true;
                    }
                    if (specialFlagsColor == _triggerColor)
                    {
                        tileInfo.IsTrigger = true;
                        tileInfo.TriggerId = GetIntFromColor(tilePixels[4]);
                    }
                    if (specialFlagsColor == _hiddenColor)
                    {
                        tileInfo.IsHidden = true;
                        tileInfo.TriggeredById = GetIntFromColor(tilePixels[3]);
                    }

                    yield return tileInfo;
                }
            }
        }

        private static Color[] GetTilePixels(Color[] pixels, int x, int y, int totalWidth)
        {
            var tilePixels = new Color[16];

            for (var j = 0; j < 4; j++)
            {
                for (var i = 0; i < 4; i++)
                {
                    tilePixels[i + 12 - (j * 4)] = pixels[(x * 4) + i + ((y * 4) + j) * totalWidth];
                }
            }

            return tilePixels;
        }

        private Transform GetBadguyTransform(IList<Color> tilePixels)
        {
            var badguyColor = tilePixels[1];
            if (badguyColor == _badGuy1Color)
            {
                return BadGuy1;
            }
            if (badguyColor == _badGuy2Color)
            {
                return BadGuy2;
            }

            return null;
        }

        private GameObject GetBackgroundSprite(IList<Color> tilePixels)
        {
            var backgroundTileColor = tilePixels[0];

            if (backgroundTileColor == _wallColor)
            {
                return WallTile;
            }

            if (backgroundTileColor == _floorColor)
            {
                return FloorTile;
            }

            return null;
        }

        private int GetIntFromColor(Color tilePixel)
        {
            var b = (int)(tilePixel.b*255);
            var g = (int)(256 * tilePixel.g*255);
            var r = (int)(256 * 256 * tilePixel.r*255);

            return r + g + b;
        }

        private Point CreatePointFromColor(Color tilePixel)
        {
            var x = (int)(tilePixel.g * 255);
            var y = (int)(tilePixel.b * 255);

            return new Point(x, y);
        }
    }
}