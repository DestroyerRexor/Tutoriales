using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance { get; private set; }

    [SerializeField] private TMPro.TMP_Text timerText;

    private float timerCountMax = 360;
    private float timerCount;
    private float timerCounting;

    private int minutes, seconds, cents;

    private bool timeOver = false;

    public event System.Action OnTimeOver;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }

    private void OnEnable()
    {
        timerCount = timerCountMax;
        timerCounting = 0;
        timeOver = false;

        if (timerText != null)
        {
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(StartTimer());
        }
    }

    private void OnDisable()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
    }

    public TimerUI SetMaxTimer(float timerMax)
    {
        timerCountMax = timerMax;
        timerCount = timerCountMax;
        return this;
    }

    public TimerUI SetTimerText(TMP_Text timerText)
    {
        this.timerText = timerText;
        return this;
    }

    public TimerUI Play()
    {
        if (timerText == null) return this;

        timeOver = false;
        timerCounting = 0;

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(StartTimer());

        return this;
    }

    private IEnumerator StartTimer()
    {
        while (!timeOver)
        {
            timerCount -= Time.deltaTime;
            timerCounting += Time.deltaTime;

            if (timerCount < 0) timerCount = 0;

            minutes = (int)(timerCount / 60f);
            seconds = (int)(timerCount - minutes * 60f);
            cents = (int)((timerCount - (int)timerCount) * 100f);
            timerText.text = string.Format("{0:00}.{1:00}.{2:00}", minutes, seconds, cents);

            if (timerCount == 0)
            {
                timeOver = true;
                OnTimeOver?.Invoke();
            }

            yield return null;
        }

        timerCoroutine = null;
    }

    public void SetTimeOver(bool isTimeOver = true)
    {
        timeOver = isTimeOver;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
