using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using UnityEngine;
using Assets.Code.Common;
using Assets.Code.Entities;
using Assets.Code.Entities.Player;

public class World : MonoBehaviour
{
    public GameObject FloorTile;
    public GameObject WallTile;

    //special flags
    private readonly Color _spawnPointColor = Colors.FromArgb(255, 255, 255, 255);
    private readonly Color _keystoneColor = Colors.FromArgb(255, 0, 0, 255);

    //background sprites
    private readonly Color _floorColor = Colors.FromArgb(255, 255, 0, 0);
    private readonly Color _wallColor = Colors.FromArgb(255, 0, 0, 255);

    //badguys
    private readonly Color _badGuy1Color = Colors.FromArgb(255, 255, 255, 0);
    private readonly Color _badGuy2Color = Colors.FromArgb(255, 255, 255, 1);

    public List<Texture2D> LevelTextures;
    private Texture2D _levelTexture;
    private int _levelIndex = 0;
    private int _currentLevelXOffset = 0;

    public Player Player;

    public Transform BadGuy1;
    public Transform BadGuy2;

    private int _numberOfBadguysLeft = 0;
    //private Vector3 _keystoneLocation;
    private Object _keystone;
    private Vector3 _keystoneLocation;


    internal bool LevelFinished
    {
        get { return _numberOfBadguysLeft <= 0; }
    }

    void Start()
    {
        _levelTexture = LevelTextures[_levelIndex];

        LoadLevel(_levelTexture, _currentLevelXOffset);
    }

    void Update()
    {
        if (_numberOfBadguysLeft <= 0)
        {
            _levelIndex++;
            if (!(LevelTextures.Count > _levelIndex))
            {
                //no more levels
                return;
            }
            var oldTextureWith = _levelTexture.width / 4;
            _levelTexture = LevelTextures[_levelIndex];

            //finished, open exit ...
            Destroy(_keystone);
            Instantiate(FloorTile, _keystoneLocation, Quaternion.identity);

            //... and draw new level
            _currentLevelXOffset += oldTextureWith;
            LoadLevel(_levelTexture, _currentLevelXOffset);
        }
    }

    private IEnumerable<TileInfo> GetLevelData(Texture2D levelTexture)
    {
        var levelHeight = levelTexture.height / 4;
        var levelWidth = levelTexture.width / 4;

        var pixels = _levelTexture.GetPixels();

        //all information of 1 tile is composed of a square of 4x4 pixels
        for (var y = 0; y < levelHeight; y++)
        {
            for (var x = 0; x < levelWidth; x++)
            {
                var tileInfo = new TileInfo();
                tileInfo.X = x;
                tileInfo.Y = y;
                string description;
                tileInfo.BackgroundTile = GetBackgroundSprite(pixels, x, y, levelTexture.width, out description);
                tileInfo.Badguy = GetBadguyTransform(pixels, x, y, levelTexture.width);
                tileInfo.Description = description;

                var specialFlagsColor = pixels[(x * 4) + 2 + ((y * 4) + 3) * levelTexture.width];
                if (specialFlagsColor == _keystoneColor)
                {
                    tileInfo.IsKeystone = true;
                }
                if (specialFlagsColor == _spawnPointColor)
                {
                    tileInfo.IsSpawnPoint = true;
                }

                yield return tileInfo;
            }
        }
    }

    private Transform GetBadguyTransform(IList<Color> pixels, int x, int y, int levelWidth)
    {
        var objectColor = pixels[(x * 4) + 1 + ((y * 4) + 3) * levelWidth];
        if (objectColor == _badGuy1Color)
        {
            return BadGuy1;
        }
        if (objectColor == _badGuy2Color)
        {
            return BadGuy2;
        }

        return null;
    }

    private GameObject GetBackgroundSprite(IList<Color> pixels, int x, int y, int levelWidth, out string description)
    {
        var index = (x * 4) + ((y * 4) + 3) * levelWidth;
        var backgroundTileColor = pixels[index];

        if (backgroundTileColor == _wallColor)
        {
            description = "wall";
            return WallTile;
        }

        if (backgroundTileColor == _floorColor)
        {
            description = "floor";
            return FloorTile;
        }

        description = "unknown";
        return FloorTile;
    }

    private void LoadLevel(Texture2D levelTexture, int xOffset = 0, int yOffset = 0)
    {
        var tiles = GetLevelData(levelTexture).ToList();

        foreach (var tileInfo in tiles)
        {
            //badguys
            if (tileInfo.Badguy != null)
            {
                var obj = (Transform)Instantiate(tileInfo.Badguy, new Vector3(tileInfo.X + xOffset, tileInfo.Y), Quaternion.identity);
                var badguy = obj.GetComponent<Entity>();

                badguy.Died += BadguyDied;

                _numberOfBadguysLeft++;
            }

            //spawn point
            if (tileInfo.IsSpawnPoint)
            {
                Player.transform.position = new Vector2(tileInfo.X + xOffset, tileInfo.Y);
            }

            //key stone
            if (tileInfo.IsKeystone)
            {
                _keystoneLocation = new Vector3(tileInfo.X + xOffset, tileInfo.Y);
                _keystone = Instantiate(WallTile, _keystoneLocation, Quaternion.identity);
            }
        }

        //floor and walls
        foreach (var tileInfo in tiles)
        {
            if (!tileInfo.IsKeystone)
                Instantiate(tileInfo.BackgroundTile, new Vector3(tileInfo.X + xOffset, tileInfo.Y), Quaternion.identity);
        }
    }

    private void BadguyDied(Entity badguy)
    {
        _numberOfBadguysLeft--;
    }
}
