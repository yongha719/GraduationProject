public interface IMasicCardSubject
{
    MasicAbilityTarget AbilityTarget { get; }

    void Ability();
    
    void Ability(UnitCard card);
}
