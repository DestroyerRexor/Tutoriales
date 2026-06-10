using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardSingleUI : MonoBehaviour
{
    private CardGroup cardGroup;

    [SerializeField] private Button cardBackButton;

    [SerializeField] private Image cardBackBackground;
    [SerializeField] private Image cardFrontBackground;
    [SerializeField] private Image cardFrontImage;

    [SerializeField] private GameObject cardBack;
    [SerializeField] private GameObject cardFront;

    private bool objectMatch;

    [Header("DoTween Animation")]
    [SerializeField] private Vector3 selectRotation = new Vector3();
    [SerializeField] private Vector3 deselectRotation = new Vector3();
    [SerializeField] private float duration = 0.25f;

    private MemoryGameManagerUI memoryGameManager;

    private void Awake()
    {
        if (cardGroup == null)
        {
            cardGroup = transform.parent.GetComponent<CardGroup>();
        }

        if (cardGroup != null)
        {
            cardGroup.Subscribe(this);
        }
    }

    public void Init(MemoryGameManagerUI memoryGameManager)
    {
        this.memoryGameManager = memoryGameManager;
    }

    private void Start()
    {
        cardBackButton.onClick.AddListener(OnClick);

        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

        StartCoroutine(WaitingToHide());

        memoryGameManager.Subscribe(this);

    }

    private void OnClick()
    {
        cardGroup.OnCardSelected(this);
    }

    public void Select()
    {
        FlipAnimation(selectRotation, true);
    }

    public void Deselect()
    {
        FlipAnimation(deselectRotation, false);
    }

    private IEnumerator WaitingToHide()
    {
        yield return new WaitForSeconds(3f);

        FlipAnimation(deselectRotation, false);
    }

    private void FlipAnimation(Vector3 targetRotation, bool showFront)
    {
        Sequence flipSeq = DOTween.Sequence();

        flipSeq.Append(transform.DORotate(targetRotation, duration).SetEase(Ease.InOutElastic));

        flipSeq.InsertCallback(duration / 2f, () =>
        {
            cardFront.SetActive(showFront);
            cardBack.SetActive(!showFront);
        });
    }

    public Image GetCardBackBackground() => cardBackBackground;
    public Image GetCardFrontBackground() => cardFrontBackground;

    public void SetObjectMatch()
    {
        objectMatch = true;
    }

    public void SetCardImage(Sprite sprite)
    {
        cardFrontImage.sprite = sprite;
    }
    
    public bool GetObjectMatch() => objectMatch;

    public void DisableCardBackButton() => cardBackButton.interactable = false;

}
