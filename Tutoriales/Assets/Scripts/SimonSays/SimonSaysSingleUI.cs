using UnityEngine;
using UnityEngine.UI;

public class SimonSaysSingleUI : MonoBehaviour
{
    [SerializeField] private SimonSaysGroup simonSaysGroup;

    [SerializeField] private Sprite disableSprite;
    [SerializeField] private Sprite enableSprite;

    [SerializeField] private SimonSaysColor simonSaysColor;

    private Button simonSayButton;
    private Image simonSayImage;
    private TweenerUI tweenerUI;

    private void Awake()
    {
        simonSayButton = GetComponent<Button>();
        simonSayImage = GetComponent<Image>();
        tweenerUI = GetComponent<TweenerUI>();

        if (simonSaysGroup == null)
        {
            simonSaysGroup = transform.parent.GetComponent<SimonSaysGroup>();
        }
    }

    private void Start()
    {
        DisableButton();
    }

    private void OnEnable()
    {
        simonSayButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        simonSayButton.onClick.RemoveListener(OnButtonClicked);
        HideColor();
    }

    private void OnButtonClicked()
    {
        simonSaysGroup.OnColorButtonPressed(simonSaysColor);

        PlayVisualFeedback();
    }

    public void Select()
    {
        PlayVisualFeedback();
    }

    private void PlayVisualFeedback()
    {
        ShowColor();

        tweenerUI.Show().OnComplete(() =>
        {
            HideColor();
        });
    }

    public void ShowColor() => simonSayImage.sprite = enableSprite;
    public void HideColor() => simonSayImage.sprite = disableSprite;

    public void DisableButton() => simonSayButton.interactable = false;
    public void EnableButton() => simonSayButton.interactable = true;

    public SimonSaysColor GetSimonSaysColor() => simonSaysColor;
}
