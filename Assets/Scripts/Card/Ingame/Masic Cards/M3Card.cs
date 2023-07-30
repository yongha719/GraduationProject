public class M3Card : MasicCard
{
    public override void Ability()
    {
        TurnManager.Instance.ShouldSummonCopy = true;
    }
}
