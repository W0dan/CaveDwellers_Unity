using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Common;

public class World : MonoBehaviour
{
    public GameObject FloorTile;
    public GameObject WallTile;

    private readonly Color _floorColor = Colors.FromArgb(255, 255, 0, 0);
    private readonly Color _wallColor = Colors.FromArgb(255, 0, 0, 255);
    private readonly Color _spawnPointColor = Colors.FromArgb(255, 0, 0, 0);
    private readonly Color _badGuyColor = Colors.FromArgb(255, 255, 255, 0);
    private readonly Color _keystoneColor = Colors.FromArgb(255, 0, 0, 254);

    public List<Texture2D> LevelTextures;
    private Texture2D _levelTexture;
    private int _levelIndex = 0;
    private int _currentLevelXOffset = 0;

    public Player Player;

    public Transform BadGuy;

    private int _numberOfBadguysLeft = 0;
    private Vector3 _keystoneLocation;
    private Object _keystone;


    internal bool LevelFinished
    {
        get { return _numberOfBadguysLeft <= 0; }
    }

    void Start()
    {
        _levelTexture = LevelTextures[_levelIndex];

        LoadLevel(_levelTexture.width, _levelTexture.height, _currentLevelXOffset);
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
            var oldTextureWith = _levelTexture.width;
            _levelTexture = LevelTextures[_levelIndex];

            //finished, open exit ...
            Destroy(_keystone);
            Instantiate(FloorTile, _keystoneLocation, Quaternion.identity);

            //... and draw new level
            _currentLevelXOffset += oldTextureWith;
            LoadLevel(_levelTexture.width, _levelTexture.height, _currentLevelXOffset);
        }
    }

    private void LoadLevel(int width, int height, int xOffset = 0, int yOffset = 0)
    {
        var tileColors = _levelTexture.GetPixels();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentTileColor = tileColors[x + y * width];

                if (currentTileColor == _badGuyColor)
                {
                    var bgClone = (Transform)Instantiate(BadGuy, new Vector3(x + xOffset, y), Quaternion.identity);
                    var badguy = bgClone.GetComponent<Entity>();

                    badguy.Died += BadguyDied;

                    _numberOfBadguysLeft++;
                }
            }
        }

        //always do background last, because what you draw later dissapears behind it.
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentTileColor = tileColors[x + y * width];

                if (currentTileColor.IsEqualTo(_floorColor)
                    || currentTileColor.IsEqualTo(_spawnPointColor)
                    || currentTileColor.IsEqualTo(_badGuyColor))
                {
                    Instantiate(FloorTile, new Vector3(x + xOffset, y), Quaternion.identity);
                }
                if (currentTileColor.IsEqualTo(_wallColor))
                {
                    Instantiate(WallTile, new Vector3(x + xOffset, y), Quaternion.identity);
                }
                if (currentTileColor.IsEqualTo(_keystoneColor))
                {
                    _keystoneLocation = new Vector3(x + xOffset, y);
                    _keystone = Instantiate(WallTile, _keystoneLocation, Quaternion.identity);
                }
                if (currentTileColor.IsEqualTo(_spawnPointColor))
                {
                    var startPosition = new Vector2(x + xOffset, y);
                    Player.transform.position = startPosition;
                }
            }
        }
    }

    private void BadguyDied(Entity badguy)
    {
        _numberOfBadguysLeft--;
    }
}
