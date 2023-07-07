using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizationUIProduction : MonoBehaviour
{
    [SerializeField] private RectTransform haveDeckBackGround;
    [SerializeField] private RectTransform selectDeckBackGround;

    private Vector2 haveDeckBackGroundStartPos = new Vector2(-1820, 0);
    private Vector2 haveDeckBackGroundEndPos = new Vector2(-230,0);
    private Vector2 selectDeckBackGroundStartPos = new Vector2(1300, 0);
    private Vector2 selectDeckBackGroundEndPos = new Vector2(730, 0);

    private IEnumerator IActiveOn()
    {

        yield break;
    }

    private IEnumerator IActiveOff()
    {

        yield break;
    }
}
