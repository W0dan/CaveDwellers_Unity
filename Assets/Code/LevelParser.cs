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

        //backgrounds
        private readonly Color _grassColor = Colors.FromArgb(255, 255, 0, 0);
        private readonly Color _grayTileColor = Colors.FromArgb(255, 255, 0, 1);
        private readonly Color _grayTileWithGrassColor = Colors.FromArgb(255, 255, 0, 2);
        private readonly Color _grayTileWithGrassRightColor = Colors.FromArgb(255, 255, 0, 3);
        private readonly Color _grayTileWithGrassTopColor = Colors.FromArgb(255, 255, 0, 4);
        private readonly Color _grayTileWithGrassLeftColor = Colors.FromArgb(255, 255, 0, 5);
        private readonly Color _wallColor = Colors.FromArgb(255, 0, 0, 255);
        private readonly Color _fountainTileColor = Colors.FromArgb(255, 0, 1, 255);
        private readonly Color _knightStatueTileColor = Colors.FromArgb(255, 0, 2, 255);

        //badguys
        private readonly Color _badGuy1Color = Colors.FromArgb(255, 255, 255, 0);
        private readonly Color _badGuy2Color = Colors.FromArgb(255, 255, 255, 1);

        //pickups
        private readonly Color _ammoPickupColor = Colors.FromArgb(255, 255, 0, 255);
        private readonly Color _healthPickupColor = Colors.FromArgb(255, 255, 1, 255);

        //sprites
        public Transform BadGuy2 { get; set; }
        public Transform BadGuy1 { get; set; }

        public GameObject AmmoPickup { get; set; }
        public GameObject HealthPickup { get; set; }

        public GameObject GrassTile { get; set; }
        public GameObject GrayTile { get; set; }
        public GameObject GrayTileWithGrass { get; set; }
        public GameObject GrayTileWithGrassRight { get; set; }
        public GameObject GrayTileWithGrassTop { get; set; }
        public GameObject GrayTileWithGrassLeft { get; set; }
        public GameObject WallTile { get; set; }
        public GameObject FountainTile { get; set; }
        public GameObject KnightStatueTile { get; set; }

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
                        Badguy = GetBadguyTransform(tilePixels),
                        Pickup = GetPickupTransform(tilePixels),
                        Ornament = GetOrnamentTransform(tilePixels)
                    };

                    var specialFlagsColor = tilePixels[2];
                    if (specialFlagsColor == _keystoneColor)
                    {
                        tileInfo.IsKeystone = true;
                        tileInfo.NextLevelLocation = CreatePointFromColor(tilePixels[4]);
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
                    tileInfo.TriggeredById = GetIntFromColor(tilePixels[3]);

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

        private GameObject GetPickupTransform(IList<Color> tilePixels)
        {
            var pickupColor = tilePixels[1];

            if (pickupColor == _ammoPickupColor)
            {
                return AmmoPickup;
            }
            if (pickupColor == _healthPickupColor)
            {
                return HealthPickup;
            }

            return null;
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

        private GameObject GetOrnamentTransform(IList<Color> tilePixels)
        {
            var ornamentColor = tilePixels[1];
            if (ornamentColor == _fountainTileColor)
                return FountainTile;
            if (ornamentColor == _knightStatueTileColor)
                return KnightStatueTile;

            return null;
        }

        private GameObject GetBackgroundSprite(IList<Color> tilePixels)
        {
            var backgroundTileColor = tilePixels[0];

            if (backgroundTileColor == _wallColor)
                return WallTile;
            if (backgroundTileColor == _grassColor)
                return GrassTile;
            if (backgroundTileColor == _grayTileColor)
                return GrayTile;
            if (backgroundTileColor == _grayTileWithGrassColor)
                return GrayTileWithGrass;
            if (backgroundTileColor == _grayTileWithGrassRightColor)
                return GrayTileWithGrassRight;
            if (backgroundTileColor == _grayTileWithGrassTopColor)
                return GrayTileWithGrassTop;
            if (backgroundTileColor == _grayTileWithGrassLeftColor)
                return GrayTileWithGrassLeft;

            return null;
        }

        private int GetIntFromColor(Color tilePixel)
        {
            var b = (int)(tilePixel.b * 255);
            var g = (int)(256 * tilePixel.g * 255);
            var r = (int)(256 * 256 * tilePixel.r * 255);

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