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

    private bool isActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public DifficultyManager OnCloseButtonClick(Action onClick)
    {
        closeMinigameButton.onClick.AddListener(() =>
        {
            closeMinigameButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                
                onClick?.Invoke();
            });
        });

        return this;
    }

    public DifficultyManager OnEasyButtonClick(Action onClick)
    {
        easyButton.onClick.AddListener(() =>
        {
            easyButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                difficulty = DifficultyEnum.Easy;
                onClick?.Invoke();
            });
        });

        return this;
    }

    public DifficultyManager OnNormalButtonClick(Action onClick)
    {
        normalButton.onClick.AddListener(() =>
        {
            normalButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                difficulty = DifficultyEnum.Normal;
                onClick?.Invoke();
            });
        });

        return this;
    }

    public DifficultyManager OnHardButtonClick(Action onClick)
    {
        hardButton.onClick.AddListener(() =>
        {
            hardButton.GetComponent<TweenerUI>()
            .Show()
            .OnComplete(() =>
            {
                difficulty = DifficultyEnum.Hard;
                onClick?.Invoke();
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

    public void HideCloseButton()
    {
        closeMinigameButton.gameObject.SetActive(false);
    }

    public void ShowCloseButton()
    {
        closeMinigameButton.gameObject.SetActive(true);
    }

    public void InteractableCloseButton(bool interactable)
    {
        closeMinigameButton.interactable = interactable;
    }

    public DifficultyEnum GetDifficulty() => difficulty;

    public void Toggle(bool toggle)
    {
        isActive = toggle;

        difficultySelector.SetActive(isActive);

    }
}
