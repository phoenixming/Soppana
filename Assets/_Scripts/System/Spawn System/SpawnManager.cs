using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : StaticInstance<SpawnManager>
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject spawnParentObjectLocation;
    [SerializeField] private GameObject[] spawnPrefabs;
    [SerializeField] private GameObject[] npcs;
    [SerializeField] private int worldSize = 50;

    private Dictionary<Fruits, float> fruitPair;
    private Fruits tempFruit;
    private int randomX;
    private int randomY;

    private List<Vector2> spawnPoints;

    public List<Vector2> SpawnPoints { get => spawnPoints; set => spawnPoints = value; }

    private Dictionary<Fruits, float> spawnMultiplier;

    private void Start()
    {
        spawnMultiplier = new Dictionary<Fruits, float>();

        fruitPair = new Dictionary<Fruits, float>();
        SpawnPoints = new List<Vector2>();
        foreach (GameObject npc in npcs)
        {
            SpawnPoints.Add(new Vector2(npc.transform.position.x - 0.5f, npc.transform.position.y - 0.5f));
        }
        foreach (GameObject prefab in spawnPrefabs)
        {
            tempFruit = prefab.GetComponent<FruitPrefab>().Fruit;
            fruitPair.Add(tempFruit, 0);
            spawnMultiplier.Add(tempFruit, 1);
        }
    }


    private void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            return;
        }


        foreach (GameObject prefab in spawnPrefabs)
        {
            tempFruit = prefab.GetComponent<FruitPrefab>().Fruit;
            fruitPair[tempFruit] += Time.deltaTime;

            if (tempFruit.FruitName == "Banana")
            {
                spawnMultiplier[tempFruit] = Mathf.Max(0.4f, uiManager.HealthSlider.value);
            }

            if (fruitPair[tempFruit] > tempFruit.SpawnRate * spawnMultiplier[tempFruit])
            {
                randomX = Random.Range(-1 * worldSize / 2, worldSize / 2);
                randomY = Random.Range(-1 * worldSize / 2, worldSize / 2);

                while (SpawnPoints.Contains(new Vector2(randomX, randomY)))
                {
                    randomX = Random.Range(-1 * worldSize / 2, worldSize / 2);
                    randomY = Random.Range(-1 * worldSize / 2, worldSize / 2);
                }
                Instantiate(prefab, new Vector2(randomX + 0.5f, randomY + 0.5f), Quaternion.identity, spawnParentObjectLocation.transform);
                fruitPair[tempFruit] = 0;
                SpawnPoints.Add(new Vector2(randomX, randomY));
            }
        }
    }
}
