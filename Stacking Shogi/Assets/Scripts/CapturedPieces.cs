using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapturedPieces : MonoBehaviour
{
    public float pieceSize;
    public Vector2 offset;
    public float textOffset;
    
    string[] pieceNames =
    {
        "歩兵",
        "香車",
        "桂馬",
        "銀将",
        "金将",
        "角行",
        "飛車",
    };

    [SerializeField] ShogiPieceManager pieceManager;
    [SerializeField] GameObject pieceCounterPrefab;

    private Dictionary<string, int> pieces;
    private Dictionary<string, TextMeshProUGUI> numofPiecesTexts;

    // Start is called before the first frame update
    void Start()
    {
        pieces = new Dictionary<string, int>();
        numofPiecesTexts = new Dictionary<string, TextMeshProUGUI>();
        if (gameObject.CompareTag("Enemy"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            offset = -offset;
            textOffset = -textOffset;
        }
        
        Init();
    }

    void Init()
    {
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < pieceNames.Length; i++)
        {
            pieces.Add(pieceNames[i], 0);
            int x = i % 4, y = i / 4;
            GameObject piece = Instantiate(pieceManager.GetPiecePrefab(pieceNames[i]), gameObject.transform);
            piece.name = pieceNames[i];
            piece.transform.position = origin + new Vector3(x * offset.x, -y * offset.y, 0);
            piece.transform.localScale = new Vector3(pieceSize, pieceSize, pieceSize);
            piece.tag = gameObject.CompareTag("Player") ? "CapturedPlayer" : "CapturedEnemy";
            
            GameObject pieceCounter = Instantiate(pieceCounterPrefab, gameObject.transform);
            pieceCounter.name = pieceNames[i] + " の個数";
            pieceCounter.transform.position = origin + new Vector3(x * offset.x + textOffset, -y * offset.y, 0);
            numofPiecesTexts[pieceNames[i]] = pieceCounter.GetComponent<TextMeshProUGUI>();
        }
    }
    
    // GameObjectから持ち駒を追加する（オブジェクトのコンポーネントを参照）
    public void AddPiece(GameObject pieceObj)
    {
        ShogiPiece shogiPiece = pieceObj.GetComponent<ShogiPiece>();
        
        if (shogiPiece != null && shogiPiece.IsStacked)
        {
            // 合成駒の場合は、元の構成駒をすべて追加する
            foreach(string originalName in shogiPiece.originalPieceNames)
            {
                AddPieceByName(originalName);
            }
        }
        else
        {
            // 単独の駒の場合
            AddPieceByName(pieceObj.name);
        }
    }

    // 名前指定で持ち駒を追加する
    private void AddPieceByName(string pieceName)
    {
        if (pieces.ContainsKey(pieceName))
        {
            pieces[pieceName]++;
        }
        else if (pieceManager.toNormal.ContainsKey(pieceName))
        {
            pieces[pieceManager.toNormal[pieceName]]++;
        }
        UpdateUI();
    }
    
    // 持ち駒を削除する
    public void RemovePiece(string pieceName)
    {
        if (pieces.ContainsKey(pieceName))
        {
            pieces[pieceName]--;
            UpdateUI();
        }
    }

    public bool HasPiece(string pieceName)
    {
        return pieces.ContainsKey(pieceName) && pieces[pieceName] > 0;
    }

    void UpdateUI()
    {
        for (int i = 0; i < pieceNames.Length; i++)
        {
            if (numofPiecesTexts.ContainsKey(pieceNames[i]))
            {
                numofPiecesTexts[pieceNames[i]].text = "×" + pieces[pieceNames[i]];
            }
        }
    }
}