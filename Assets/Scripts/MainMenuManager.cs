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
    public Button deckUIquit;

    [Space(10f)]
    [Header("���� UIs")]
    public Button settingBtn;
    public GameObject settingUI;
    public Button settingUIquit;

    public Button gameQuit;

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

        });

        settingUIquit.onClick.AddListener(() =>
        {

        });
        #endregion
        #region �� ��ButtonUI
        deckOrganizationButton.onClick.AddListener(() =>
        {
            deckOrganizationUI.SetActive(true);
        });

        deckUIquit.onClick.AddListener(() =>
        {
            deckOrganizationUI.SetActive(false);
        });
        #endregion
    }
}