using System.IO;
using System.Threading.Tasks;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Networking;

public class ResourceManager : Singleton<ResourceManager>
{
    [Tooltip("유닛 카드 데이터 받아올 스프레드시트 링크")] private const string UNIT_CARD_DATA_URL =
        "https://docs.google.com/spreadsheets/d/1uZHW4YokPwbg9gl0dDWcIjlWeieUlkiMwRk_PvQCPWU/export?format=tsv&range=A3:J16";

    [Tooltip("마법 카드 데이터 받아올 스프레드시트 링크")] private const string MASIC_CARD_DATA_URL =
        "https://docs.google.com/spreadsheets/d/1uZHW4YokPwbg9gl0dDWcIjlWeieUlkiMwRk_PvQCPWU/export?format=tsv&range=A20:F28";

    [SerializedDictionary("유닛 카드 등급", "유닛 카드 데이터")]
    private SerializedDictionary<string, UnitCardData> unitCardDatas = new();

    private SerializedDictionary<string, UnitCardData> masicCardDatas = new();


    private const string UNIT_CARD_DECK_TEXTURES = "Cards/UnitSprite/Deck";
    private const string UNIT_CARD_FIELD_TEXTURES = "Cards/UnitSprite/Field";
    
    private const string MASIC_CARD_FIELD_TEXTURES = "Cards/MasicSprite";

    [SerializedDictionary("Card Rating", "Card Sprites")]
    private static SerializedDictionary<string, (Sprite deck, Sprite field)> unitCardSprites = new(10);
    private static SerializedDictionary<string, Sprite> masicCardSprites = new();

    protected override void Awake()
    {
        LoadUnitCardSprites();
        LoadMasicCardSprite();
    }

    private void LoadUnitCardSprites()
    {
        var deckTextures = Resources.LoadAll<Texture2D>(UNIT_CARD_DECK_TEXTURES);
        var fieldTextures = Resources.LoadAll<Texture2D>(UNIT_CARD_FIELD_TEXTURES);

        for (int i = 0; i < deckTextures.Length; i++)
        {
            var cardRating = deckTextures[i].name.Split('_')[0];

            Rect rect = new Rect(0, 0, deckTextures[i].width, deckTextures[i].height);
            Sprite deck = Sprite.Create(deckTextures[i], rect, new Vector2(0.5f, 0.5f));

            rect = new Rect(0, 0, fieldTextures[i].width, fieldTextures[i].height);
            Sprite field = Sprite.Create(fieldTextures[i], rect, new Vector2(0.5f, 0.5f));

            unitCardSprites.Add(cardRating, (deck, field));
        }
    }

    private void LoadMasicCardSprite()
    {
        var deckTextures = Resources.LoadAll<Texture2D>(MASIC_CARD_FIELD_TEXTURES);
        
        for (int i = 0; i < deckTextures.Length; i++)
        {
            var cardRating = deckTextures[i].name.Split('_')[0];

            Rect rect = new Rect(0, 0, deckTextures[i].width, deckTextures[i].height);
            Sprite sprite = Sprite.Create(deckTextures[i], rect, new Vector2(0.5f, 0.5f));

            masicCardSprites.Add(cardRating, sprite);
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
    private void ParsingCardData(string requsetdata)
    {
        string[] line = requsetdata.Split('\n');

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
    /// <returns></returns>
    public static (Sprite deck, Sprite field) GetCardSprites(string cardName)
    {
        (Sprite deckTexture, Sprite fieldTexture) textures = (null, null);

        if (unitCardSprites.TryGetValue(cardName, out textures) == false)
        {
            Debug.Assert(false, $"Method : {nameof(GetCardSprites)}\n cardName({cardName})이 이상함");

            return (null, null);
        }
        
        return textures;
    }
}