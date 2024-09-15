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
    public GameObject huhyoukyousyaPrehab; // 歩兵香車のPrefab
    public GameObject tokinkyousyaPrefab; // と金香車のPrefab
    public GameObject huhyounarikyouPrehab; // 歩兵成香のPrefab
    public GameObject tokinnarikyouPrehab; // と金成香のPrefab
    public GameObject huhyoukeimaPrefab; // 歩兵桂馬のPrefab
    public GameObject tokinkeimaPrefab; // と金桂馬のPrefab
    public GameObject huhyounarikeiPrefab; // 歩兵成桂のPrefab
    public GameObject tokinnarikeiPrefab; // と金成桂のPrefab
    public GameObject kyousyakeimaPrefab; // 香車桂馬のPrefab
    public GameObject narikyoukeimaPrefab; // 成香桂馬のPrefab
    public GameObject kyousyanarikeiPrefab; // 香車成桂のPrefab
    public GameObject narikyounarikeiPrefab; // 成香成桂のPrefab

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
            { "成銀", promotedSilverPrefab }
        };
        
        LoadShogiPiecesData(); // JSONを読み込む
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
