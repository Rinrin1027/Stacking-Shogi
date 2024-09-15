using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// 駒の移動情報を格納するクラス
[System.Serializable]
public class PieceMove
{
    public int x;
    public int y;
    public int 距離;
}

// 駒データを格納するクラス
[System.Serializable]
public class ShogiPieceData
{
    public string 名前;
    public List<PieceMove> 移動;
    public List<string>成り;  // このフィールドで成り先の駒名を指定
}

// JSON全体を格納するクラス
[System.Serializable]
public class ShogiPieceDictionary
{
    public Dictionary<string, ShogiPieceData> 駒;
}


public class ShogiPieceManager : MonoBehaviour
{
    // 成る前の駒の名前    
    public Dictionary<string, string> toNormal;

    // 駒のPrefabを設定する
    public GameObject pawnPrefab;   // 歩のPrefab
    public GameObject rookPrefab;   // 飛車のPrefab
    public GameObject lancePrefab;  // 香車のPrefab 
    public GameObject knightPrefab; // 桂馬のPrefab
    public GameObject bishopPrefab; // 角行のPrefab
    public GameObject goldPrefab;   // 金のPrefab
    public GameObject silverPrefab; // 銀のPrefab
    public GameObject kingPrefab;   // 王のPrefab
    public GameObject promotedPawnPrefab;  // と金のPrefab
    public GameObject promotedRookPrefab;  // 竜王のPrefab
    public GameObject promotedBishopPrefab; // 竜馬のPrefab
    public GameObject promotedLancePrefab;  // 成香のPrefab
    public GameObject promotedKnightPrefab; // 成桂のPrefab
    public GameObject promotedSilverPrefab; // 成銀のPrefab
    public GameObject huhyoukyousyaPrefab; // 歩兵香車のPrefab
    public GameObject tokinkyousyaPrefab; // と金香車のPrefab
    public GameObject huhyounarikyouPrefab; // 歩兵成香のPrefab
    public GameObject tokinnarikyouPrefab; // と金成香のPrefab
    public GameObject huhyoukeimaPrefab; // 歩兵桂馬のPrefab
    public GameObject tokinkeimaPrefab; // と金桂馬のPrefab
    public GameObject huhyounarikeiPrefab; // 歩兵成桂のPrefab
    public GameObject tokinnarikeiPrefab; // と金成桂のPrefab
    public GameObject kyousyakeimaPrefab; // 香車桂馬のPrefab
    public GameObject narikyoukeimaPrefab; // 成香桂馬のPrefab
    public GameObject kyousyanarikeiPrefab; // 香車成桂のPrefab
    public GameObject narikyounarikeiPrefab; // 成香成桂のPrefab
    public GameObject huhyouginsyouPrefab; // 歩兵銀将のPrefab
    public GameObject tokingunsyouPrefab; // と金銀将のPrefab
    public GameObject huhyounariginPrefab; // 歩兵成銀のPrefab
    public GameObject tokinnariginPrefab; // と金成銀のPrefab
    public GameObject kyousyaginsyouPrefab; // 香車銀将のPrefab
    public GameObject narikyouginsyouPrefab; // 成香銀将のPrefab
    public GameObject kyousyanariginPrefab; // 香車成銀のPrefab
    public GameObject narikyounariginPrefab; // 成香成銀のPrefab
    public GameObject keimaginsyouPrefab; // 桂馬銀将のPrefab
    public GameObject narikeiginsyouPrefab; // 成桂銀将のPrefab
    public GameObject keimanariginPrefab; // 桂馬成銀のPrefab
    public GameObject narikeinariginPrefab; // 成桂成銀のPrefab
    public GameObject huhyoukinsyouPrefab; // 歩兵金将のPrefab
    public GameObject tokinkinsyouPrefab; // と金金将のPrefab
    public GameObject kyousyakinsyouPrefab; // 香車金将のPrefab
    public GameObject narikyoukinsyouPrefab; // 成香金将のPrefab
    public GameObject keimakinsyouPrefab; // 桂馬金将のPrefab
    public GameObject narikeikinsyouPrefab; // 成桂金将のPrefab
    public GameObject ginsyoukinsyouPrefab; // 銀将金将のPrefab
    public GameObject nariginkinsyouPrefab; // 成銀金将のPrefab
    public GameObject huhyoukakugyouPrefab; // 歩兵角行のPrefab
    public GameObject tokinkakugyouPrefab; // と金角行のPrefab
    public GameObject huhyouryumaPrefab; // 歩兵竜馬のPrefab
    public GameObject tokinryumaPrefab; // と金竜馬のPrefab
    public GameObject kyousyakakugyouPrefab; // 香車角行のPrefab
    public GameObject narikyoukakugyouPrefab; // 成香角行のPrefab
    public GameObject kyousyaryumaPrefab; // 香車竜馬のPrefab
    public GameObject narikyouryumaPrefab; // 成香竜馬のPrefab
    public GameObject keimakakugyouPrefab; // 桂馬角行のPrefab
    public GameObject narikeikakugyouPrefab; // 成桂角行のPrefab
    public GameObject keimaryumaPrefab; // 桂馬竜馬のPrefab
    public GameObject narikeiryumaPrefab; // 成桂竜馬のPrefab
    public GameObject ginsyoukakugyouPrefab; // 銀将角行のPrefab
    public GameObject nariginkakugyouPrefab; // 成銀角行のPrefab
    public GameObject ginsyouryumaPrefab; // 銀将竜馬のPrefab
    public GameObject nariginryumaPrefab; // 成銀竜馬のPrefab
    public GameObject kinsyoukakugyouPrefab; // 金将角行のPrefab
    public GameObject kinsyouryumaPrefab; // 金将竜馬のPrefab
    public GameObject huhyouhisyaPrefab; // 歩兵飛車のPrefab
    public GameObject tokinhisyaPrefab; // と金飛車のPrefab
    public GameObject huhyouryuouPrefab; // 歩兵竜王のPrefab
    public GameObject tokinryuouPrefab; // と金竜王のPrefab
    public GameObject kyousyahisyaPrefab; // 香車飛車のPrefab
    public GameObject narikyouhisyaPrefab; // 成香飛車のPrefab
    public GameObject kyousyaryuouPrefab; // 香車竜王のPrefab
    public GameObject narikyouryuouPrefab; // 成香竜王のPrefab
    public GameObject keimahisyaPrefab; // 桂馬飛車のPrefab
    public GameObject narikeihisyaPrefab; // 成桂飛車のPrefab
    public GameObject keimaryuouPrefab; // 桂馬竜王のPrefab
    public GameObject narikeiryuouPrefab; // 成桂竜王のPrefab
    public GameObject ginsyouhisyaPrefab; // 銀将飛車のPrefab
    public GameObject nariginhisyaPrefab; // 成銀飛車のPrefab
    public GameObject ginsyouryuouPrefab; // 銀将竜王のPrefab
    public GameObject nariginryuouPrefab; // 成銀竜王のPrefab
    public GameObject kinsyouhisyaPrefab; // 金将飛車のPrefab
    public GameObject kinsyouryuouPrefab; // 金将竜王のPrefab
    public GameObject kakugyouhisyaPrefab; // 角行飛車のPrefab
    public GameObject ryumahisyaPrefab; // 竜馬飛車のPrefab
    public GameObject kakugyouryuouPrefab; // 角行竜王のPrefab
    public GameObject ryumaryuouPrefab; // 竜馬竜王のPrefab

