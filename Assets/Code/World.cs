﻿using System.Collections.Generic;
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

    ////special flags
    //private readonly Color _spawnPointColor = Colors.FromArgb(255, 255, 255, 255);
    //private readonly Color _keystoneColor = Colors.FromArgb(255, 0, 0, 255);
    //private readonly Color _triggerColor = Colors.FromArgb(255, 0, 0, 254);
    //private readonly Color _hiddenColor = Colors.FromArgb(255, 0, 0, 253);

    ////background sprites
    //private readonly Color _floorColor = Colors.FromArgb(255, 255, 0, 0);
    //private readonly Color _wallColor = Colors.FromArgb(255, 0, 0, 255);

    ////badguys
    //private readonly Color _badGuy1Color = Colors.FromArgb(255, 255, 255, 0);
    //private readonly Color _badGuy2Color = Colors.FromArgb(255, 255, 255, 1);

    public List<Texture2D> LevelTextures;
    private Texture2D _levelTexture;
    private int _levelIndex = 0;
    private int _currentLevelXOffset = 0;
    private int _currentLevelYOffset = 0;

    public Player Player;

    public Transform BadGuy1;
    public Transform BadGuy2;

    private int _numberOfBadguysLeft = 0;
    private Object _keystone;
    private Vector3 _keystoneLocation;

    private LevelParser _levelParser;
    private List<TileInfo> _currentLevelTiles;

    //subject to change ... this construct would only allow for 1 trigger per level.
    private GameObject _triggerStone;

    internal bool LevelFinished
    {
        get { return _numberOfBadguysLeft <= 0; }
    }

    void Start()
    {
        Player.HitTrigger += Player_HitTrigger;

        _levelTexture = LevelTextures[_levelIndex];

        _levelParser = new LevelParser
        {
            FloorTile = FloorTile,
            WallTile = WallTile,
            BadGuy1 = BadGuy1,
            BadGuy2 = BadGuy2
        };
        _currentLevelTiles = _levelParser.GetLevelData(_levelTexture).ToList();
        LoadLevel(_currentLevelTiles, _currentLevelXOffset);
    }

    private void Player_HitTrigger(int triggerId)
    {
       // var triggerTile = _currentLevelTiles.Single(tile => tile.IsTrigger && tile.TriggerId == triggerId);

        Destroy(_triggerStone);

        LoadLevel(_currentLevelTiles, _currentLevelXOffset, _currentLevelYOffset, triggerId);
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
            //var oldTextureWith = _levelTexture.width / 4;
            var nextLevelLocation = _currentLevelTiles.Single(tile => tile.IsKeystone).NextLevelLocation;
            _levelTexture = LevelTextures[_levelIndex];

            //finished, open exit ...
            Destroy(_keystone);
            Instantiate(FloorTile, _keystoneLocation, Quaternion.identity);

            //... and draw new level
            _currentLevelXOffset += nextLevelLocation.X;
            _currentLevelYOffset += nextLevelLocation.Y;
            _currentLevelTiles = _levelParser.GetLevelData(_levelTexture).ToList();
            LoadLevel(_currentLevelTiles, _currentLevelXOffset, _currentLevelYOffset);
        }
    }

    private void LoadLevel(List<TileInfo> tiles, int xOffset = 0, int yOffset = 0, int triggerId = 0)
    {
        foreach (var tileInfo in tiles.Where(tile => tile.TriggeredById == triggerId))
        {
            //badguys
            if (tileInfo.Badguy != null)
            {
                var obj = (Transform)Instantiate(tileInfo.Badguy, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset), Quaternion.identity);
                var badguy = obj.GetComponent<Entity>();

                badguy.Died += BadguyDied;

                _numberOfBadguysLeft++;
            }

            //spawn point
            if (tileInfo.IsSpawnPoint)
            {
                Player.transform.position = new Vector2(tileInfo.X + xOffset, tileInfo.Y + yOffset);
            }

            //key stone
            if (tileInfo.IsKeystone)
            {
                _keystoneLocation = new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset);
                _keystone = Instantiate(WallTile, _keystoneLocation, Quaternion.identity);
            }

            //trigger
            if (tileInfo.IsTrigger)
            {
                var triggerLocation = new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset);
                _triggerStone = (GameObject)Instantiate(WallTile, triggerLocation, Quaternion.identity);
                _triggerStone.name = "trigger_" + tileInfo.TriggerId;
            }
        }

        //floor and walls
        foreach (var tileInfo in tiles.Where(tile => tile.BackgroundTile != null && tile.TriggeredById == triggerId))
        {
            if (!tileInfo.IsKeystone)
                Instantiate(tileInfo.BackgroundTile, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset), Quaternion.identity);
        }
    }

    private void BadguyDied(Entity badguy)
    {
        _numberOfBadguysLeft--;
    }
}
