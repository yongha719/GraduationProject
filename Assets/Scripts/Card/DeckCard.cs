using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DeckCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Button selectBtn;

    [SerializeField]
    private Button activeMode;

    [SerializeField]
    private GameObject activeUI;

    public CardData data;
    void Start()
    {

    }
}
