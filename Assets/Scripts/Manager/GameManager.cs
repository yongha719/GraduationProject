using AYellowpaper.SerializedCollections;
using AYellowpaper.SerializedCollections.Editor;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Networking;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    public string PlayerName;

    public bool IsTest;

    [SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new SerializedDictionary<string, CardData>();

    private const string CARDDATA_URL = "https://docs.google.com/spreadsheets/d/1y26T7EyZe3DPMKAMjTs2PpUJ34UlDPmmy7k9oEy1Ye0/export?format=tsv&range=A5:I18";

    void Start()
    {
        print("Debug Test");

        StartCoroutine(ERequestCardData());
    }

    private IEnumerator ERequestCardData()
    {
        UnityWebRequest request = UnityWebRequest.Get(CARDDATA_URL);

        yield return request.SendWebRequest();

        ParsingCardData(request.downloadHandler.text);
    }

    void ParsingCardData(string requsetdata)
    {
        string[] line = requsetdata.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            CardData cardData = new CardData(line[i].Split('\t'));

            CardDatas.Add(cardData.CardRating, cardData);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
