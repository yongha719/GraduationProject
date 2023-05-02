using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Networking;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    public string PlayerName;

    public Dictionary<string, CardData> CardDatas = new Dictionary<string, CardData>();

    private const string CARDDATA_URL = "https://docs.google.com/spreadsheets/d/1y26T7EyZe3DPMKAMjTs2PpUJ34UlDPmmy7k9oEy1Ye0/export?format=tsv&range=A5:I18";

    void Start()
    {
        print("Debug Test");

        StartCoroutine(ERequsetCardData());
    }

    private IEnumerator ERequsetCardData()
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
            string[] data = line[i].Split('\t');

            CardData cardData = new CardData(data);

            string rating = data[8];
            print(rating);

            CardDatas.Add(rating, cardData);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
