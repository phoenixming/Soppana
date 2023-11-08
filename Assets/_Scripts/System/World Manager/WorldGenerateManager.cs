using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerateManager : StaticInstance<WorldGenerateManager>
{
    [SerializeField] private Sprite tile;
    [SerializeField] private GameObject spawnParentObjectLocation;

    [SerializeField] private int worldSize = 100;
    [SerializeField] private float noiseFreq = 0.05f;

    private Texture2D noiseTexture;

    public int WorldSize { get => worldSize; set => worldSize = value; }

    // Start is called before the first frame update

    void Start()
    {
        //GenerateNoiseTexture();
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void GenerateMap()
    {
        for (int x = -1 * worldSize / 2; x < worldSize / 2; x++)
        {
            for (int y = -1 * worldSize / 2; y < worldSize / 2; y++)
            {
                GameObject newObject = new GameObject(name = "Tile");
                newObject.AddComponent<SpriteRenderer>();
                newObject.GetComponent<SpriteRenderer>().sprite = tile;
                newObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.7f, 0.3f);
                newObject.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                newObject.transform.parent = spawnParentObjectLocation.transform;
            }
        }
    }

    private void GenerateNoiseTexture()
    {
        noiseTexture = new Texture2D(WorldSize, WorldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float v = Mathf.PerlinNoise(x * noiseFreq, y * noiseFreq);
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }

        noiseTexture.Apply();
    }
}
