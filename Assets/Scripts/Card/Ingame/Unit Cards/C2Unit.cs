
public class C2Unit : UnitCard
{
    protected override void Start()
    {
        CardState = CardState.Field;

        cardInfo.OnFieldStateChange();
    }
}
