public abstract class Ability
{
    protected string abilityName;

    public string AbilityName { get => abilityName; protected set => abilityName = value; }

    public virtual void DoAbility()
    {
        return;
    }


}
