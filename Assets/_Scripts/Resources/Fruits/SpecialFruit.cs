using UnityEngine;

[CreateAssetMenu(menuName = "Fruits/SpecialFruit")]
public class SpecialFruit : Fruits
{
    public SpecialFruit()
    {
        FruitName = "SpecialFruit";
        SpecialAbility = new SpecialAbility();
        FriendPoint = 30;
    }
}
