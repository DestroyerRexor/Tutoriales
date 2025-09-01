using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlidingPuzzleSingleUI : MonoBehaviour
{
    [SerializeField] private Button pieceButton;
    [SerializeField] private TMPro.TMP_Text pieceNumberText;
    private TweenerUI tweenerUI;

    private SlidingPuzzleGroup slidingPuzzleGroup;
    private Vector3 targetPosition;
    private RectTransform rectTransform;

    private float duration = 0.2f;
    private int pieceNumber = 0;

    public Vector3 TargetPosition => targetPosition;
    public int PieceNumber => pieceNumber;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        tweenerUI = GetComponent<TweenerUI>();

        targetPosition = rectTransform.localPosition;
    }

    private void Start()
    {
        pieceButton.onClick.AddListener(OnPieceClicked);
    }

    private void OnDestroy()
    {
        pieceButton.onClick.RemoveListener(OnPieceClicked);
    }

    private void OnPieceClicked()
    {
        tweenerUI.Show();
        slidingPuzzleGroup.OnPieceClicked(this);
    }

    public void Setup(SlidingPuzzleGroup slidingPuzzleGroup, int pieceNumber)
    {
        DisableButton();
        this.slidingPuzzleGroup = slidingPuzzleGroup;
        this.pieceNumber = pieceNumber;
    }

    public void SetTargetPosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
        rectTransform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutQuad);
    }

    public void EnableButton()
    {
        pieceButton.interactable = true;
    }

    public void DisableButton()
    {
        pieceButton.interactable = false;
    }

    public void SetVisualNumber(int number)
    {
        pieceNumberText.SetText(number.ToString());
    }
}
