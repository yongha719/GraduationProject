using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionTest : MonoBehaviour
{
    [SerializeField] private GameObject illustAppearProduction;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(illustAppearProduction);
        }
    }
}
