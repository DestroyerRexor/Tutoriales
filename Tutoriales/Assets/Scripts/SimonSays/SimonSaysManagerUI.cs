using UnityEngine;

[DefaultExecutionOrder(1)]
public class SimonSaysManagerUI : MinigamesBase
{
    public static SimonSaysManagerUI Instance { get; private set; }

    [Space(20)]
    [SerializeField] private SimonSaysGroup simonSaysGroup;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Toggle(false);
    }

    private void OnEnable()
    {
        InitializeDifficulty();

        simonSaysGroup.OnLoseGame += SimonSaysGroup_OnLoseGame;
        simonSaysGroup.OnWinGame += SimonSaysGroup_OnWinGame;
    }

    private void OnDisable()
    {
        simonSaysGroup.OnLoseGame -= SimonSaysGroup_OnLoseGame;
        simonSaysGroup.OnWinGame -= SimonSaysGroup_OnWinGame;
    }

    private void InitializeDifficulty()
    {
        DifficultyManager.Instance
            .ResetListeners()
            .OnCloseButtonClick(() =>
            {
                Toggle(false);
                ToggleGameArea(false);
                DifficultyManager.Instance.Toggle(false);
                DifficultyManager.Instance.HideCloseButton();
            })
            .OnEasyButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                Toggle(true);
                ToggleGameArea(true);
            })
            .OnNormalButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                Toggle(true);
                ToggleGameArea(true);
            })
            .OnHardButtonClick(() =>
            {
                DifficultyManager.Instance.Toggle(false);
                Toggle(true);
                ToggleGameArea(true);
            });
    }

    private void SimonSaysGroup_OnWinGame(object sender, System.EventArgs e)
    {
        GameWin(10, "Felicidades", "Ganaste");
    }

    private void SimonSaysGroup_OnLoseGame(object sender, System.EventArgs e)
    {
        GameLose("Lo siento", "Has perdido");
    }
}
