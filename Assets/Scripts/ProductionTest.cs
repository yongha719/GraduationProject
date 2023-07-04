using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProductionTest : MonoBehaviour
{
    [SerializeField] private CharacterProduction illustAppearProduction;
    void Update()
    {
        #region 테스트 코드
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnAppearEffect(ECharacterType.Baekyura);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnAppearEffect(ECharacterType.CleaningRobot);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnAppearEffect(ECharacterType.Hanseorin);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnAppearEffect(ECharacterType.Leesooha);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpawnAppearEffect(ECharacterType.Kangsebin);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpawnAppearEffect(ECharacterType.Yuki);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpawnAppearEffect(ECharacterType.Yooeunha);
        }
        #endregion
    }

    private void SpawnAppearEffect(ECharacterType type)
    {
        CharacterProduction product = Instantiate(illustAppearProduction);
        product.characterType = type;
    }
}
