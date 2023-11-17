using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryGameManagerUI : MonoBehaviour
{
    public static MemoryGameManagerUI Instance { get; private set; }

    [SerializeField] private CardGroup cardGroup;
    [SerializeField] private List<CardSingleUI> cardSingleUIList = new List<CardSingleUI>();

    [SerializeField] private GameObject gameArea;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.1f);

        DifficultyManager.Instance
            .ResetListeners()
            .OnEasyButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                ToggleGameArea(true);
            })
            .OnNormalButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                ToggleGameArea(true);
            })
            .OnHardButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                ToggleGameArea(true);
            });
    }

    private void Start()
    {
        cardGroup.OnCardMatch += CardGroup_OnCardMatch;
    }

    public void Subscribe(CardSingleUI cardSingleUI)
    {
        if (cardSingleUIList == null)
        {
            cardSingleUIList = new List<CardSingleUI>();
        }

        if (!cardSingleUIList.Contains(cardSingleUI))
        {
            cardSingleUIList.Add(cardSingleUI);
        }
    }

    private void CardGroup_OnCardMatch(object sender, System.EventArgs e)
    {
        if (cardSingleUIList.All(x => x.GetObjectMatch()))
        {
            StartCoroutine(OnCompleteGame());
        }
    }

    private IEnumerator OnCompleteGame()
    {
        yield return new WaitForSeconds(0.75f);

        //Hacer cualquier cosa cuando ganes

        Debug.Log("Has ganado");

    }

    public DifficultyEnum GetDifficulty()
    {
        return DifficultyManager.Instance.GetDifficulty();
    }

    public void Restart()
    {
        cardSingleUIList.Clear();
    }
    private void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    private void ToggleGameArea(bool toggle)
    {
        gameArea.SetActive(toggle);
    }
}
