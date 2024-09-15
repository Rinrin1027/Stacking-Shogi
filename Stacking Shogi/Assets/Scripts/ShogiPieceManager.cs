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
    public string 成り;
    
}

// JSON全体を格納するクラス
[System.Serializable]
public class ShogiPieceDictionary
{
    public Dictionary<string, ShogiPieceData> 駒;
}


public class ShogiPieceManager : MonoBehaviour
{
    // 駒のPrefabを設定する
    public GameObject pawnPrefab;   // 歩のPrefab
    public GameObject rookPrefab;   // 飛車のPrefab
    public GameObject lancePrefab;   // 香車のPrefab 
    public GameObject knightPrefab; // 桂馬のPrefab
    public GameObject bishopPrefab; // 角行のPrefab
    public GameObject goldPrefab;   // 金のPrefab
    public GameObject silverPrefab; // 銀のPrefab
    public GameObject kingPrefab;   // 王のPrefab
    public GameObject promotedPawnPrefab;  // と金のPrefab
    public GameObject promotedRookPrefab;  // 竜王のPrefab
    public GameObject promotedBishopPrefab; // 竜馬のPrefab

    private Dictionary<string, GameObject> piecePrefabDictionary; // 駒名とPrefabの対応を保存する辞書
    private ShogiPieceDictionary shogiPiecesData; // 駒データを格納する変数

    void Awake()
    {
        // 駒のPrefabを辞書に登録
        piecePrefabDictionary = new Dictionary<string, GameObject>
        {
            { "歩", pawnPrefab },
            { "飛車", rookPrefab },
            { "香車", lancePrefab },
            { "桂馬", knightPrefab },
            { "角行", bishopPrefab },
            { "金", goldPrefab },
            { "銀", silverPrefab },
            { "王", kingPrefab },
            { "と金", promotedPawnPrefab },
            { "竜王", promotedRookPrefab },
            { "竜馬", promotedBishopPrefab }
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
        Debug.Log(shogiPiecesData.駒.Count);
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
