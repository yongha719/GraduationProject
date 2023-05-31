using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceManager : Singleton<ResourceManager>
{

    [Tooltip("카드 데이터 받아올 스프레드시트 링크")]
    private const string CARD_DATA_URL = "https://docs.google.com/spreadsheets/d/1uZHW4YokPwbg9gl0dDWcIjlWeieUlkiMwRk_PvQCPWU/export?format=tsv&range=A3:j16";

    private SerializedDictionary<string, CardData> cardDatas = new();
    
    
    [Tooltip("에셋 번들 받아올 파일 경로")]
    private const string ASSET_BUNDLE_PATH = "Bundle/card";

    private AssetBundle cardAssetBundle;
    

    public async Task<SerializedDictionary<string, CardData>> AsyncRequestCardData()
    {
        var request = UnityWebRequest.Get(CARD_DATA_URL);

        // 비동기 작업의 완료를 나타내는 개체임
        // bool 반환값을 가지는 개체 생성
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        var asyncOp = request.SendWebRequest();
        
        // 받아왔을 때 실행할 이벤트
        asyncOp.completed += operation =>
        {
            tcs.SetResult(request.result == UnityWebRequest.Result.Success);
        };

        // 비동기 작업이 완료될때 까지 대기
        await Task.Run(() => tcs.Task); 
        // 여기서 다음 코드 실행

        return ParsingCardData(request.downloadHandler.text);
    }

    /// <summary> Request로 받은 데이터를 카드 데이터로 파싱해서 반환 </summary>
    private SerializedDictionary<string, CardData> ParsingCardData(string requsetdata)
    {
        var datas = new SerializedDictionary<string, CardData>();

        string[] line = requsetdata.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            CardData cardData = new CardData(line[i].Split('\t'));

            datas.Add(cardData.CardRating, cardData);
        }

        return datas;
    }

    void LoadAssetBundle()
    {
        // 에셋 번들을 불러옴
        // LoadFromFile은 번들 파일의 경로를 가져옴
        // Assets/ + PATH 로 경로 설정
        cardAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, ASSET_BUNDLE_PATH));

        // 불러온 에셋 번들이 없을 때
        if (cardAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
        }
    }

}
