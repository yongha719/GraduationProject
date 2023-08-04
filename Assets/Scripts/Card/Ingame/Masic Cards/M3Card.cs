public class M3Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        CardManager.Instance.ShouldSummonCopy = true;
    }
}
