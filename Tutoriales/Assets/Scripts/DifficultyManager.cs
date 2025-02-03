using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [SerializeField] private GameObject difficultySelector;

    [SerializeField] private Button closeMinigameButton;

    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;

    private DifficultyEnum difficulty;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }

    public DifficultyManager OnCloseButtonClick(Action onClick, bool autoHide = true)
    {
        closeMinigameButton.onClick.AddListener(() =>
        {
            closeMinigameButton.interactable = false;
            closeMinigameButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                closeMinigameButton.interactable = true;

                onClick?.Invoke();

                if (autoHide)
                    Hide();
            });
        });

        return this;
    }

    public DifficultyManager OnEasyButtonClick(Action onClick, bool autoHide = true)
    {
        easyButton.onClick.AddListener(() =>
        {
            easyButton.interactable = false;
            easyButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                easyButton.interactable = true;

                difficulty = DifficultyEnum.Easy;
                onClick?.Invoke();

                if (autoHide) 
                    Hide();
            });
        });

        return this;
    }

    public DifficultyManager OnNormalButtonClick(Action onClick, bool autoHide = true)
    {
        normalButton.onClick.AddListener(() =>
        {
            normalButton.interactable = false;
            normalButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                normalButton.interactable = true;

                difficulty = DifficultyEnum.Normal;
                onClick?.Invoke();

                if (autoHide)
                    Hide();
            });
        });

        return this;
    }

    public DifficultyManager OnHardButtonClick(Action onClick, bool autoHide = true)
    {
        hardButton.onClick.AddListener(() =>
        {
            hardButton.interactable = false;
            hardButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                hardButton.interactable = true;

                difficulty = DifficultyEnum.Hard;
                onClick?.Invoke();

                if (autoHide)
                    Hide();
            });
        });

        return this;
    }

    public DifficultyManager ResetListeners()
    {
        easyButton.onClick.RemoveAllListeners();
        normalButton.onClick.RemoveAllListeners();
        hardButton.onClick.RemoveAllListeners();
        //closeMinigameButton.onClick.RemoveAllListeners();

        return this;
    }

    public DifficultyEnum GetDifficulty() => difficulty;

    public void Show()
    {
        difficultySelector.SetActive(true);
    }

    public void Hide()
    {
        difficultySelector.SetActive(false);
    }
}
