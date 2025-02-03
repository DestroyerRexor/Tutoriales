using UnityEngine;

public class SimonSaysManagerUI : MinigamesBase
{
    protected override string minigameName => "Simon Says";

    [Space(20)]
    [SerializeField] private SimonSaysGroup simonSaysGroup;

    private void SimonSaysGroup_OnWinGame(object sender, System.EventArgs e)
    {
        GameWin(10, "Felicidades", "Ganaste");
    }

    private void SimonSaysGroup_OnLoseGame(object sender, System.EventArgs e)
    {
        GameLose("Lo siento", "Has perdido");
    }

    public override void Show()
    {
        base.Show();
        simonSaysGroup.Init(this);
        InitializeDifficulty();

        simonSaysGroup.OnLoseGame += SimonSaysGroup_OnLoseGame;
        simonSaysGroup.OnWinGame += SimonSaysGroup_OnWinGame;

    }

    public override void Hide()
    {
        base.Hide();

        simonSaysGroup.OnLoseGame -= SimonSaysGroup_OnLoseGame;
        simonSaysGroup.OnWinGame -= SimonSaysGroup_OnWinGame;

    }
}
