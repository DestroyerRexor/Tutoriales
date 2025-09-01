using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleManagerUI : MinigamesBase
{
    protected override string minigameName => "Sliding Puzzle";

    [Space(20)]
    [SerializeField] private SlidingPuzzleGroup slidingPuzzleGroup;
    [SerializeField] private TMPro.TMP_Text timerText;

    public override void Show()
    {
        base.Show();
        slidingPuzzleGroup.Init(this);
        InitializeDifficulty();
        InitializeTimer();

        slidingPuzzleGroup.OnCompleteGame += OnCompleteGame;

        TimerUI.Instance.OnTimeOver += OnLoseGame;
    }

    private void InitializeTimer()
    {
        timerText.gameObject.SetActive(true);
        TimerUI.Instance.SetTimerText(timerText).Show();
    }

    public override void Hide()
    {
        base.Hide();

        slidingPuzzleGroup.OnCompleteGame -= OnCompleteGame;
        TimerUI.Instance.OnTimeOver += OnLoseGame;
    }

    private void OnCompleteGame()
    {
        GameWin(10, "Felicidades", "Ganaste");
    }

    private void OnLoseGame()
    {
        slidingPuzzleGroup.OnTimeOver();
        GameLose("Lo siento", "Has perdido");
    }
}
