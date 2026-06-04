using System.Collections.Generic;
using UnityEngine;

public class ShogiPiece : MonoBehaviour
{
    // この駒を構成する元の駒のリスト
    public List<string> originalPieceNames = new List<string>();

    // 駒が合成されているかどうか
    public bool IsStacked 
    {
        get { return originalPieceNames.Count >= 2; }
    }

    public void Init(List<string> names)
    {
        originalPieceNames = new List<string>(names);
    }

    public void Init(string name)
    {
        originalPieceNames = new List<string> { name };
    }
}
