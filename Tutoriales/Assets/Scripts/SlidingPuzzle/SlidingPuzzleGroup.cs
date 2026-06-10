using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlidingPuzzleGroup : MonoBehaviour
{
    [SerializeField] private Transform emptyPiece;
    [SerializeField] private List<SlidingPuzzleSingleUI> piecesList = new List<SlidingPuzzleSingleUI>();

    private SlidingPuzzleManagerUI slidingPuzzleManager;

    private int emptyPieceIndex;

    public event System.Action OnCompleteGame;

    private void Start()
    {
        emptyPieceIndex = piecesList.Count - 1;
    }

    //[ContextMenu("Set Pieces")]
    //public void SetPieces()
    //{
    //    piecesList.Clear();

    //    for (int i = 1; i < transform.childCount; i++)
    //    {
    //        int index = i + 1;
    //        if (transform.GetChild(i).TryGetComponent(out SlidingPuzzleSingleUI piece))
    //        {
    //            piece.name = $"Piece_{index:00}";
    //            piecesList.Add(piece);
    //            piece.SetVisualNumber(index);
    //        }
    //        else
    //        {
    //            piecesList.Add(null);
    //        }
    //    }
    //}

    public void Init(SlidingPuzzleManagerUI slidingPuzzleManager)
    {
        this.slidingPuzzleManager = slidingPuzzleManager;
        InitializePieces();
        InitializeTimer();
        Shuffle();
    }

    public void OnTimeOver()
    {
        foreach (SlidingPuzzleSingleUI piece in piecesList)
        {
            if (piece == null) continue;
            piece.DisableButton();
        }
    }

    public void OnPieceClicked(SlidingPuzzleSingleUI slidingPuzzleSingleUI)
    {
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(piecesList.Count));

        int pieceIndex = FindIndexByPiece(slidingPuzzleSingleUI);
        int pieceRow = pieceIndex / gridSize;
        int pieceCol = pieceIndex % gridSize;

        if(IsAdjacentToEmptyPiece(pieceRow, pieceCol))
        {
            Vector2 lastEmptyPiecePosition = emptyPiece.localPosition;
            emptyPiece.localPosition = slidingPuzzleSingleUI.TargetPosition;
            slidingPuzzleSingleUI.SetTargetPosition(lastEmptyPiecePosition);

            piecesList[emptyPieceIndex] = piecesList[pieceIndex];
            piecesList[pieceIndex] = null;
            emptyPieceIndex = pieceIndex;

            if (CheckIfComplete())
            {
                foreach (SlidingPuzzleSingleUI piece in piecesList)
                {
                    if (piece == null) continue;
                    piece.DisableButton();
                }

                OnCompleteGame?.Invoke();
            }
        }
    }

    private bool CheckIfComplete()
    {
        int nullIndex = piecesList.FindIndex(piece => piece == null);
        if(nullIndex >= 0 && nullIndex < piecesList.Count - 1)
        {
            return false;
        }

        return piecesList
            .Where(piece => piece != null)
            .Zip(piecesList.Skip(1).Where(piece => piece != null),
            (a, b) => a.PieceNumber + 1 == b.PieceNumber)
            .All(x => x);
    }

    private bool IsAdjacentToEmptyPiece(int row, int col)
    {
        int gridSize = Mathf.RoundToInt(Mathf.Sqrt(piecesList.Count));

        int emptyRow = emptyPieceIndex / gridSize;
        int emptyCol = emptyPieceIndex % gridSize;

        return (row == emptyRow && Mathf.Abs(col - emptyCol) == 1) || (col == emptyCol && Mathf.Abs(row - emptyRow) == 1);
    }

    private void InitializePieces()
    {
        for (int i = 0; i < piecesList.Count; i++)
        {
            int index = i + 1;
            if (piecesList[i] == null) continue;
            piecesList[i].Setup(this, index);
        }
    }

    private void InitializeTimer()
    {
        float timer = 180f;

        switch (slidingPuzzleManager.GetDifficulty())
        {
            case DifficultyEnum.Easy:
                timer = 180f;
                break;
            case DifficultyEnum.Normal:
                timer = 120f;
                break;
            case DifficultyEnum.Hard:
                timer = 60f;
                break;
            default:
                break;
        }

        TimerUI.Instance.SetMaxTimer(timer).Play();
    }

    private int FindIndexByPiece(SlidingPuzzleSingleUI slidingPuzzleSingleUI)
    {
        for (int i = 0; i < piecesList.Count; i++)
        {
            if (piecesList[i] == null) continue;

            if (piecesList[i] == slidingPuzzleSingleUI) return i;
        }

        return -1;
    }

    private void Shuffle()
    {
        int piecesSize = piecesList.Count - 1;

        if (emptyPieceIndex != piecesSize)
        {
            if (piecesList[piecesSize] != null)
            {
                Vector3 pieceOnLastPos = piecesList[piecesSize].TargetPosition;
                piecesList[piecesSize].SetTargetPosition(emptyPiece.localPosition);
                emptyPiece.localPosition = pieceOnLastPos;
                piecesList[emptyPieceIndex] = piecesList[piecesSize];
                piecesList[piecesSize] = null;
                emptyPieceIndex = piecesSize;
            }
        }

        for (int i = 0; i < piecesSize; i++)
        {
            if (piecesList[i] == null) continue;

            Vector3 lastPos = piecesList[i].TargetPosition;
            int rndIndex = Random.Range(0, piecesSize);
            piecesList[i].SetTargetPosition(piecesList[rndIndex].TargetPosition);
            piecesList[rndIndex].SetTargetPosition(lastPos);

            SlidingPuzzleSingleUI piece = piecesList[i];
            piecesList[i] = piecesList[rndIndex];
            piecesList[rndIndex] = piece;
        }

        int invertion = GetInversions();

        if (invertion % 2 != 0)
        {
            //No es posible resolverlo
            Shuffle();
        }
        else
        {
            foreach (SlidingPuzzleSingleUI piece in piecesList)
            {
                if (piece == null) continue;

                piece.EnableButton();
            }
        }
    }

    private int GetInversions()
    {
        int inversionSum = 0;
        for (int i = 0; i < piecesList.Count; i++)
        {
            int thisPieceInvertion = 0;
            for (int j = i; j < piecesList.Count; j++)
            {
                if (piecesList[j] != null)
                {
                    if (piecesList[i] != null && piecesList[i].PieceNumber > piecesList[j].PieceNumber)
                    {
                        thisPieceInvertion++;
                    }
                }
            }
            inversionSum += thisPieceInvertion;
        }
        return inversionSum;
    }
}
