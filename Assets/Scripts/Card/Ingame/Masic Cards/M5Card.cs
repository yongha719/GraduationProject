
public class M5Card : MasicCard
{
    public override void Ability(UnitCard card = null)
    {
        CardManager.Instance.UseMineMasic = true;   
    }
}
