using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : MonoBehaviour
{
    public ShogiBoard shogiBoard; // 将棋盤の管理クラス
    public ShogiPieceController shogiPieceController; // 駒の配置を行うスクリプト
    public int recordIndex = 0; // 現在の手数
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnPrevButtonPressed()
    {
        if (recordIndex > 0)
        {
            recordIndex--;
            shogiPieceController.SetPieces(shogiBoard.records[recordIndex]);
        }
    }
    
    void OnNextButtonPressed()
    {
        if (recordIndex < shogiBoard.records.Count - 1)
        {
            recordIndex++;
            shogiPieceController.SetPieces(shogiBoard.records[recordIndex]);
        }
    }
}
