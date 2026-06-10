using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SimonSaysColor
{
    Red,
    Blue,
    Yellow,
    Green
}

public class SimonSaysGroup : MonoBehaviour
{

    public event System.EventHandler OnWinGame;
    public event System.EventHandler OnLoseGame;

    private List<SimonSaysColor> simonSaysOrder = new List<SimonSaysColor>();
    private List<SimonSaysColor> simonSaysPlayerOrder = new List<SimonSaysColor>();

    [SerializeField] private Image faceImage;
    [SerializeField] private Sprite smileFace;
    [SerializeField] private Sprite singFace;

    [SerializeField] private List<SimonSaysSingleUI> simonSaysSingleList = new List<SimonSaysSingleUI>();

    private int timesToWin = 5;
    private float secondsToShow = 1f;
    private float secondsToHide = 0.5f;

    private SimonSaysManagerUI simonSaysManager;

    private Dictionary<SimonSaysColor, SimonSaysSingleUI> _colorToUI = new Dictionary<SimonSaysColor, SimonSaysSingleUI>();

    private Coroutine _sequenceCoroutine;
    private int _currentPlayerStep = 0;

    public void Init(SimonSaysManagerUI simonSaysManagerUI)
    {
        simonSaysManager = simonSaysManagerUI;
        faceImage.sprite = smileFace;

        _colorToUI.Clear();

        foreach (SimonSaysSingleUI singleUI in simonSaysSingleList)
        {
            _colorToUI.Add(singleUI.GetSimonSaysColor(), singleUI);
        }

        GetTimesByDifficulty();

        _sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    private void OnDisable()
    {
        simonSaysOrder.Clear();
        simonSaysPlayerOrder.Clear();

        if (_sequenceCoroutine != null) StopCoroutine(_sequenceCoroutine);
    }

    private IEnumerator PlaySequence()
    {
        foreach (SimonSaysSingleUI singleUI in simonSaysSingleList)
        {
            singleUI.DisableButton();
        }

        yield return new WaitForSeconds(1.5f);

        simonSaysOrder.Add(GenerateRandomColor());

        for (int i = 0; i < simonSaysOrder.Count; i++)
        {
            SimonSaysColor colorToPlay = simonSaysOrder[i];

            if (_colorToUI.TryGetValue(colorToPlay, out SimonSaysSingleUI simonSaysSingle))
            {
                faceImage.sprite = singFace;
                simonSaysSingle.Select();

                yield return new WaitForSeconds(secondsToShow);

                faceImage.sprite = smileFace;
                yield return new WaitForSeconds(secondsToHide);
            }
        }

        _currentPlayerStep = 0;
        simonSaysPlayerOrder.Clear();

        foreach (SimonSaysSingleUI singleUI in simonSaysSingleList)
        {
            singleUI.EnableButton();
        }
    }

    public void OnColorButtonPressed(SimonSaysColor pressedColor)
    {
        simonSaysPlayerOrder.Add(pressedColor);

        if (pressedColor != simonSaysOrder[_currentPlayerStep])
        {
            OnLoseGame?.Invoke(this, System.EventArgs.Empty);
            return;
        }

        _currentPlayerStep++;

        if (_currentPlayerStep >= simonSaysOrder.Count)
        {
            timesToWin--;

            foreach (SimonSaysSingleUI singleUI in simonSaysSingleList)
            {
                singleUI.DisableButton();
            }

            if (IsGameOver())
            {
                OnWinGame?.Invoke(this, System.EventArgs.Empty);
            }
            else
            {
                _sequenceCoroutine = StartCoroutine(PlaySequence());
            }
        }
    }

    private int GetTimesByDifficulty()
    {
        switch (simonSaysManager.GetDifficulty())
        {
            case DifficultyEnum.Easy:
                timesToWin = 5;
                secondsToShow = 0.6f;
                secondsToHide = 0.5f;
                break;
            case DifficultyEnum.Normal:
                timesToWin = 7;
                secondsToShow = 0.3f;
                secondsToHide = 0.2f;
                break;
            case DifficultyEnum.Hard:
                timesToWin = 12;
                secondsToShow = 0.2f;
                secondsToHide = 0.1f;
                break;
            default:
                break;
        }

        return timesToWin;
    }

    private SimonSaysColor GenerateRandomColor() => (SimonSaysColor)Random.Range(0, 4);
    private bool IsGameOver() => timesToWin <= 0;

}
