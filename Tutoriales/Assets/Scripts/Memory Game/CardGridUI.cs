using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardGridUI : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public string cardName;
        public Sprite cardImage;
    }

    [SerializeField] private List<Card> cardList = new List<Card>();
    [SerializeField] private List<Card> cardListToSort = new List<Card>();
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform cardPrefab;

    private MemoryGameManagerUI memoryGameManager;

    private void Start()
    {
        cardPrefab.gameObject.SetActive(false);
    }

    public void Init(MemoryGameManagerUI memoryGameManager)
    {
        this.memoryGameManager = memoryGameManager;
    }

    private void CleanGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }

        cardListToSort.Clear();
    }

    public void FillGrid()
    {
        CleanGrid();

        int cardsToShow = 0;

        switch (memoryGameManager.GetDifficulty())
        {
            case DifficultyEnum.Easy:
                cardsToShow = 6;
                break;
            case DifficultyEnum.Normal:
                cardsToShow = 9;
                break;
            case DifficultyEnum.Hard:
                cardsToShow = 12;
                break;
            default:
                break;
        }

        for (int i = 0; i < cardsToShow; i++)
        {
            cardListToSort.Add(cardList[i]);
            cardListToSort.Add(cardList[i]);
        }

        System.Random rnd = new System.Random();

        IOrderedEnumerable<Card> randomized = cardListToSort.OrderBy(i => rnd.Next());

        foreach (Card card in randomized)
        {
            Transform cardTransform = Instantiate(cardPrefab, cardContainer);
            cardTransform.gameObject.SetActive(true);
            cardTransform.name = card.cardName;

            CardSingleUI cardSingleUI = cardTransform.GetComponent<CardSingleUI>();
            cardSingleUI.SetCardImage(card.cardImage);
            cardSingleUI.Init(memoryGameManager);

        }
    }

}
