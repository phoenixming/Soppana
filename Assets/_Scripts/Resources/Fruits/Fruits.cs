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

    [SerializeField] private bool hasStages;
    [SerializeField] private Sprite stage2Image;
    [SerializeField] private Sprite stage3Image;


    public string FruitName { get => fruitName; set => fruitName = value; }
    public Ability SpecialAbility { get => specialAbility; set => specialAbility = value; }
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }
    public float DecayInitialTime { get => decayInitialTime; }
    public float DecayChangeMultiplier { get => decayChangeMultiplier; }
    public Sprite FruitImage { get => fruitImage; }
    public float FriendPoint { get => friendPoint; set => friendPoint = value; }
    public float DecayMultiplier { get => decayMultiplier; set => decayMultiplier = value; }
    public bool HasStages { get => hasStages; set => hasStages = value; }
    public Sprite Stage2Image { get => stage2Image;}
    public Sprite Stage3Image { get => stage3Image;}
}

