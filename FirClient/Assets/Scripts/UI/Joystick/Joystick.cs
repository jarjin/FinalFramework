using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(EasyTouch))]
public class Joystick : GameBehaviour
{
    public float radius = 60.0f;
    public float percent = 0.7f;
    public Vector2 returnSpeed = new Vector2(10, 10);

    /// <summary>
    /// 外部监听函数
    /// </summary>
    public static event Action<Vector2> OnJoystickStart;
    public static event Action<Vector2, float> OnJoystickMove;
    public static event Action OnJoystickEnd;

    private Camera uiCamera;
    private Vector2 startPos;
    private RectTransform canvas;
    private RectTransform background;
    private RectTransform handler;
    private RectTransform mTrans;
    private bool isDragging = false;
    private bool returnHandle = true;
    private int fingerIndex = 0;

    public Vector2 Coordinates
    {
        get
        {
            Vector2 vec;
            if (handler.anchoredPosition.magnitude < radius)
                vec = handler.anchoredPosition / radius;
            else
                vec = handler.anchoredPosition;
            //vec = Quaternion.Euler(0, 0, -60) * vec;
            return vec.normalized;
        }
    }

    Camera UICamera
    {
        get
        {
            if (uiCamera == null)
                uiCamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
            return uiCamera;
        }
    }

    void Awake()
    {
        mTrans = GetComponent<RectTransform>();
        startPos = mTrans.anchoredPosition;
        //canvas = ManagementCenter.uiCanvas.GetComponent<RectTransform>();
        background = transform.Find("Background").GetComponent<RectTransform>();
        handler = transform.Find("Handler").GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchUp += On_TouchUp;
        EasyTouch.On_TouchDown += On_TouchMove;
    }

    void OnDisable()
    {
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_TouchDown -= On_TouchMove;
    }

    void Update()
    {
        if (returnHandle)
        {
            if (handler.anchoredPosition.magnitude > Mathf.Epsilon)
            {
                float x = handler.anchoredPosition.x * returnSpeed.x;
                float y = handler.anchoredPosition.y * returnSpeed.y;
                handler.anchoredPosition -= new Vector2(x, y) * Time.deltaTime;
            }
            else
            {
                returnHandle = false;
            }
        }
    }

    /// <summary>
    /// 获取两点之间的一个点
    /// </summary>
    private Vector3 BetweenPoint(Vector3 start, Vector3 end, float distance)
    {
        Vector3 normal = (end - start).normalized;
        return normal * distance + start;
    }

    /// <summary>
    /// 获取超出位置
    /// </summary>
    /// <returns></returns>
    Vector2 GetOverstepPos(Vector3 inPoint)
    {
        if (inPoint.x > Screen.width * percent)
        {
            inPoint.x = Screen.width * percent;
        }
        Vector2 outPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, inPoint, UICamera, out outPoint))
        {
            outPoint = new Vector2(outPoint.x + canvas.pivot.x * canvas.rect.width,
                           outPoint.y + canvas.pivot.y * canvas.rect.height);

            var distance = (outPoint - mTrans.anchoredPosition).magnitude;
            if (distance > radius)
            {
                return BetweenPoint(outPoint, mTrans.anchoredPosition, radius);
            }
            else
            {
                return Vector2.zero;
            }
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 指尖单击
    /// </summary>
    void On_TouchStart(Gesture gesture)
    {
        if (gesture.position.x <= Screen.width * percent)
        {
            if (gesture.touchCount > 1)
                return;
            returnHandle = false;
            isDragging = true;
            Vector2 outPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, gesture.position, UICamera, out outPoint))
            {
                outPoint = new Vector2(outPoint.x + canvas.pivot.x * canvas.rect.width,
                           outPoint.y + canvas.pivot.y * canvas.rect.height);

                var distance = (outPoint - mTrans.anchoredPosition).magnitude;
                if (distance > radius)
                {
                    mTrans.anchoredPosition = outPoint;
                }
                else
                {
                    handler.anchoredPosition = GetJoystickOffset(gesture.position);
                }
                if (OnJoystickStart != null)
                {
                    OnJoystickStart(Coordinates);
                }
            }
            if (fingerIndex == -1)
            {
                fingerIndex = gesture.fingerIndex;
            }
        }
    }

    /// <summary>
    /// 指尖移动
    /// </summary>
    void On_TouchMove(Gesture gesture)
    {
        if (gesture.fingerIndex != fingerIndex)
            return;
        if (returnHandle == true && !isDragging)
            return;

        if (gesture.position.x <= Screen.width * percent || isDragging)
        {
            Vector2 newPos = GetOverstepPos(gesture.position);
            if (newPos != Vector2.zero)
            {       
                //mTrans.anchoredPosition = Vector2.Lerp(mTrans.anchoredPosition, newPos, 1.5f);
            }
            handler.anchoredPosition = GetJoystickOffset(gesture.position);

            Vector2 a = new Vector2(0, 1);
            Vector2 b = Coordinates.normalized;
            float angle = Vector2.Angle(a, b);
            angle *= Mathf.Sign(Vector3.Cross(a, b).z);
            background.localEulerAngles = new Vector3(0, 0, angle);

            if (OnJoystickMove != null)
            {
                OnJoystickMove(Coordinates, angle);
            }
        }
    }

    /// <summary>
    /// 指尖抬起
    /// </summary>
    void On_TouchUp(Gesture gesture)
    {
        if (gesture.fingerIndex != fingerIndex)
            return;
        isDragging = false;
        returnHandle = true;
        fingerIndex = -1;
        mTrans.anchoredPosition = startPos;
        background.localEulerAngles = Vector3.zero;
        if (OnJoystickEnd != null)
        {
            OnJoystickEnd();
        }
    }

    private Vector2 GetJoystickOffset(Vector2 inPos)
    {
        Vector3 globalHandle;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, inPos, UICamera, out globalHandle))
        {
            handler.position = globalHandle;
        }
        var handleOffset = handler.anchoredPosition;

        if (handleOffset.magnitude > radius)
        {
            handleOffset = handleOffset.normalized * radius;
            handler.anchoredPosition = handleOffset;
        }
        return handleOffset;
    }
}