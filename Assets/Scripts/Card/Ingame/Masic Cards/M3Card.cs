public class M3Card : MasicCard
{
    public override void Ability()
    {
        CardManager.Instance.ShouldSummonCopy = true;
    }
}
