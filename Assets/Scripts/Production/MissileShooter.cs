using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    public GameObject missilPrefab; // 미사일 프리팹.
    private GameObject target; // 도착 지점.

    [Header("미사일 기능 관련")]
    public float spd = 2; // 미사일 속도.
    [Space(10f)]
    public float distanceFromStart = 6.0f; // 시작 지점을 기준으로 얼마나 꺾일지.
    public float distanceFromEnd = 3.0f; // 도착 지점을 기준으로 얼마나 꺾일지.
    [Space(10f)]
    public int shotCount = 3; // 총 몇 개 발사할건지.
    [Range(0, 1)] public float interval = 0.15f;
    public int m_shotCountEveryInterval = 2; // 한번에 몇 개씩 발사할건지.

    public IEnumerator ICreateMissile()
    {
        int _shotCount = shotCount;
        while (_shotCount > 0)
        {
            for (int i = 0; i < m_shotCountEveryInterval; i++)
            {
                if (_shotCount > 0)
                {
                    GameObject missile = Instantiate(missilPrefab);
                    missile.GetComponent<BezierMissile>().Init(transform, target.transform, spd, distanceFromStart, distanceFromEnd);

                    _shotCount--;
                }
            }
            yield return new WaitForSeconds(interval);
        }
        yield return null;
    }
}