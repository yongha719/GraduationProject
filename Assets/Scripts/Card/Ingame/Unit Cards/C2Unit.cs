
public class C2Unit : UnitCard
{
    protected override void Start()
    {
        CardState = CardState.Field;
    }

    // 사령관 고유스킬 땜에 만들었음
    public void SetStat(int hp, int power)
    {
        CardData.Hp = 1;
        CardData.Power = 1;
    }
}
