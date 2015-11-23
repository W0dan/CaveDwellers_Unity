using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform FloorTile;
    public Transform WallTile;

    public Color FloorColor;
    public Color WallColor;
    public Color SpawnPointColor;
    public Color BadGuyColor;

    public Texture2D LevelTexture;

    public Entity Player;

    public Transform BadGuy;

    void Start()
    {
        LoadLevel(LevelTexture.width, LevelTexture.height);
    }

    void Update()
    {

    }

    private void LoadLevel(int width, int height)
    {
        var tileColors = LevelTexture.GetPixels();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentTileColor = tileColors[x + y * width];

                if (currentTileColor == BadGuyColor)
                {
                    Instantiate(BadGuy, new Vector3(x, y), Quaternion.identity);
                }
            }
        }

        //always do background last, because what you draw later dissapears behind it.
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var currentTileColor = tileColors[x + y * width];

                if (currentTileColor == FloorColor
                    || currentTileColor == SpawnPointColor
                    || currentTileColor == BadGuyColor)
                {
                    Instantiate(FloorTile, new Vector3(x, y), Quaternion.identity);
                }
                if (currentTileColor == WallColor)
                {
                    Instantiate(WallTile, new Vector3(x, y), Quaternion.identity);
                }
                if (currentTileColor == SpawnPointColor)
                {
                    var startPosition = new Vector2(x, y);
                    Player.transform.position = startPosition;
                }
            }
        }
    }
}
