using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapturedPieces : MonoBehaviour
{
    string[] pieceNames =
    {
        "歩",
        "香車",
        "桂馬",
        "銀",
        "金",
        "角行",
        "飛車",
    };

    [SerializeField] ShogiPieceManager pieceManager;
    [SerializeField] GameObject pieceCounterPrefab;

    private Dictionary<string, int> pieces;
    private Dictionary<string, TextMeshPro> numofPiecesTexts;

    // Start is called before the first frame update
    void Start()
    {
        pieces = new Dictionary<string, int>();
        numofPiecesTexts = new Dictionary<string, TextMeshPro>();
        
        Init();
    }

    void Init()
    {
        Vector3 origin = gameObject.transform.position;
        for (int i = 0; i < pieceNames.Length; i++)
        {
            int x = i % 4, y = i / 4;
            GameObject piece = Instantiate(pieceManager.GetPiecePrefab(pieceNames[i]), gameObject.transform);
            piece.name = "持ち駒 " + pieceNames[i];
            piece.transform.position = origin + new Vector3(x, -y);
            piece.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            piece.tag = gameObject.tag;
            
            GameObject pieceCounter = Instantiate(pieceCounterPrefab, gameObject.transform);
            pieceCounter.name = "持ち駒の個数 " + pieceNames[i];
            pieceCounter.transform.position = origin + new Vector3(x + 0.5f, -y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // 持ち駒を追加する
    public void AddPiece(string pieceName)
    {
        pieces[pieceName]++;
    }
    
    // 持ち駒を削除する
    public void RemovePiece(string pieceName)
    {
        pieces[pieceName]++;
    }

    public bool HasPiece(string pieceName)
    {
        return pieces[pieceName] > 0;
    }
    
}