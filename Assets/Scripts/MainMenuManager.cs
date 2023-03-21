using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button gameStartBtn;

    [Space(10f)]
    [Header("µ¦ Æí¼º UIs")]
    public Button deckOrganizationButton;//µ¦ Æí¼º ¹öÆ°
    public GameObject deckOrganizationUI;//µ¦ Æí¼ºUI

    [Space(10f)]
    [Header("¼³Á¤ UIs")]
    public Button settingBtn;
    public GameObject settingUI;


    void Start()
    {
        AddListener();
    }

    private void AddListener()
    {
        gameStartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Card");
        });

        #region SettingUI
        settingBtn.onClick.AddListener(() =>
        {
            settingUI.SetActive(true);
        });

        #endregion
        #region µ¦ Æí¼ºButtonUI
        deckOrganizationButton.onClick.AddListener(() =>
        {
            deckOrganizationUI.SetActive(true);
        });

        #endregion
    }
}