using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private MinigamesHandler minigamesHandler;
    [Space]
    [SerializeField] private GameObject layout;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMPro.TMP_Text titleText;
    [SerializeField] private TMPro.TMP_Text descriptionText;
    //[SerializeField] private Button restartButton;

    public event System.Action OnClose;
    public event System.Action OnRestart;

    private void Start()
    {
        closeButton.onClick.AddListener(Close);

        Hide();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        minigamesHandler.Show();
        Hide();
        OnClose?.Invoke();
        OnRestart?.Invoke();
    }

    public GameOverManager Setup(string title, string description, System.Action onCloseCallback = null)
    {
        titleText.SetText(title);
        descriptionText.SetText(description);

        OnClose = onCloseCallback;

        return this;
    }

    private void Hide()
    {
        layout.SetActive(false);
    }

    public void Show()
    {
        layout.SetActive(true);
    }
}
