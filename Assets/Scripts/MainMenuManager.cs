using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Button gameStartBtn;

    [Space(10f)]
    [Header("덱 편성 UIs")]
    public Button deckOrganizationButton;//덱 편성 버튼
    public GameObject deckOrganizationUI;//덱 편성UI

    [Space(10f)]
    [Header("설정 UIs")]
    public Button settingBtn;
    public GameObject settingUI;

    [SerializeField]
    private TMP_InputField InputName;

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
        #region 덱 편성ButtonUI
        deckOrganizationButton.onClick.AddListener(() =>
        {
            deckOrganizationUI.SetActive(true);
        });

        #endregion
    }
}