using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button gameStartBtn;

    [Space(10f)]
    [Header("�� �� UIs")]
    public Button deckOrganizationButton;//�� �� ��ư
    public GameObject deckOrganizationUI;//�� ��UI

    [Space(10f)]
    [Header("���� UIs")]
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
        #region �� ��ButtonUI
        deckOrganizationButton.onClick.AddListener(() =>
        {
            deckOrganizationUI.SetActive(true);
        });

        #endregion
    }
}