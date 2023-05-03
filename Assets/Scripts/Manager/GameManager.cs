using AYellowpaper.SerializedCollections;
using Mono.Cecil;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : SingletonPunCallbacks<GameManager>, IPunObservable
{
    [Tooltip("플레이어 이름")]
    public string PlayerName;

    [Tooltip("테스트")]
    public bool IsTest;

    [Tooltip("카드 데이터들"), SerializedDictionary("Card Rating", "Card Data")]
    public SerializedDictionary<string, CardData> CardDatas = new SerializedDictionary<string, CardData>();

    private const string CARD_DATA_URL = "https://docs.google.com/spreadsheets/d/1y26T7EyZe3DPMKAMjTs2PpUJ34UlDPmmy7k9oEy1Ye0/export?format=tsv&range=A5:I18";

    private const string ASSET_BUNDLE_PATH = "Bundle/card";

    void Start()
    {
        print("Debug Test");

        StartCoroutine(ERequestCardData());
        LoadAssetBundle();
    }

    private IEnumerator ERequestCardData()
    {
        UnityWebRequest request = UnityWebRequest.Get(CARD_DATA_URL);

        // 요청한 데이터를 받을 때까지 기다림
        yield return request.SendWebRequest();

        ParsingCardData(request.downloadHandler.text);
    }

    /// <summary> Request로 받은 데이터를 카드 데이터로 파싱함 </summary>
    void ParsingCardData(string requsetdata)
    {
        string[] line = requsetdata.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            CardData cardData = new CardData(line[i].Split('\t'));

            CardDatas.Add(cardData.CardRating, cardData);
        }
    }

    void LoadAssetBundle()
    {
        // 에셋 번들을 불러옴
        // LoadFromFile은 번들 파일의 경로를 가져옴
        // Assets/ + PATH 로 경로 설정
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, ASSET_BUNDLE_PATH));

        // 불러온 에셋 번들이 없을 때
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        // 제네릭으로 넣은 타입으로 인자로 넣은 이름과 맞는 에셋 번들을 찾아 가져온다.
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("InGame_Card");
        Instantiate(prefab);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
