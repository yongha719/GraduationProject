using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastChargerSkill : InherenceSkill
{
    private WaitUntil waitClick = new WaitUntil(() => Input.GetMouseButtonDown(0));

    public override EInherenceSkillType InherenceSkillType => EInherenceSkillType.FastCharger;

    [SerializeField]
    private int HealAmount;
    
    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];
    
    protected override void Skill()
    {
        StartCoroutine(EFastChargerSkill());
    }
    
    private IEnumerator EFastChargerSkill()
    {
        yield return waitClick;
        
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is null ||
                hit.collider.TryGetComponent(out UnitCard card) == false ||
                card.IsEnemy)
                continue;

            // 카드한테 보호막주는 코드싸기
            card.Hp += HealAmount;
            
            enableAttackCall();
        }
    }
}
