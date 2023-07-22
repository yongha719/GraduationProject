using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    /// <summary>
    /// 부모 게임오브젝트의 SetActive를 조정할 수 있음
    /// </summary>
    public static void ParentObjectSetActive(this MonoBehaviour @object, bool value)
    {
        @object.transform.parent.gameObject.SetActive(value);
    }
}
