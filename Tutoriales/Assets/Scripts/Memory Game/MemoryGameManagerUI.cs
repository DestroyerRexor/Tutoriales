using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryGameManagerUI : MinigamesBase
{
    protected override string minigameName => "Memory Game";

    [Space]
    [SerializeField] private CardGroup cardGroup;
    [SerializeField] private CardGridUI cardGridUI;
    [SerializeField] private List<CardSingleUI> cardSingleUIList = new List<CardSingleUI>();

    private void Start()
    {
        gameOverManager.OnRestart += OnClose;
    }

    private void OnDestroy()
    {
        gameOverManager.OnRestart -= OnClose;
    }

    private void OnClose()
    {
        Restart();
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

        GameWin(10, "Felicidades", "Ganaste");
        Restart();
    }

    public void Restart()
    {
        cardSingleUIList.Clear();
    }

    public override void Show()
    {
        base.Show();
        cardGridUI.Init(this);
        cardGridUI.FillGrid();
        cardGroup.OnCardMatch += CardGroup_OnCardMatch;
    }

    public override void Hide()
    {
        base.Hide();

        cardGroup.OnCardMatch -= CardGroup_OnCardMatch;
    }
}
