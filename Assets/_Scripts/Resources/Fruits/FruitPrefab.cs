using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FruitPrefab : MonoBehaviour
{
    public static event Action<FruitPrefab> DecayFruit;

    [SerializeField] private Fruits fruit;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Fruits Fruit { get => fruit; set => fruit = value; }
    public bool IsStored { get => isStored; set => isStored = value; }
    public float TimeCount { get => timeCount; set => timeCount = value; }
    public State FruitState { get => fruitState; set => fruitState = value; }

    private float timeCount = 0;
    private bool isStored = false;

    private float decayMultiplier = 1;

    public enum State
    {
        Green,
        Yellow,
        Red
    }

    private State fruitState = State.Green;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = fruit.FruitImage;
    }


    void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            return;
        }
        if (GameManager.Instance.KarmaScore >= 50)
        {
            decayMultiplier = 0.8f;
            if (GameManager.Instance.KarmaScore >= 100)
            {
                decayMultiplier = 0.6f;
            }
        }
        else
        {
            decayMultiplier = 1.2f;
        }

        if (!IsStored)
        {
            TimeCount += Time.deltaTime * fruit.DecayMultiplier;
            if (TimeCount * decayMultiplier >= fruit.DecayInitialTime)
            {
                SpawnManager.Instance.SpawnPoints.Remove(this.transform.position);
                Destroy(gameObject);
            }
        }
        else
        {
            TimeCount += Time.deltaTime * fruit.DecayChangeMultiplier;
            if (TimeCount * decayMultiplier >= fruit.DecayInitialTime)
            {
                OnDecayFruit();
                SpawnManager.Instance.SpawnPoints.Remove(this.transform.position);
                Destroy(gameObject);
            }
        }

        if (TimeCount <= fruit.DecayInitialTime / 3f)
        {
            spriteRenderer.color = Color.green;
            FruitState = State.Green;
        }
        else if (TimeCount <= 2 * fruit.DecayInitialTime / 3f)
        {
            spriteRenderer.color = Color.yellow;
            FruitState = State.Yellow;
        }
        else
        {
            spriteRenderer.color = Color.red;
            FruitState = State.Red;
        }
    }


    private void OnDecayFruit()
    {
        DecayFruit?.Invoke(this);
    }
}
