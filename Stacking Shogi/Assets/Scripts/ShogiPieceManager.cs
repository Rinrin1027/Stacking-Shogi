using System.Collections.Generic;
using UnityEngine;

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
    public GameObject piecePrefab; // 駒のPrefab

    private ShogiPieceDictionary shogiPiecesData; // 駒データを格納する変数

    void Start()
    {
        LoadShogiPiecesData(); // JSONを読み込む
    }

    // JSONファイルから駒データを読み込む
    void LoadShogiPiecesData()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("ShogiPieces"); // ShogiPieces.jsonを読み込む
        shogiPiecesData = JsonUtility.FromJson<ShogiPieceDictionary>(jsonText.text);
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