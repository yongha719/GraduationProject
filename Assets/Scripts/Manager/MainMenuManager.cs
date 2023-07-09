using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    public Button gameStartBtn;
    public GameObject title;
    public GameObject quitBtn;
    public GameObject inherenceSkillUI;


    [Space(10f)]
    [Header("덱 편성 UIs")]
    public Button deckOrganizationButton;//덱 편성 버튼
    public GameObject deckOrganizationUI;//덱 편성UI
    public Button deckOrganizationQuitBtn;//덱 편성 끄기

    [Space(10f)]
    [Header("설정 UIs")]
    public Button settingBtn;
    public GameObject settingUI;
    public Button settingQuitBtn;

    [SerializeField]
    private TMP_InputField InputName;

    [Header("스킬 선택 UI")]
    [SerializeField, Tooltip("현재 선택되어있는 스킬을 보여주는 버튼")] private Button selectedSkillBtn;
    [SerializeField] private GameObject selectedSkillUI;
    [SerializeField] private List<Button> allSkillButtonList = new List<Button>();
    [SerializeField] private List<Image> selectProduction = new List<Image>();
    

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

        selectedSkillBtn.onClick.AddListener(() =>
        {
            selectedSkillUI.gameObject.SetActive(true);
        });

        
        for (int i = 0;i < allSkillButtonList.Count; i++)
        {
            int a = i;
            allSkillButtonList[a].onClick.AddListener(() =>
            {
               GameManager.Instance.CommanderInherenceSkillType = (EInherenceSkillType)a;
               
                foreach (var item in selectProduction)
                    item.gameObject.SetActive(false);
                
                selectProduction[a].gameObject.SetActive(true);

                selectedSkillUI.gameObject.SetActive(false);

                print((EInherenceSkillType)a);
            });
        }

        #region SettingUI
        settingBtn.onClick.AddListener(() =>
        {
            title.gameObject.SetActive(false);
            settingUI.SetActive(true);
            deckOrganizationButton.gameObject.SetActive(false);
            gameStartBtn.gameObject.SetActive(false);
            settingBtn.gameObject.SetActive(false);
            inherenceSkillUI.SetActive(false);
            quitBtn.gameObject.SetActive(false);
        });

        settingQuitBtn.onClick.AddListener(() =>
        {
            title.gameObject.SetActive(true);
            settingUI.gameObject.SetActive(false);
            deckOrganizationButton.gameObject.SetActive(true);
            gameStartBtn.gameObject.SetActive(true);
            settingBtn.gameObject.SetActive(true);
            inherenceSkillUI.SetActive(true);
            quitBtn.gameObject.SetActive(true);
        });

        #endregion
        #region 덱 편성ButtonUI
        deckOrganizationButton.onClick.AddListener(() =>
        {
            title.gameObject.SetActive(false);
            deckOrganizationUI.SetActive(true);
            deckOrganizationButton.gameObject.SetActive(false);
            gameStartBtn.gameObject.SetActive(false);
            settingBtn.gameObject.SetActive(false);
            inherenceSkillUI.SetActive(false);
            quitBtn.gameObject.SetActive(false);
        });

        deckOrganizationQuitBtn.onClick.AddListener(() =>
        {
            title.gameObject.SetActive(true);
            deckOrganizationUI.SetActive(false);
            deckOrganizationButton.gameObject.SetActive(true);
            gameStartBtn.gameObject.SetActive(true);
            settingBtn.gameObject.SetActive(true);
            inherenceSkillUI.SetActive(true);
            quitBtn.gameObject.SetActive(true);
        });


        #endregion
    }
}