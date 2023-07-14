using AYellowpaper.SerializedCollections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// 이 스크립트가 하는 역할===
// 유닛, 마법 카드 데이터들 불러오고 관리
// 유닛, 마법 카드 스프라이트 관리

public class ResourceManager : Singleton<ResourceManager>
{
    [Tooltip("유닛 카드 데이터 받아올 스프레드시트 링크")]
    private const string UNIT_CARD_DATA_URL =
        "https://docs.google.com/spreadsheets/d/1uZHW4YokPwbg9gl0dDWcIjlWeieUlkiMwRk_PvQCPWU/export?format=tsv&range=A3:J16";

    [Tooltip("마법 카드 데이터 받아올 스프레드시트 링크")]
    private const string MASIC_CARD_DATA_URL =
        "https://docs.google.com/spreadsheets/d/1uZHW4YokPwbg9gl0dDWcIjlWeieUlkiMwRk_PvQCPWU/export?format=tsv&range=A20:F28";

    [SerializedDictionary("유닛 카드 등급", "유닛 카드 데이터")]
    private SerializedDictionary<string, UnitCardData> unitCardDatas = new();

    private SerializedDictionary<string, MasicCardData> masicCardDatas = new();


    private const string UNIT_CARD_DECK_TEXTURES = "Cards/UnitSprite/Deck";
    private const string UNIT_CARD_FIELD_TEXTURES = "Cards/UnitSprite/Field";

    private const string MASIC_CARD_FIELD_TEXTURES = "Cards/MasicSprite";

    [SerializedDictionary("Card Rating", "Card Sprites")]
    private static SerializedDictionary<string, (Sprite deck, Sprite field)> unitCardSprites = new(10);

    private static SerializedDictionary<string, Sprite> masicCardSprites = new();

    protected override void Awake()
    {
        LoadUnitCardSprites();
        LoadMasicCardSprites();
    }

    private void LoadUnitCardSprites()
    {
        var deckSprites = Resources.LoadAll<Sprite>(UNIT_CARD_DECK_TEXTURES);
        var fieldSprites = Resources.LoadAll<Sprite>(UNIT_CARD_FIELD_TEXTURES);

        for (int i = 0; i < deckSprites.Length; i++)
        {
            var cardRating = deckSprites[i].name.Split('_')[0];

            unitCardSprites.Add(cardRating, (deckSprites[i], fieldSprites[i]));
        }
    }

    private void LoadMasicCardSprites()
    {
        var deckTextures = Resources.LoadAll<Sprite>(MASIC_CARD_FIELD_TEXTURES);

        for (int i = 0; i < deckTextures.Length; i++)
        {
            var cardRating = deckTextures[i].name.Split('_')[0];

            masicCardSprites.Add(cardRating, deckTextures[i]);
        }
    }

    public async Task<SerializedDictionary<string, UnitCardData>> AsyncRequestCardData()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Assert(false, "인터넷 연결안됨");
        }

        var request = UnityWebRequest.Get(UNIT_CARD_DATA_URL);

        // 비동기 작업의 완료를 나타내는 개체임
        // bool 반환값을 가지는 개체 생성
        var tcs = new TaskCompletionSource<bool>();

        var asyncOp = request.SendWebRequest();

        // 받아왔을 때 실행할 이벤트
        asyncOp.completed += operation => tcs.SetResult(request.result == UnityWebRequest.Result.Success);

        // 비동기 작업이 완료될때 까지 대기
        await Task.Run(() => tcs.Task);
        // 여기서 다음 코드 실행

        ParsingCardData(request.downloadHandler.text);

        return unitCardDatas;
    }

    /// <summary> Request로 받은 데이터를 카드 데이터로 파싱해서 반환 </summary>
    private void ParsingCardData(string requsetText)
    {
        string[] line = requsetText.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            var cardData = new UnitCardData(line[i].Split('\t'));

            unitCardDatas.Add(cardData.CardRating, cardData);
        }
    }

    /// <summary>
    /// Card의 Deck Sprite와 Field Sprite를 반환
    /// </summary>
    /// <param name="cardName"></param>
    public static (Sprite deck, Sprite field) GetUnitCardSprites(string cardName)
    {
        (Sprite deckTexture, Sprite fieldTexture) sprites = (null, null);

        if (unitCardSprites.TryGetValue(cardName, out sprites) == false)
        {
            Debug.Assert(false, $"Method : {nameof(GetUnitCardSprites)}\n cardName({cardName})이 이상함");

            return (null, null);
        }

        return sprites;
    }

    public static Sprite GetMasicCardSprites(string cardName)
    {
        Sprite sprite = null;

        if (masicCardSprites.TryGetValue(cardName, out sprite) == false)
        {
            Debug.Assert(false, $"Method : {nameof(GetMasicCardSprites)}\n cardName({cardName})이 이상함");

            return null;
        }

        return sprite;
    }
}