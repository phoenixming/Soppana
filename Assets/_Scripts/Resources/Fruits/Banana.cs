using UnityEngine;

[CreateAssetMenu(menuName = "Fruits/Banana")]
public class Banana : Fruits
{
    public Banana()
    {
        FruitName = "Banana";
        SpecialAbility = new Fullup();
        FriendPoint = 10;
        SpawnRate = Fullup.fullValue * 10;
    }
}
