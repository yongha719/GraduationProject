using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum EInherenceSkillType
{
    PhotonShield,//광자 보호막
    PistolShot,//권총 발사
    Barrier,//방벽
    EmergencyCall,//긴급호출
    ChangeMood,//분위기 전환
    FastCharger,//급속충전기
    End,
}

public class MainMenuManager : MonoBehaviour
{
    public Button gameStartBtn;

    public EInherenceSkillType inherenceSkillType = EInherenceSkillType.PhotonShield;

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
                inherenceSkillType = (EInherenceSkillType)a;
                for (int j = 0; j < selectProduction.Count; j++)
                {
                    selectProduction[j].gameObject.SetActive(false);
                }
                selectProduction[a].gameObject.SetActive(true);

                selectedSkillUI.gameObject.SetActive(false);

                print((EInherenceSkillType)a);
            });
        }

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