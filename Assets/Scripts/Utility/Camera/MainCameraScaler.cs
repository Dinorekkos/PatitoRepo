using UnityEngine;

[ExecuteInEditMode]
public class MainCameraScaler : MonoBehaviour
{
    public bool IsEnabled = false;
    public Camera myCamera;
    public int targetWidth = 750;
    public int targetHeight = 1334;
    [SerializeField]
    private float orthographicSize = 5;
    public ScaleType type;

    public float normalAspect;
    public float currentAspect;

    private float currentOrthographicSize;

    public bool viewOnUpdate = true;

    public float OrthographicSize
    {
        get
        {
            return orthographicSize;
        }
        set
        {
            orthographicSize = value;
            UpdateScale();
        }
    }

    void UpdateScale()
    {
        normalAspect = ((float)targetWidth / (float)targetHeight);
        currentAspect = ((float)Screen.width / (float)Screen.height);

        if (!IsEnabled)
        {
            return;
        }

        switch (type)
        {
            case ScaleType.Width:
                ScaleWidth();
                break;
            case ScaleType.Height:
                ScaleHeight();
                break;
            case ScaleType.Middle:
                ScaleMiddle();
                break;
            case ScaleType.AspectRatioDepending:
                if (currentAspect >= 0.5)
                {
                    ScaleHeight();
                }
                else
                {
                    ScaleWidth();
                }
                break;
        }
    }

    void ScaleWidth()
    {
        currentOrthographicSize = OrthographicSize * normalAspect / currentAspect;
        UpdateCameraSize();
    }
    void ScaleHeight()
    {
        currentOrthographicSize = OrthographicSize;
        UpdateCameraSize();
    }
    void ScaleMiddle()
    {
        currentOrthographicSize = (OrthographicSize + (OrthographicSize * normalAspect / currentAspect)) / 2;
        UpdateCameraSize();
    }
    void UpdateCameraSize()
    {
        Debug.Assert(myCamera != null, "My Camera is not asigned for gameobject: " + gameObject.name);
        myCamera.orthographicSize = currentOrthographicSize;
    }

    public static MainCameraScaler Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScale();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (viewOnUpdate)
        {
            UpdateScale();
        }
    }
#endif
}