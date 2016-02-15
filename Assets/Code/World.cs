using System.Collections.Generic;
using System.Linq;
using Assets.Code;
using UnityEngine;
using Assets.Code.Entities;
using Assets.Code.Entities.Player;

public class World : MonoBehaviour
{
    public Player Player;

    public Transform BadGuy1;
    public Transform BadGuy2;

    public List<GameObject> FloorTiles;
    public List<GameObject> WallTiles;

    public GameObject HealthPickup;
    public GameObject AmmoPickup;

    public Canvas GameMenu;
    public Canvas StageCompleteCanvas;

    public List<Texture2D> LevelTextures;

    private Texture2D _levelTexture;
    private int _levelIndex = 0;
    private int _currentLevelXOffset = 0;
    private int _currentLevelYOffset = 0;

    private int _numberOfBadguysLeft = 0;
    private Object _keystone;
    private Vector3 _keystoneLocation;

    private LevelParser _levelParser;
    private List<TileInfo> _currentLevelTiles;

    private readonly List<int> _discoveredTriggerIds = new List<int> { 0 };

    private int _dropCounter = 0;

    internal bool LevelFinished
    {
        get { return _numberOfBadguysLeft <= 0; }
    }

    void Start()
    {
        //todo: put this in MenuManager ?//
        Time.timeScale = 1;

        var stageComplete = StageCompleteCanvas.GetComponent<CanvasGroup>();

        stageComplete.alpha = 0x00;
        stageComplete.blocksRaycasts = false;

        var gameMenu = GameMenu.GetComponent<CanvasGroup>();
        gameMenu.alpha = 0x00;
        gameMenu.blocksRaycasts = false;
        //end todo//

        Player.HitSomething += Player_HitSomethingWithGun;

        _levelTexture = LevelTextures[_levelIndex];

        _levelParser = new LevelParser
        {
            GrassTile = FloorTiles[0],
            GrayTile = FloorTiles[1],
            GrayTileWithGrass = FloorTiles[2],
            GrayTileWithGrassRight = FloorTiles[3],
            GrayTileWithGrassTop = FloorTiles[4],
            GrayTileWithGrassLeft = FloorTiles[5],
            WallTile = WallTiles[0],
            FountainTile = WallTiles[1],
            KnightStatueTile = WallTiles[2],
            BadGuy1 = BadGuy1,
            BadGuy2 = BadGuy2,
            AmmoPickup = AmmoPickup,
            HealthPickup = HealthPickup,
        };
        _currentLevelTiles = _levelParser.GetLevelData(_levelTexture).ToList();
        LoadLevel(_currentLevelTiles, _currentLevelXOffset);
    }

    private void Player_HitSomethingWithGun(GameObject obj)
    {
        if (obj.name.StartsWith("trigger"))
        {
            var triggerId = int.Parse(obj.name.Substring(8));

            Destroy(obj);

            _discoveredTriggerIds.Add(triggerId);
            LoadLevel(_currentLevelTiles, _currentLevelXOffset, _currentLevelYOffset, triggerId);
        }

        if (obj.tag == "Badguy")
        {
            var badguy = obj.GetComponent<Entity>();

            badguy.TakeHealth(10);
        }
    }

    void Update()
    {
        var gameMenuCanvasGroup = GameMenu.GetComponent<CanvasGroup>();
        if (Input.GetKey(KeyCode.Escape) && !MenuManager.IsPaused && MenuManager.IsHidden)
        {
            MenuManager.ShowMenu(gameMenuCanvasGroup);
        }
        else if (Input.GetKey(KeyCode.Escape) && MenuManager.IsShown)
        {
            MenuManager.ResumeGame(gameMenuCanvasGroup);
        }

        if (_numberOfBadguysLeft <= 0)
        {
            _levelIndex++;
            if (!(LevelTextures.Count > _levelIndex))
            {
                MenuManager.ShowStageComplete(StageCompleteCanvas);

                return;
            }
            var nextLevelLocation = _currentLevelTiles.Single(tile => tile.IsKeystone).NextLevelLocation;
            _levelTexture = LevelTextures[_levelIndex];

            //finished, open exit ...
            Destroy(_keystone);
            Instantiate(FloorTiles[0], _keystoneLocation, Quaternion.identity);

            //... and draw new level
            _currentLevelXOffset += nextLevelLocation.X;
            _currentLevelYOffset += nextLevelLocation.Y;

            var newLevelTiles = _levelParser.GetLevelData(_levelTexture).ToList();
            _currentLevelTiles = _currentLevelTiles //make sure that we keep hidden tiles, that can still be triggered !!
                .Where(tile => _discoveredTriggerIds.All(id => tile.TriggeredById != id))
                .Select(tile =>
                {
                    tile.X -= nextLevelLocation.X;
                    tile.Y -= nextLevelLocation.Y;
                    return tile;
                }).ToList();
            _currentLevelTiles.AddRange(newLevelTiles);

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
                var obj = (Transform)Instantiate(tileInfo.Badguy, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 51), Quaternion.identity);
                var badguy = obj.GetComponent<Entity>();

                badguy.Died += BadguyDied;

                _numberOfBadguysLeft++;
            }

            //pickups
            if (tileInfo.Pickup != null)
            {
                Instantiate(tileInfo.Pickup, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 50), Quaternion.identity);
            }

            //ornaments
            if (tileInfo.Ornament != null)
            {
                Instantiate(tileInfo.Ornament, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 50), Quaternion.identity);
            }

            //spawn point
            if (tileInfo.IsSpawnPoint)
            {
                Player.transform.position = new Vector2(tileInfo.X + xOffset, tileInfo.Y + yOffset);
            }

            //key stone
            if (tileInfo.IsKeystone)
            {
                _keystoneLocation = new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 50);
                _keystone = Instantiate(WallTiles[0], _keystoneLocation, Quaternion.identity);
            }

            //trigger
            if (tileInfo.IsTrigger)
            {
                var triggerLocation = new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset);
                var triggerStone = (GameObject)Instantiate(WallTiles[0], triggerLocation, Quaternion.identity);
                triggerStone.name = "trigger_" + tileInfo.TriggerId;
            }

            if (tileInfo.BackgroundTile != null && !tileInfo.IsKeystone)
                Instantiate(tileInfo.BackgroundTile, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 0), Quaternion.identity);
        }

        ////floor and walls
        //foreach (var tileInfo in tiles.Where(tile => tile.BackgroundTile != null && tile.TriggeredById == triggerId))
        //{
        //    if (!tileInfo.IsKeystone)
        //        Instantiate(tileInfo.BackgroundTile, new Vector3(tileInfo.X + xOffset, tileInfo.Y + yOffset, 0), Quaternion.identity);
        //}
    }

    private void BadguyDied(Entity badguy)
    {
        var obj = badguy.gameObject;
        Destroy(obj);

        _dropCounter++;

        if (_dropCounter == 5 || _dropCounter == 10)
        {
            var dropLocation = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

            Instantiate(AmmoPickup, dropLocation, Quaternion.identity);
        }
        if (_dropCounter == 15)
        {
            var dropLocation = new Vector3(obj.transform.position.x, obj.transform.position.y, -5);

            Instantiate(HealthPickup, dropLocation, Quaternion.identity);
            _dropCounter = 0;
        }

        _numberOfBadguysLeft--;
    }
}
