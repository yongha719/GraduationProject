using System;
using AYellowpaper.SerializedCollections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    [Tooltip("플레이어 이름")] public string PlayerName;

    [Tooltip("테스트")] public bool IsTest;

    [Tooltip("플레이어 무적")] public bool IsPlayerInvincibility;

    [Tooltip("내 코스트"), SerializeField] private uint cost = 3;

    public uint Cost
    {
        get => cost;

        set
        {
            print(value);

            if (value > MaxCost)
            {
                cost = MaxCost;
                return;
            }

            cost = value;
        }
    }

    [Tooltip("내 최대 코스트"), SerializeField] private uint maxCost = 4;

    public uint MaxCost
    {
        get => maxCost;

        set
        {
            if (value > 10)
                return;

            maxCost = value;
        }
    }

    public uint EnemyCost { get; private set; }

    public uint EnemyMaxCost { get; private set; }

    public EInherenceSkillType CommanderInherenceSkillType = EInherenceSkillType.None;

    public CostGaugeUI CostGaugeUI;

    private GameObject LogCanvas;

    protected override void Awake()
    {
        base.Awake();

        LogCanvas = FindObjectOfType<LogManager>()?.gameObject;
    }

    private void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnPlayerTurnAction += IncreaseCost;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F11))
            LogCanvas.SetActive(true);
        else if (Input.GetKey(KeyCode.F2))
        {
            IncreaseCost();
        }
    }

    /// <summary>
    /// 카드 낼 코스트 있는지 확인
    /// </summary>
    public bool CheckCardCostAvailability(uint cost, out Action costDecrease)
    {
        costDecrease = () =>
        {
            Cost -= cost;
            CostGaugeUI.CostGaugeChange();
        };

        return cost <= Cost;
    }

    public void DecreaseCost(uint cost)
    {
        Cost -= cost;
        CostGaugeUI.CostGaugeChange();
    }

    private void IncreaseCost()
    {
        MaxCost++;
        Cost = MaxCost;

        CostGaugeUI.CostGaugeChange(true);
    }

    public static void CostGaugeChange()
    {
        Instance.CostGaugeUI.CostGaugeChange(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)Cost);
            stream.SendNext((int)MaxCost);
        }
        else
        {
            print("코스트 받아옴");

            // uint 로 변환하려면 이렇게 해야댐 object에서 uint로 안되는듯 포톤에서 오류남
            EnemyCost = (uint)(int)stream.PeekNext();
            EnemyMaxCost = (uint)(int)stream.PeekNext();
        }
    }
}