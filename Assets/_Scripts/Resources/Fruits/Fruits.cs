using UnityEngine;

public class Fruits : ScriptableObject
{
    [SerializeField] private string fruitName;
    [SerializeField] private Ability specialAbility;

    [SerializeField] private float spawnRate;
    [SerializeField] private float decayInitialTime;

    [SerializeField] private float decayMultiplier;
    [SerializeField] private float decayChangeMultiplier;

    [SerializeField] private Sprite fruitImage;

    [SerializeField] private float friendPoint;


    public string FruitName { get => fruitName; set => fruitName = value; }
    public Ability SpecialAbility { get => specialAbility; set => specialAbility = value; }
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }
    public float DecayInitialTime { get => decayInitialTime; }
    public float DecayChangeMultiplier { get => decayChangeMultiplier; }
    public Sprite FruitImage { get => fruitImage; }
    public float FriendPoint { get => friendPoint; set => friendPoint = value; }
    public float DecayMultiplier { get => decayMultiplier; set => decayMultiplier = value; }
}

