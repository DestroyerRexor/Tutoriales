using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesHandler : MonoBehaviour
{
    [SerializeField] private List<MinigamesBase> minigamesList = new List<MinigamesBase>();
    [Space]
    [SerializeField] private SelectGame selectMinigamePrefab;
    [SerializeField] private Transform parent;
    [Space]
    [SerializeField] private GameObject minigamesGrid;
    [SerializeField] private GameOverManager gameOverManager;

    private void Start()
    {
        FillMinigames();
    }

    private void FillMinigames()
    {
        foreach (MinigamesBase minigamesBase in minigamesList)
        {
            SelectGame selectGame = Instantiate(selectMinigamePrefab, parent);

            selectGame.Setup(minigamesBase, this);

            minigamesBase.Hide();
            minigamesBase.Setup(gameOverManager);
        }
    }

    public void Hide()
    {
        minigamesGrid.SetActive(false);
    }

    public void Show()
    {
        minigamesGrid.SetActive(true);
    }
}
