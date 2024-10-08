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
    public GameObject tokinginsyouPrefab; // と金銀将のPrefab
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
            { "と金銀将", tokinginsyouPrefab },
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
    // 合成駒名を取得する関数
    public string GetCombinedPieceName(string pieceName1, string pieceName2)
    {
        // 合成パターンを網羅
        Dictionary<(string, string), string> combinePatterns = new Dictionary<(string, string), string>
        {
            { ("歩兵", "香車"), "歩兵香車" },
            { ("と金", "香車"), "と金香車" },
            { ("歩兵", "成香"), "歩兵成香" },
            { ("と金", "成香"), "と金成香" },
            { ("歩兵", "桂馬"), "歩兵桂馬" },
            { ("と金", "桂馬"), "と金桂馬" },
            { ("歩兵", "成桂"), "歩兵成桂" },
            { ("と金", "成桂"), "と金成桂" },
            { ("香車", "桂馬"), "香車桂馬" },
            { ("成香", "桂馬"), "成香桂馬" },
            { ("香車", "成桂"), "香車成桂" },
            { ("成香", "成桂"), "成香成桂" },
            { ("歩兵", "銀将"), "歩兵銀将" },
            { ("と金", "銀将"), "と金銀将" },
            { ("歩兵", "成銀"), "歩兵成銀" },
            { ("と金", "成銀"), "と金成銀" },
            { ("香車", "銀将"), "香車銀将" },
            { ("成香", "銀将"), "成香銀将" },
            { ("香車", "成銀"), "香車成銀" },
            { ("成香", "成銀"), "成香成銀" },
            { ("桂馬", "銀将"), "桂馬銀将" },
            { ("成桂", "銀将"), "成桂銀将" },
            { ("桂馬", "成銀"), "桂馬成銀" },
            { ("成桂", "成銀"), "成桂成銀" },
            { ("歩兵", "金将"), "歩兵金将" },
            { ("と金", "金将"), "と金金将" },
            { ("香車", "金将"), "香車金将" },
            { ("成香", "金将"), "成香金将" },
            { ("桂馬", "金将"), "桂馬金将" },
            { ("成桂", "金将"), "成桂金将" },
            { ("銀将", "金将"), "銀将金将" },
            { ("成銀", "金将"), "成銀金将" },
            { ("歩兵", "角行"), "歩兵角行" },
            { ("と金", "角行"), "と金角行" },
            { ("歩兵", "竜馬"), "歩兵竜馬" },
            { ("と金", "竜馬"), "と金竜馬" },
            { ("香車", "角行"), "香車角行" },
            { ("成香", "角行"), "成香角行" },
            { ("香車", "竜馬"), "香車竜馬" },
            { ("成香", "竜馬"), "成香竜馬" },
            { ("桂馬", "角行"), "桂馬角行" },
            { ("成桂", "角行"), "成桂角行" },
            { ("桂馬", "竜馬"), "桂馬竜馬" },
            { ("成桂", "竜馬"), "成桂竜馬" },
            { ("銀将", "角行"), "銀将角行" },
            { ("成銀", "角行"), "成銀角行" },
            { ("銀将", "竜馬"), "銀将竜馬" },
            { ("成銀", "竜馬"), "成銀竜馬" },
            { ("金将", "角行"), "金将角行" },
            { ("金将", "竜馬"), "金将竜馬" },
            { ("歩兵", "飛車"), "歩兵飛車" },
            { ("と金", "飛車"), "と金飛車" },
            { ("歩兵", "竜王"), "歩兵竜王" },
            { ("と金", "竜王"), "と金竜王" },
            { ("香車", "飛車"), "香車飛車" },
            { ("成香", "飛車"), "成香飛車" },
            { ("香車", "竜王"), "香車竜王" },
            { ("成香", "竜王"), "成香竜王" },
            { ("桂馬", "飛車"), "桂馬飛車" },
            { ("成桂", "飛車"), "成桂飛車" },
            { ("桂馬", "竜王"), "桂馬竜王" },
            { ("成桂", "竜王"), "成桂竜王" },
            { ("銀将", "飛車"), "銀将飛車" },
            { ("成銀", "飛車"), "成銀飛車" },
            { ("銀将", "竜王"), "銀将竜王" },
            { ("成銀", "竜王"), "成銀竜王" },
            { ("金将", "飛車"), "金将飛車" },
            { ("金将", "竜王"), "金将竜王" },
            { ("角行", "飛車"), "角行飛車" },
            { ("竜馬", "飛車"), "竜馬飛車" },
            { ("角行", "竜王"), "角行竜王" },
            { ("竜馬", "竜王"), "竜馬竜王" }

        };

        // 組み合わせが辞書に存在する場合は合成した名前を返す
        var key1 = (pieceName1, pieceName2);
        var key2 = (pieceName2, pieceName1);  // 順番を逆にしたキー

        if (combinePatterns.ContainsKey(key1))
        {
            return combinePatterns[key1];
        }
        else if (combinePatterns.ContainsKey(key2))
        {
            return combinePatterns[key2];  // 順番逆の場合の結果を返す
        }

        // 組み合わせが存在しない場合は片方の駒を返す
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
            return null;
        }
    }
    public GameObject GetCombinedPiecePrefab(string pieceName1, string pieceName2, bool isEnemy = false)
    {
        // 合成された駒の名前を取得
        string combinedPieceName = GetCombinedPieceName(pieceName1, pieceName2);

        // 駒のPrefabを取得
        GameObject combinedPiecePrefab = GetPiecePrefab(combinedPieceName);

        if (combinedPiecePrefab != null)
        {
            // 駒を生成
            GameObject newPiece = Instantiate(combinedPiecePrefab);
            newPiece.name = combinedPieceName;
            newPiece.tag = isEnemy ? "Enemy" : "Player"; // タグを設定

            // 敵側の駒であれば180度回転させる
            if (isEnemy)
            {
                newPiece.transform.rotation = Quaternion.Euler(0, 0, 180); // 180度回転
            }

            return newPiece;
        }

        // もし駒が見つからなければnullを返す
        return null;
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
            return null;
        }
    }
}