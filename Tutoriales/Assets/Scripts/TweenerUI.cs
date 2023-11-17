using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum UIAnimationType
{
    MoveTransform,
    Scale,
    ScaleX,
    ScaleY,
    Fade,
    Size,
    Rotation
}

public class TweenerUI : MonoBehaviour
{
    public GameObject objectToAnimate;
    private RectTransform rectTransform;
    public UIAnimationType animationType;
    public Ease easeType;
    public float duration;
    public float delay;

    public bool loop;
    public bool pingpong;

    public bool startPositionOffset;
    public Vector3 from;
    public Vector3 to;

    private Tweener tweener;

    public bool showOnEnable;
    public bool workOnDisable;

    private void Awake()
    {
        Init();

        if (!startPositionOffset && animationType == UIAnimationType.MoveTransform)
        {
            SwapDirection();
            MoveAbsolute();
            SwapDirection();
        }

    }

    private void Init()
    {
        if (objectToAnimate == null)
        {
            objectToAnimate = gameObject;
        }

        if (rectTransform == null)
        {
            rectTransform = objectToAnimate.GetComponent<RectTransform>();
        }
    }

    private void OnEnable()
    {
        if (showOnEnable)
        {
            Show();
        }
    }

    public void OnDisable()
    {
        if (workOnDisable)
        {
            Disable();
        }
    }

    private void OnDestroy()
    {
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
        }
    }

    public TweenerUI Show()
    {
        Init();

        HandleTween();
        return this;
    }

    private void HandleTween()
    {
        switch (animationType)
        {
            case UIAnimationType.MoveTransform:
                MoveAbsolute();
                break;
            case UIAnimationType.Scale:
                Scale();
                break;
            case UIAnimationType.ScaleX:
                Scale();
                break;
            case UIAnimationType.ScaleY:
                Scale();
                break;
            case UIAnimationType.Fade:
                Fade();
                break;
            case UIAnimationType.Size:
                Size();
                break;
            case UIAnimationType.Rotation:
                Rotation();
                break;
            default:
                break;
        }

        if (loop)
        {
            tweener.SetLoops(-1);
        }

        if (pingpong)
        {
            tweener.SetLoops(-1, LoopType.Yoyo);
        }
    }

    private void Rotation()
    {
        if (startPositionOffset)
        {
            rectTransform.rotation = Quaternion.Euler(from);
        }

        tweener = rectTransform.DORotate(to, duration)
            .SetEase(easeType)
            .SetDelay(delay);
    }

    private void Size()
    {
        if (startPositionOffset)
        {
            rectTransform.sizeDelta = from;
        }

        tweener = rectTransform.DOSizeDelta(to, duration)
            .SetEase(easeType)
            .SetDelay(delay);
    }

    private void Fade()
    {
        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }

        if (startPositionOffset)
        {
            objectToAnimate.GetComponent<CanvasGroup>().alpha = from.x;
        }

        tweener = objectToAnimate.GetComponent<CanvasGroup>().DOFade(to.x, duration)
            .SetEase(easeType)
            .SetDelay(delay);

    }

    private void Scale()
    {
        if (startPositionOffset)
        {
            rectTransform.localScale = from;
        }

        tweener = rectTransform.DOScale(to, duration)
            .SetEase(easeType)
            .SetDelay(delay);

    }

    private void MoveAbsolute()
    {
        if (startPositionOffset)
        {
            rectTransform.transform.position = from;
        }

        tweener = rectTransform.DOAnchorPos(to, duration)
            .SetEase(easeType)
            .SetDelay(delay);

    }

    public TweenerUI SetDelay(int newDelay)
    {
        delay = newDelay;

        return this;
    }

    public TweenerUI SwapDirection()
    {
        var temp = from;
        from = to;
        to = temp;

        return this;
    }

    public TweenerUI ForceReset()
    {
        tweener.Kill();

        return this;
    }

    public TweenerUI Disable()
    {
        SwapDirection();

        HandleTween();

        tweener.OnComplete(() =>
        {
            SwapDirection();
            gameObject.SetActive(false);
        });

        return this;
    }

    public TweenerUI Disable(GameObject gameObject)
    {
        SwapDirection();

        HandleTween();

        tweener.OnComplete(() =>
        {
            SwapDirection();
            gameObject.SetActive(false);
        });

        return this;
    }

    public TweenerUI Disable(Action onCompleteAction)
    {
        SwapDirection();

        HandleTween();

        tweener.OnComplete(() =>
        {
            onCompleteAction?.Invoke();
        });

        return this;
    }

    public TweenerUI OnComplete(Action onCompleteAction)
    {
        tweener.OnComplete(() =>
        {
            onCompleteAction?.Invoke();
        });

        return this;
    }

}
