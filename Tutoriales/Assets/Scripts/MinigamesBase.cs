using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MinigamesBase : MonoBehaviour
{
    [Header("Minigame base")]
    [SerializeField] protected GameObject gameArea;

    [SerializeField] private Button negativeButton;
    [SerializeField] private Button positiveButton;

    protected void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    protected void ToggleGameArea(bool toggle)
    {
        gameArea.SetActive(toggle);
    }

    protected virtual void GameWin(int currencyValue, string titleText, string descriptionText, System.Action onNegative = null, System.Action onPositive = null)
    {
        Debug.Log($"{titleText} {descriptionText} {currencyValue}");

        negativeButton?.onClick.AddListener(() =>
        {
            onNegative?.Invoke();
            Toggle(true);
            ToggleGameArea(false);
            DifficultyManager.Instance.Toggle(true);
        });

        positiveButton?.onClick.AddListener(() =>
        {
            onPositive?.Invoke();
            Toggle(false);
            ToggleGameArea(false);
            DifficultyManager.Instance.HideCloseButton();
        });
    }

    public virtual void GameLose(string titleText, string descriptionText, System.Action onNegative = null, System.Action onPositive = null)
    {
        Debug.Log($"{titleText} {descriptionText}");

        negativeButton?.onClick.AddListener(() =>
        {
            onNegative?.Invoke();
            Toggle(true);
            ToggleGameArea(false);
            DifficultyManager.Instance.Toggle(true);
        });

        positiveButton?.onClick.AddListener(() =>
        {
            onPositive?.Invoke();
            Toggle(false);
            ToggleGameArea(false);
            DifficultyManager.Instance.HideCloseButton();
        });
    }

    public DifficultyEnum GetDifficulty()
    {
        return DifficultyManager.Instance.GetDifficulty();
    }
}
