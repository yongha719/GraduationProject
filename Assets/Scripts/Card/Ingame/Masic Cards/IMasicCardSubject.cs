public interface IMasicCardSubject
{
    MasicAbilityTarget AbilityTarget { get; }

    
    void Ability(UnitCard card);
}
