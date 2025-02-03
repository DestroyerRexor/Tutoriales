using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGame : MonoBehaviour
{
    [SerializeField] private Button openMinigame;
    [SerializeField] private TMPro.TMP_Text titleMinigame;

    private MinigamesBase minigamesBase;
    private MinigamesHandler minigamesHandler;

    private void Start()
    {
        openMinigame.onClick.AddListener(Open);
    }

    private void OnDestroy()
    {
        openMinigame.onClick.RemoveListener(Open);
    }

    public void Setup(MinigamesBase minigamesBase, MinigamesHandler minigamesHandler)
    {
        this.minigamesBase = minigamesBase;
        this.minigamesHandler = minigamesHandler;

        titleMinigame.SetText(minigamesBase.ToString());
    }

    private void Open()
    {
        DifficultyManager.Instance.Show();
        minigamesBase.InitializeDifficulty(minigamesHandler.Show);

        minigamesHandler.Hide();
    }
}
