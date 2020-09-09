using UnityEngine;
using UnityEngine.UI;

public class CCanvasAlpha : MonoBehaviour
{
    public float alpha = 1;
    public bool blocksRaycasts = true;

    private float _alpha = 0;
    private bool _blocksRaycasts = false;
    private Image _background = null;

    private float Alpha
    {
        get { return _alpha; }
        set {
            _alpha = value;
            UpdateAlpha(alpha);
        }
    }

    private void Awake()
    {
        _background = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateAlpha(alpha);
        if (_background != null)
        {
            _background.raycastTarget = blocksRaycasts;
        }
    }

    private void OnEnable()
    {
        if (Alpha != alpha)
        {
            Alpha = alpha;
        }
        UpdateAlpha(alpha);
    }

    private void Update()
    {
        if (Alpha != alpha)
        {
            Alpha = alpha;
        }
        if (_blocksRaycasts != blocksRaycasts)
        {
            UpdateBlocksRaycasts();
        }
    }

    private void UpdateBlocksRaycasts()
    {
        _blocksRaycasts = blocksRaycasts;
        if (_background != null)
        {
            _background.raycastTarget = blocksRaycasts;
        }
    }

    private void UpdateAlpha(float v)
    {
        var canvasRenders = GetComponentsInChildren<CanvasRenderer>();
        if (canvasRenders != null)
        {
            for(int i = 0; i < canvasRenders.Length; i++)
            {
                if (canvasRenders[i] != null)
                {
                    canvasRenders[i].SetAlpha(v);
                }
            }
        }
    }
}