    private Dictionary<string, GameObject> piecePrefabDictionary; // 駒名とPrefabの対応を保存する辞書
    private Dictionary<string, GameObject> promotedPiecePrefabDictionary; // 成り駒の辞書
    private ShogiPieceDictionary shogiPiecesData; // 駒データを格納する変数

    void Awake()
    {
        toNormal = new Dictionary<string, string>()
        {
            {"と金", "歩兵"},
            {"成香", "香車"},
            {"成桂", "桂馬"},
            {"成銀", "銀将"},
            {"竜馬", "角行"},
            {"竜王", "飛車"},
        };
        
        // 駒のPrefabを辞書に登録
        piecePrefabDictionary = new Dictionary<string, GameObject>
        {
            { "歩兵", pawnPrefab },
            { "飛車", rookPrefab },
            { "香車", lancePrefab },
            { "桂馬", knightPrefab },
            { "角行", bishopPrefab },
            { "金将", goldPrefab },
            { "銀将", silverPrefab },
            { "玉将", kingPrefab },
            { "と金", promotedPawnPrefab },
            { "竜王", promotedRookPrefab },
            { "竜馬", promotedBishopPrefab },
            { "成香", promotedLancePrefab },
            { "成桂", promotedKnightPrefab },
            { "成銀", promotedSilverPrefab },
            { "歩兵香車", huhyoukyousyaPrefab },
            { "と金香車", tokinkyousyaPrefab },
            { "歩兵成香", huhyounarikyouPrefab },
            { "と金成香", tokinnarikyouPrefab },
            { "歩兵桂馬", huhyoukeimaPrefab },
            { "と金桂馬", tokinkeimaPrefab },
            { "歩兵成桂", huhyounarikeiPrefab },
            { "と金成桂", tokinnarikeiPrefab },
            { "香車桂馬", kyousyakeimaPrefab },
            { "成香桂馬", narikyoukeimaPrefab },
            { "香車成桂", kyousyanarikeiPrefab },
            { "成香成桂", narikyounarikeiPrefab },
            { "歩兵銀将", huhyouginsyouPrefab },
            { "と金銀将", tokingunsyouPrefab },
            { "歩兵成銀", huhyounariginPrefab },
            { "と金成銀", tokinnariginPrefab },
            { "香車銀将", kyousyaginsyouPrefab },
            { "成香銀将", narikyouginsyouPrefab },
            { "香車成銀", kyousyanariginPrefab },
            { "成香成銀", narikyounariginPrefab },
            { "桂馬銀将", keimaginsyouPrefab },
            { "成桂銀将", narikeiginsyouPrefab },
            { "桂馬成銀", keimanariginPrefab },
            { "成桂成銀", narikeinariginPrefab },
            { "歩兵金将", huhyoukinsyouPrefab },
            { "と金金将", tokinkinsyouPrefab },
            { "香車金将", kyousyakinsyouPrefab },
            { "成香金将", narikyoukinsyouPrefab },
            { "桂馬金将", keimakinsyouPrefab },
            { "成桂金将", narikeikinsyouPrefab },
            { "銀将金将", ginsyoukinsyouPrefab },
            { "成銀金将", nariginkinsyouPrefab },
            { "歩兵角行", huhyoukakugyouPrefab },
            { "と金角行", tokinkakugyouPrefab },
            { "歩兵竜馬", huhyouryumaPrefab },
            { "と金竜馬", tokinryumaPrefab },
            { "香車角行", kyousyakakugyouPrefab },
            { "成香角行", narikyoukakugyouPrefab },
            { "香車竜馬", kyousyaryumaPrefab },
            { "成香竜馬", narikyouryumaPrefab },
            { "桂馬角行", keimakakugyouPrefab },
            { "成桂角行", narikeikakugyouPrefab },
            { "桂馬竜馬", keimaryumaPrefab },
            { "成桂竜馬", narikeiryumaPrefab },
            { "銀将角行", ginsyoukakugyouPrefab },
            { "成銀角行", nariginkakugyouPrefab },
            { "銀将竜馬", ginsyouryumaPrefab },
            { "成銀竜馬", nariginryumaPrefab },
            { "金将角行", kinsyoukakugyouPrefab },
            { "金将竜馬", kinsyouryumaPrefab },
            { "歩兵飛車", huhyouhisyaPrefab },
            { "と金飛車", tokinhisyaPrefab },
            { "歩兵竜王", huhyouryuouPrefab },
            { "と金竜王", tokinryuouPrefab },
            { "香車飛車", kyousyahisyaPrefab },
            { "成香飛車", narikyouhisyaPrefab },
            { "香車竜王", kyousyaryuouPrefab },
            { "成香竜王", narikyouryuouPrefab },
            { "桂馬飛車", keimahisyaPrefab },
            { "成桂飛車", narikeihisyaPrefab },
            { "桂馬竜王", keimaryuouPrefab },
            { "成桂竜王", narikeiryuouPrefab },
            { "銀将飛車", ginsyouhisyaPrefab },
            { "成銀飛車", nariginhisyaPrefab },
            { "銀将竜王", ginsyouryuouPrefab },
            { "成銀竜王", nariginryuouPrefab },
            { "金将飛車", kinsyouhisyaPrefab },
            { "金将竜王", kinsyouryuouPrefab },
            { "角行飛車", kakugyouhisyaPrefab },
            { "竜馬飛車", ryumahisyaPrefab },
            { "角行竜王", kakugyouryuouPrefab },
            { "竜馬竜王", ryumaryuouPrefab }
        };
        
        LoadShogiPiecesData(); // JSONを読み込む
    }
    public string GetCombinedPieceName(string pieceName1, string pieceName2)
    {
        // 合成パターンを網羅
        Dictionary<(string, string), string> combinePatterns = new Dictionary<(string, string), string>
        {
            { ("歩兵", "香車"), "歩兵香車" },
            { ("香車", "歩兵"), "歩兵香車" },
            { ("歩兵", "飛車"), "歩兵飛車" },
            { ("飛車", "歩兵"), "歩兵飛車" },
            { ("歩兵", "桂馬"), "歩兵桂馬" },
            { ("桂馬", "歩兵"), "歩兵桂馬" },
            { ("金将", "銀将"), "金将銀将" },
            { ("銀将", "金将"), "金将銀将" },
            { ("飛車", "角行"), "角行飛車" },
            { ("竜馬", "竜王"), "竜馬竜王" },
            { ("歩兵", "銀将"), "歩兵銀将" },
            { ("銀将", "歩兵"), "歩兵銀将" },
            { ("桂馬", "銀将"), "桂馬銀将" },
            { ("銀将", "桂馬"), "桂馬銀将" },
            { ("香車", "桂馬"), "香車桂馬" },
            { ("桂馬", "香車"), "香車桂馬" },
            { ("香車", "銀将"), "香車銀将" },
            { ("銀将", "香車"), "香車銀将" },
            { ("歩兵", "角行"), "歩兵角行" },
            { ("角行", "歩兵"), "歩兵角行" },
            { ("歩兵", "竜王"), "歩兵竜王" },
            { ("竜王", "歩兵"), "歩兵竜王" },
            { ("香車", "飛車"), "香車飛車" },
            { ("飛車", "香車"), "香車飛車" },
            { ("香車", "竜王"), "香車竜王" },
            { ("竜王", "香車"), "香車竜王" },
            { ("桂馬", "飛車"), "桂馬飛車" },
            { ("飛車", "桂馬"), "桂馬飛車" },
            { ("桂馬", "竜王"), "桂馬竜王" },
            { ("竜王", "桂馬"), "桂馬竜王" },
            { ("銀将", "飛車"), "銀将飛車" },
            { ("飛車", "銀将"), "銀将飛車" },
            { ("銀将", "竜王"), "銀将竜王" },
            { ("竜王", "銀将"), "銀将竜王" },
            { ("金将", "飛車"), "金将飛車" },
            { ("飛車", "金将"), "金将飛車" },
            { ("金将", "竜王"), "金将竜王" },
            { ("竜王", "金将"), "金将竜王" },
            { ("角行", "飛車"), "角行飛車" },
            { ("飛車", "角行"), "角行飛車" },
            { ("角行", "竜馬"), "角行竜馬" },
            { ("竜馬", "角行"), "角行竜馬" },
        };

        // 組み合わせが辞書に存在する場合は合成した名前を返す
        var key = (pieceName1, pieceName2);
        if (combinePatterns.ContainsKey(key))
        {
            return combinePatterns[key];
        }

        // 存在しない場合は片方の駒を返す
        return pieceName1;
    }

    // JSONファイルから駒データを読み込む
    void LoadShogiPiecesData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("ShogiPieces"); // ShogiPieces.jsonを読み込む
        shogiPiecesData = JsonConvert.DeserializeObject<ShogiPieceDictionary>(jsonText.text);
    }

    // 駒のPrefabを取得する関数
    public GameObject GetPiecePrefab(string pieceName)
    {
        if (piecePrefabDictionary.ContainsKey(pieceName))
        {
            return piecePrefabDictionary[pieceName];
        }
        else
        {
            Debug.LogError("駒のPrefabが見つかりません: " + pieceName);
            return null;
        }
    }

    // 指定された駒のデータを取得
    public ShogiPieceData GetPieceData(string pieceName)
    {
        if (shogiPiecesData.駒.ContainsKey(pieceName))
        {
            return shogiPiecesData.駒[pieceName];
        }
        else
        {
            Debug.LogError("駒が見つかりません: " + pieceName);
            return null;
        }
    }
}
