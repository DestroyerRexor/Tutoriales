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

    private bool waitingForInput = false;
    private int timesToWin = 5;
    private float secondsToShow = 1f;
    private float secondsToHide = 0.5f;

    private void OnEnable()
    {
        faceImage.sprite = smileFace;
        GetTimesByDifficulty();
        StartCoroutine(InitSimonSays());
    }

    private void OnDisable()
    {
        simonSaysOrder.Clear();
        simonSaysPlayerOrder.Clear();

        foreach (SimonSaysSingleUI simonSaysSingleUI in simonSaysSingleList)
        {
            simonSaysSingleUI.RemoveListener();
        }

        StopCoroutine(InitSimonSays());
    }

    private IEnumerator InitSimonSays()
    {
        #region PREPARANDO SIMON DICE
        foreach (SimonSaysSingleUI simonSaysSingleUI in simonSaysSingleList)
        {
            simonSaysSingleUI.DisableButton();
        }

        yield return new WaitForSeconds(1.5f);

        #endregion

        #region GENERAR UN COLOR ALEATORIO Y AÑADIRLO A LISTA
        SimonSaysColor simonSays = GenerateRandomColor();
        simonSaysOrder.Add(simonSays);
        #endregion

        #region RECORRER LISTA DE COLORES PASADOS, SI ENCUENTRA ILUMINARLO Y ACTIVAR CARITA
        for (int i = 0; i < simonSaysOrder.Count; i++)
        {
            SimonSaysSingleUI simonSaysSingle = simonSaysSingleList.Find(x => x.GetSimonSaysColor() == simonSaysOrder[i]);

            if (simonSaysSingle == null) continue;

            faceImage.sprite = singFace;
            simonSaysSingle.ShowColor();
            yield return new WaitForSeconds(secondsToShow);
            faceImage.sprite = smileFace;
            simonSaysSingle.HideColor();
            yield return new WaitForSeconds(secondsToHide);

        }
        #endregion

        #region ACTIVAR INTERACCION CON BOTONES
        foreach (SimonSaysSingleUI simonSaysSingleUI in simonSaysSingleList)
        {
            simonSaysSingleUI.EnableButton();
        }
        #endregion

        #region ESPERAR A QUE EL JUGADOR PULSE UN BOTÓN
        for (int i = 0; i < simonSaysOrder.Count; i++)
        {
            waitingForInput = true;
            while (waitingForInput)
            {
                yield return null;
            }
        }
        #endregion

        #region DESACTIVAR INTERACCION CON BOTONES
        foreach (SimonSaysSingleUI simonSaysSingleUI in simonSaysSingleList)
        {
            simonSaysSingleUI.DisableButton();
        }
        #endregion

        #region VERIFICAR SI EL ORDEN ES CORRECTO
        bool success = true;
        for (int i = 0; i < simonSaysOrder.Count; i++)
        {
            if (simonSaysOrder[i] != simonSaysPlayerOrder[i])
            {
                success = false;
                break;
            }
        }
        #endregion

        #region SI EL ORDEN ES CORRECTO PASAMOS A LA SIGUIENTE RONDA Y LIMPIAMOS LA LISTA DEL JUGADOR Y VOLVEMOS A INICIAR EL JUEGO, CASO CONTRARIO MARCAR COMO PERDIDO
        if (success)
        {
            timesToWin--;
            simonSaysPlayerOrder.Clear();

            if (IsGameOver())
            {
                OnWinGame?.Invoke(this, System.EventArgs.Empty);
            }
            else
            {
                StartCoroutine(InitSimonSays());
            }
        }
        else
        {
            OnLoseGame?.Invoke(this, System.EventArgs.Empty);
        }
        #endregion

    }

    private int GetTimesByDifficulty()
    {
        switch (SimonSaysManagerUI.Instance.GetDifficulty())
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
    public void OnColorSelected(SimonSaysSingleUI simonSaysSingleUI)
    {
        simonSaysSingleUI.Select();

        simonSaysPlayerOrder.Add(simonSaysSingleUI.GetSimonSaysColor());

        waitingForInput = false;

    }
    public bool GetWaitingForInput() => waitingForInput;
    private SimonSaysColor GenerateRandomColor() => (SimonSaysColor)Random.Range(0, 4);
    private bool IsGameOver() => timesToWin <= 0;

}
