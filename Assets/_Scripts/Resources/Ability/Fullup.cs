using System;

public class Fullup : Ability
{

    public static event Action OnFullup;

    public static float fullValue = 0.5f;

    public Fullup()
    {
        AbilityName = "Full up";
    }


    public override void DoAbility()
    {
        OnFullup?.Invoke();
    }
}
