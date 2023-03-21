using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitBtn : MonoBehaviour
{
    private Button quitBtn;


    [SerializeField]
    private GameObject obj;
    void Start()
    {
        quitBtn = GetComponent<Button>();

        quitBtn.onClick.AddListener(() =>
        {
            obj.SetActive(false);
        });

    }

}
