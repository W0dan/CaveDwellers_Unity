using UnityEngine;

public class PixelPerfect : MonoBehaviour
{
    public float TextureSize = 100f;

    void Start()
    {
        var unitsPerPixel = 1f / TextureSize;

        Camera.main.orthographicSize = (Screen.height / 2f) * unitsPerPixel;
    }

    void Update()
    {
        
    }
}