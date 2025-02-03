using System.Collections.Generic;
using UnityEngine;

public abstract class MinigamesBase : MonoBehaviour
{
    [Header("Minigame base")]
    [SerializeField] protected GameObject gameArea;
    protected virtual string minigameName => "";

    private GameOverManager gameOverManager;

    public void Setup(GameOverManager gameOverManager)
    {
        this.gameOverManager = gameOverManager;
    }

    public virtual void InitializeDifficulty(System.Action onClose = null)
    {
        DifficultyManager.Instance
            .ResetListeners()
            .OnCloseButtonClick(() =>
            {
                Hide();
                onClose?.Invoke();
            })
            .OnEasyButtonClick(() =>
            {
                Show();
            })
            .OnNormalButtonClick(() =>
            {
                Show();
            })
            .OnHardButtonClick(() =>
            {
                Show();
            });
    }

    protected virtual void GameWin(int currencyValue, string titleText, string descriptionText, System.Action onNegative = null, System.Action onPositive = null)
    {
        gameOverManager.Setup(titleText, $"{descriptionText}\n{currencyValue}", Hide).Show();
    }

    public virtual void GameLose(string titleText, string descriptionText, System.Action onNegative = null, System.Action onPositive = null)
    {
        gameOverManager.Setup(titleText, descriptionText, Hide).Show();
    }

    public DifficultyEnum GetDifficulty()
    {
        return DifficultyManager.Instance.GetDifficulty();
    }

    public virtual void Show()
    {
        gameArea.SetActive(true);
    }

    public virtual void Hide()
    {
        gameArea.SetActive(false);
    }

    public override string ToString()
    {
        return minigameName;
    }
}
