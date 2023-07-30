using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonShieldSkill : InherenceSkill
{
    private WaitUntil waitClick = new WaitUntil(() => Input.GetMouseButtonDown(0));
    
    public override EInherenceSkillType InherenceSkillType => EInherenceSkillType.PhotonShield;

    private RaycastHit2D[] raycastHits = new RaycastHit2D[10];
    
    protected override void Skill()
    {
        StartCoroutine(EPhotonShieldSkill());
    }

    private IEnumerator EPhotonShieldSkill()
    {
        yield return waitClick;
        
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Physics2D.RaycastNonAlloc(worldPosition, Vector2.zero, raycastHits);

        foreach (var hit in raycastHits)
        {
            if (hit.collider is null ||
                hit.collider.TryGetComponent(out UnitCard card) == false ||
                card.IsMine == false)
                continue;

            // 카드한테 보호막주는 코드싸기
            print("보호막");
            
            enableAttackCall();
        }
    }
}