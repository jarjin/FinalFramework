using UnityEngine;
using System.Collections;

/// <summary>
/// This is the main class, you need to add it to your main camera or on a empty gameobject in your scene (or use Hedgehog Team menu).
/// </summary>
public class EasyTouch : MonoBehaviour {

    public delegate void TouchCancelHandler(Gesture gesture);
    public delegate void Cancel2FingersHandler(Gesture gesture);
    public delegate void TouchStartHandler(Gesture gesture);
    public delegate void TouchDownHandler(Gesture gesture);
    public delegate void TouchUpHandler(Gesture gesture);
    public delegate void SimpleTapHandler(Gesture gesture);
    public delegate void DoubleTapHandler(Gesture gesture);
    public delegate void LongTapStartHandler(Gesture gesture);
    public delegate void LongTapHandler(Gesture gesture);
    public delegate void LongTapEndHandler(Gesture gesture);
    public delegate void DragStartHandler(Gesture gesture);
    public delegate void DragHandler(Gesture gesture);
    public delegate void DragEndHandler(Gesture gesture);
    public delegate void SwipeStartHandler(Gesture gesture);
    public delegate void SwipeHandler(Gesture gesture);
    public delegate void SwipeEndHandler(Gesture gesture);
    public delegate void TouchStart2FingersHandler(Gesture gesture);
    public delegate void TouchDown2FingersHandler(Gesture gesture);
    public delegate void TouchUp2FingersHandler(Gesture gesture);
    public delegate void SimpleTap2FingersHandler(Gesture gesture);
    public delegate void DoubleTap2FingersHandler(Gesture gesture);
    public delegate void LongTapStart2FingersHandler(Gesture gesture);
    public delegate void LongTap2FingersHandler(Gesture gesture);
    public delegate void LongTapEnd2FingersHandler(Gesture gesture);
    public delegate void TwistHandler(Gesture gesture);
    public delegate void TwistEndHandler(Gesture gesture);
    public delegate void PinchInHandler(Gesture gesture);
    public delegate void PinchOutHandler(Gesture gesture);
    public delegate void PinchEndHandler(Gesture gesture);
    public delegate void DragStart2FingersHandler(Gesture gesture);
    public delegate void Drag2FingersHandler(Gesture gesture);
    public delegate void DragEnd2FingersHandler(Gesture gesture);
    public delegate void SwipeStart2FingersHandler(Gesture gesture);
    public delegate void Swipe2FingersHandler(Gesture gesture);
    public delegate void SwipeEnd2FingersHandler(Gesture gesture);

    /// <summary>
    /// Occurs when The system cancelled tracking for the touch, as when (for example) the user puts the device to her face.
    /// </summary>
    public static event TouchCancelHandler On_Cancel;
    /// <summary>
    /// Occurs when the touch count is no longer egal to 2 and different to 0, after the begining of a two fingers gesture.
    /// </summary>
    public static event Cancel2FingersHandler On_Cancel2Fingers;
    /// <summary>
    /// Occurs when a finger touched the screen.
    /// </summary>
    public static event TouchStartHandler On_TouchStart;
    /// <summary>
    /// Occurs as the touch is active.
    /// </summary>
    public static event TouchDownHandler On_TouchDown;
    /// <summary>
    /// Occurs when a finger was lifted from the screen.
    /// </summary>
    public static event TouchUpHandler On_TouchUp;
    /// <summary>
    /// Occurs when a finger was lifted from the screen, and the time elapsed since the beginning of the touch is less than the time required for the detection of a long tap.
    /// </summary>
    public static event SimpleTapHandler On_SimpleTap;
    /// <summary>
    /// Occurs when the number of taps is egal to 2 in a short time.
    /// </summary>
    public static event DoubleTapHandler On_DoubleTap;
    /// <summary>
    /// Occurs when a finger is touching the screen,  but hasn't moved  since the time required for the detection of a long tap.
    /// </summary>
    public static event LongTapStartHandler On_LongTapStart;
    /// <summary>
    /// Occurs as the touch is active after a LongTapStart
    /// </summary>
    public static event LongTapHandler On_LongTap;
    /// <summary>
    /// Occurs when a finger was lifted from the screen, and the time elapsed since the beginning of the touch is more than the time required for the detection of a long tap.
    /// </summary>
    public static event LongTapEndHandler On_LongTapEnd;
    /// <summary>
    /// Occurs when a drag start. A drag is a swipe on a pickable object
    /// </summary>
    public static event DragStartHandler On_DragStart;
    /// <summary>
    /// Occurs as the drag is active.
    /// </summary>
    public static event DragHandler On_Drag;
    /// <summary>
    /// Occurs when a finger that raise the drag event , is lifted from the screen.
    /// </summary>/
    public static event DragEndHandler On_DragEnd;
    /// <summary>
    /// Occurs when swipe start.
    /// </summary>
    public static event SwipeStartHandler On_SwipeStart;
    /// <summary>
    /// Occurs as the  swipe is active.
    /// </summary>
    public static event SwipeHandler On_Swipe;
    /// <summary>
    /// Occurs when a finger that raise the swipe event , is lifted from the screen.
    /// </summary>
    public static event SwipeEndHandler On_SwipeEnd;
    /// <summary>
    /// Like On_TouchStart but for a 2 fingers gesture.
    /// </summary>
    public static event TouchStart2FingersHandler On_TouchStart2Fingers;
    /// <summary>
    /// Like On_TouchDown but for a 2 fingers gesture.
    /// </summary>
    public static event TouchDown2FingersHandler On_TouchDown2Fingers;
    /// <summary>
    /// Like On_TouchUp but for a 2 fingers gesture.
    /// </summary>
    public static event TouchUp2FingersHandler On_TouchUp2Fingers;
    /// <summary>
    /// Like On_SimpleTap but for a 2 fingers gesture.
    /// </summary>
    public static event SimpleTap2FingersHandler On_SimpleTap2Fingers;
    /// <summary>
    /// Like On_DoubleTap but for a 2 fingers gesture.
    /// </summary>
    public static event DoubleTap2FingersHandler On_DoubleTap2Fingers;
    /// <summary>
    /// Like On_LongTapStart but for a 2 fingers gesture.
    /// </summary>
    public static event LongTapStart2FingersHandler On_LongTapStart2Fingers;
    /// <summary>
    /// Like On_LongTap but for a 2 fingers gesture.
    /// </summary>
    public static event LongTap2FingersHandler On_LongTap2Fingers;
    /// <summary>
    /// Like On_LongTapEnd but for a 2 fingers gesture.
    /// </summary>
    public static event LongTapEnd2FingersHandler On_LongTapEnd2Fingers;
    /// <summary>
    /// Occurs when a twist gesture start
    /// </summary>
    public static event TwistHandler On_Twist;
    /// <summary>
    /// Occurs as the twist gesture is active.
    /// </summary>
    public static event TwistEndHandler On_TwistEnd;
    /// <summary>
    /// Occurs as the twist in gesture is active.
    /// </summary>
    public static event PinchInHandler On_PinchIn;
    /// <summary>
    /// Occurs as the pinch out gesture is active.
    /// </summary>
    public static event PinchOutHandler On_PinchOut;
    /// <summary>
    /// Occurs when the 2 fingers that raise the pinch event , are lifted from the screen.
    /// </summary>
    public static event PinchEndHandler On_PinchEnd;
    /// <summary>
    /// Like On_DragStart but for a 2 fingers gesture.
    /// </summary>
    public static event DragStart2FingersHandler On_DragStart2Fingers;
    /// <summary>
    /// Like On_Drag but for a 2 fingers gesture.
    /// </summary>
    public static event Drag2FingersHandler On_Drag2Fingers;
    /// <summary>
    /// Like On_DragEnd2Fingers but for a 2 fingers gesture.
    /// </summary>
    public static event DragEnd2FingersHandler On_DragEnd2Fingers;
    /// <summary>
    /// Like On_SwipeStart but for a 2 fingers gesture.
    /// </summary>
    public static event SwipeStart2FingersHandler On_SwipeStart2Fingers;
    /// <summary>
    /// Like On_Swipe but for a 2 fingers gesture.
    /// </summary>
    public static event Swipe2FingersHandler On_Swipe2Fingers;
    /// <summary>
    /// Like On_SwipeEnd but for a 2 fingers gesture.
    /// </summary>
    public static event SwipeEnd2FingersHandler On_SwipeEnd2Fingers;

    public enum GestureType { Tap, Drag, Swipe, None, LongTap, Pinch, Twist, Cancel, Acquisition };
    /// <summary>
    /// Represents the different directions for a swipe or drag gesture.
    /// </summary>
    public enum SwipeType { None, Left, Right, Up, Down, Other };

    /// <summary>
    /// Event name.
    /// </summary>
    public enum EventName {
        None, On_Cancel, On_Cancel2Fingers, On_TouchStart, On_TouchDown, On_TouchUp, On_SimpleTap, On_DoubleTap, On_LongTapStart, On_LongTap,
        On_LongTapEnd, On_DragStart, On_Drag, On_DragEnd, On_SwipeStart, On_Swipe, On_SwipeEnd, On_TouchStart2Fingers, On_TouchDown2Fingers, On_TouchUp2Fingers, On_SimpleTap2Fingers,
        On_DoubleTap2Fingers, On_LongTapStart2Fingers, On_LongTap2Fingers, On_LongTapEnd2Fingers, On_Twist, On_TwistEnd, On_PinchIn, On_PinchOut, On_PinchEnd, On_DragStart2Fingers,
        On_Drag2Fingers, On_DragEnd2Fingers, On_SwipeStart2Fingers, On_Swipe2Fingers, On_SwipeEnd2Fingers
    }


    public bool enable = true;				// Enables or disables Easy Touch
    public bool enableRemote = false;		// Enables or disables Unity remote
    public bool useBroadcastMessage = true; //
    public bool joystickAddon = false;
    public bool enable2FingersGesture = true; // Enables 2 fingers gesture.
    public bool enableTwist = true;			// Enables or disables recognition of the twist
    public bool enablePinch = true;			// Enables or disables recognition of the Pinch
    public bool autoSelect = true;  		// Enables or disables auto select
    public LayerMask pickableLayers;		// Layer detectable by default.
    public float StationnaryTolerance = 25f;		// 
    public float longTapTime = 1f;			// The time required for the detection of a long tap.
    public float swipeTolerance = 0.85f;		// Determines the accuracy of detecting a drag movement 0 => no precision 1=> high precision.
    public float minPinchLength = 0f;			// The minimum length for a pinch detection.
    public float minTwistAngle = 1f;			// The minimum angle for a twist detection.

    // Inspectori
    public bool showGeneral = true;
    public bool showSelect = true;
    public bool showGesture = true;
    public bool showTwoFinger = true;

    public static EasyTouch instance;								// Fake singleton

    private EasyTouchInput input;

    private GestureType complexCurrentGesture = GestureType.None; 	// The current gesture 2 fingers
    private GestureType oldGesture = GestureType.None;

    private float startTimeAction;									// The time of onset of action.
    private Finger[] fingers = new Finger[100];						// The informations of the touch for finger 1.

    private Camera mainCam;											// The main camera of the scene, it has possessed the tag "MainCamera".

    private GameObject pickObject2Finger;
    private GameObject oldPickObject2Finger;
    private GameObject receiverObject = null; 						// Other object that can receive messages.

    private Texture secondFingerTexture;							// The texture to display the simulation of the second finger.

    private Vector2 startPosition2Finger;							// Start position for two fingers gesture
    private int twoFinger0;											// finger index
    private int twoFinger1;											// finger index
    private Vector2 oldStartPosition2Finger;
    private float oldFingerDistance;
    private bool twoFingerDragStart = false;
    private bool twoFingerSwipeStart = false;

    private int oldTouchCount = 0;


    public EasyTouch() {
        enable = true;
        useBroadcastMessage = false;
        enable2FingersGesture = true;
        enableTwist = true;
        enablePinch = true;
        autoSelect = true;
        StationnaryTolerance = 25f;
        longTapTime = 1f;
        swipeTolerance = 0.85f;
        minPinchLength = 0f;
        minTwistAngle = 1f;
    }

    void OnEnable() {
        if (Application.isPlaying && Application.isEditor) {
            InitEasyTouch();
        }
    }

    void Start() {
        InitEasyTouch();
    }

    void InitEasyTouch() {
        input = new EasyTouchInput();

        // Assing the fake singleton
        if (EasyTouch.instance == null)
            instance = this;

        // We search the main camera with the tag MainCamera.
        // For automatic object selection.
        mainCam = Camera.main;

        if (mainCam == null) {
            Debug.LogError("No camera with flag \"MainCam\" was found in the scene");
        }

        // The texture to display the simulation of the second finger.
#if ((!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR)
        secondFingerTexture = Resources.Load("secondFinger") as Texture;
#endif
    }

    void OnDrawGizmos() {
    }

    // Non comments.
    void Update() {

        if (enable && EasyTouch.instance == this) {

            int i;

            // How many finger do we have ?
            int count = input.TouchCount();

            // Reste after two finger gesture;
            if (oldTouchCount == 2 && count != 2 && count > 0) {
                CreateGesture2Finger(EventName.On_Cancel2Fingers, Vector2.zero, Vector2.zero, Vector2.zero, 0, SwipeType.None, 0, Vector2.zero, 0, 0);
            }

            // Get touches		
#if (((UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR))
				UpdateTouches(true, count);
#else
            UpdateTouches(false, count);
#endif

            // two fingers gesture
            oldPickObject2Finger = pickObject2Finger;
            if (enable2FingersGesture) {
                if (count == 2) {
                    TwoFinger();
                } else {
                    complexCurrentGesture = GestureType.None;
                    pickObject2Finger = null;
                    twoFingerSwipeStart = false;
                    twoFingerDragStart = false;
                }
            }

            // Other fingers gesture
            for (i = 0; i < 100; i++) {
                if (fingers[i] != null) {
                    OneFinger(i);
                } else {
                    fingers[i] = null;
                }
            }

            oldTouchCount = count;
        }
    }

    void UpdateTouches(bool realTouch, int touchCount) {

        if (realTouch || enableRemote) {
            foreach (Touch touch in Input.touches) {

                if (fingers[touch.fingerId] == null) {
                    fingers[touch.fingerId] = new Finger();
                    fingers[touch.fingerId].fingerIndex = touch.fingerId;
                    fingers[touch.fingerId].gesture = GestureType.None;
                }
                fingers[touch.fingerId].position = touch.position;
                fingers[touch.fingerId].deltaPosition = touch.deltaPosition;
                fingers[touch.fingerId].tapCount = touch.tapCount;
                fingers[touch.fingerId].deltaTime = touch.deltaTime;
                fingers[touch.fingerId].phase = touch.phase;
                fingers[touch.fingerId].touchCount = touchCount;

            }
        } else {
            int i = 0;
            while (i < touchCount) {
                fingers[i] = input.GetMouseTouch(i, fingers[i]) as Finger;
                fingers[i].touchCount = touchCount;
                i++;
            }
        }
    }

    private void OneFinger(int fingerIndex) {

        float timeSinceStartAction = 0;

        // A tap starts ?
        if (fingers[fingerIndex].gesture == GestureType.None) {

            startTimeAction = Time.time;
            fingers[fingerIndex].gesture = GestureType.Tap;
            fingers[fingerIndex].startPosition = fingers[fingerIndex].position;

            // do we touch a pickable gameobject ?
            if (autoSelect)
                fingers[fingerIndex].pickedObject = GetPickeGameObject(fingers[fingerIndex].startPosition);

            // we notify a touch
            CreateGesture(EventName.On_TouchStart, fingers[fingerIndex], 0, SwipeType.None, 0, Vector2.zero);
        }

        // Calculates the time since the beginning of the action.
        timeSinceStartAction = Time.time - startTimeAction;


        // touch canceled?
        if (fingers[fingerIndex].phase == TouchPhase.Canceled) {
            fingers[fingerIndex].gesture = GestureType.Cancel;
        }

        if (fingers[fingerIndex].phase != TouchPhase.Ended && fingers[fingerIndex].phase != TouchPhase.Canceled) {

            // Are we stationary ?
            if (fingers[fingerIndex].phase == TouchPhase.Stationary && timeSinceStartAction >= longTapTime && fingers[fingerIndex].gesture == GestureType.Tap) {
                fingers[fingerIndex].gesture = GestureType.LongTap;
                CreateGesture(EventName.On_LongTapStart, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
            }

            // Let's move us?
            if ((fingers[fingerIndex].gesture == GestureType.Tap || fingers[fingerIndex].gesture == GestureType.LongTap) && (FingerInTolerance(fingers[fingerIndex]) == false)) {


                //  long touch => cancel
                if (fingers[fingerIndex].gesture == GestureType.LongTap) {
                    fingers[fingerIndex].gesture = GestureType.Cancel;
                    CreateGesture(EventName.On_LongTapEnd, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);

                } else {
                    // If an object is selected we drag
                    if (fingers[fingerIndex].pickedObject) {
                        fingers[fingerIndex].gesture = GestureType.Drag;
                        CreateGesture(EventName.On_DragStart, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
                    }
                        // If not swipe
                    else {
                        fingers[fingerIndex].gesture = GestureType.Swipe;
                        CreateGesture(EventName.On_SwipeStart, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
                    }
                }
            }

            // Gesture update
            EventName message = EventName.None;

            switch (fingers[fingerIndex].gesture) {
                case GestureType.LongTap:
                message = EventName.On_LongTap;
                break;
                case GestureType.Drag:
                message = EventName.On_Drag;
                break;
                case GestureType.Swipe:
                message = EventName.On_Swipe;
                break;
            }

            // Send gesture
            SwipeType currentSwipe = SwipeType.None;
            if (message != EventName.None) {
                currentSwipe = GetSwipe(new Vector2(0, 0), fingers[fingerIndex].deltaPosition);
                CreateGesture(message, fingers[fingerIndex], timeSinceStartAction, currentSwipe, 0, fingers[fingerIndex].deltaPosition);
            }

            // Stationnary
            CreateGesture(EventName.On_TouchDown, fingers[fingerIndex], timeSinceStartAction, currentSwipe, 0, fingers[fingerIndex].deltaPosition);
        } else {

            // End of the touch		
            switch (fingers[fingerIndex].gesture) {
                // tap
                case GestureType.Tap:
                if (fingers[fingerIndex].tapCount < 2) {
                    CreateGesture(EventName.On_SimpleTap, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
                } else {
                    CreateGesture(EventName.On_DoubleTap, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
                }
                break;
                // long tap
                case GestureType.LongTap:
                CreateGesture(EventName.On_LongTapEnd, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
                break;
                // drag
                case GestureType.Drag:
                CreateGesture(EventName.On_DragEnd, fingers[fingerIndex], timeSinceStartAction, GetSwipe(fingers[fingerIndex].startPosition, fingers[fingerIndex].position), (fingers[fingerIndex].startPosition - fingers[fingerIndex].position).magnitude, fingers[fingerIndex].position - fingers[fingerIndex].startPosition);
                break;
                // swipe
                case GestureType.Swipe:
                CreateGesture(EventName.On_SwipeEnd, fingers[fingerIndex], timeSinceStartAction, GetSwipe(fingers[fingerIndex].startPosition, fingers[fingerIndex].position), (fingers[fingerIndex].position - fingers[fingerIndex].startPosition).magnitude, fingers[fingerIndex].position - fingers[fingerIndex].startPosition);
                break;
                // cancel
                case GestureType.Cancel:
                CreateGesture(EventName.On_Cancel, fingers[fingerIndex], 0, SwipeType.None, 0, Vector2.zero);
                break;
            }

            CreateGesture(EventName.On_TouchUp, fingers[fingerIndex], timeSinceStartAction, SwipeType.None, 0, Vector2.zero);
            fingers[fingerIndex] = null;
        }

    }

    private bool CreateGesture(EventName message, Finger finger, float actionTime, SwipeType swipe, float swipeLength, Vector2 swipeVector) {

        //Creating the structure with the required information
        Gesture gesture = new Gesture();

        gesture.fingerIndex = finger.fingerIndex;
        gesture.touchCount = finger.touchCount;
        gesture.startPosition = finger.startPosition;
        gesture.position = finger.position;
        gesture.deltaPosition = finger.deltaPosition;

        gesture.actionTime = actionTime;
        gesture.deltaTime = finger.deltaTime;

        gesture.swipe = swipe;
        gesture.swipeLength = swipeLength;
        gesture.swipeVector = swipeVector;

        gesture.deltaPinch = 0;
        gesture.twistAngle = 0;
        gesture.pickObject = finger.pickedObject;
        gesture.otherReceiver = receiverObject;

        if (useBroadcastMessage) {
            SendGesture(message, gesture);
        }
        if (!useBroadcastMessage || joystickAddon) {
            RaiseEvent(message, gesture);
        }

        return true;
    }

    private void SendGesture(EventName message, Gesture gesture) {


        if (useBroadcastMessage) {
            // Sent to user GameObject
            if (receiverObject != null) {
                if (receiverObject != gesture.pickObject) {
                    receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
                }
            }

            // Sent to the  GameObject who is selected
            if (gesture.pickObject) {
                gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
                // sent to gameobject
            else {
                SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void TwoFinger() {

        float timeSinceStartAction = 0;
        bool move = false;
        Vector2 position = Vector2.zero;
        Vector2 deltaPosition = Vector2.zero;
        float fingerDistance = 0;

        // A touch starts
        if (complexCurrentGesture == GestureType.None) {
            twoFinger0 = GetTwoFinger(-1);
            twoFinger1 = GetTwoFinger(twoFinger0);

            startTimeAction = Time.time;
            complexCurrentGesture = GestureType.Tap;

            fingers[twoFinger0].complexStartPosition = fingers[twoFinger0].position;
            fingers[twoFinger1].complexStartPosition = fingers[twoFinger1].position;

            fingers[twoFinger0].oldPosition = fingers[twoFinger0].position;
            fingers[twoFinger1].oldPosition = fingers[twoFinger1].position;


            oldFingerDistance = Mathf.Abs(Vector2.Distance(fingers[twoFinger0].position, fingers[twoFinger1].position));
            startPosition2Finger = new Vector2((fingers[twoFinger0].position.x + fingers[twoFinger1].position.x) / 2, (fingers[twoFinger0].position.y + fingers[twoFinger1].position.y) / 2);
            deltaPosition = Vector2.zero;

            // do we touch a pickable gameobject ?
            if (autoSelect) {
                pickObject2Finger = GetPickeGameObject(fingers[twoFinger0].complexStartPosition);
                if (pickObject2Finger != GetPickeGameObject(fingers[twoFinger1].complexStartPosition)) {
                    pickObject2Finger = null;
                }
            }

            // we notify the touch
            CreateGesture2Finger(EventName.On_TouchStart2Fingers, startPosition2Finger, startPosition2Finger, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, 0, 0);
        }


        // Calculates the time since the beginning of the action.
        timeSinceStartAction = Time.time - startTimeAction;

        // Position & deltaPosition
        position = new Vector2((fingers[twoFinger0].position.x + fingers[twoFinger1].position.x) / 2, (fingers[twoFinger0].position.y + fingers[twoFinger1].position.y) / 2);
        deltaPosition = position - oldStartPosition2Finger;
        fingerDistance = Mathf.Abs(Vector2.Distance(fingers[twoFinger0].position, fingers[twoFinger1].position));

        // Cancel
        if (fingers[twoFinger0].phase == TouchPhase.Canceled || fingers[twoFinger1].phase == TouchPhase.Canceled) {
            complexCurrentGesture = GestureType.Cancel;
        }

        // Let's go
        if (fingers[twoFinger0].phase != TouchPhase.Ended && fingers[twoFinger1].phase != TouchPhase.Ended && complexCurrentGesture != GestureType.Cancel) {

            // Are we stationary ?
            if (complexCurrentGesture == GestureType.Tap && timeSinceStartAction >= longTapTime && FingerInTolerance(fingers[twoFinger0]) && FingerInTolerance(fingers[twoFinger1])) {
                complexCurrentGesture = GestureType.LongTap;
                // we notify the beginning of a longtouch
                CreateGesture2Finger(EventName.On_LongTapStart2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, 0, 0);
            }

            // Let's move us ?
            //if (FingerInTolerance(fingers[twoFinger0])==false ||FingerInTolerance(fingers[twoFinger1])==false){
            move = true;
            //}

            // we move
            if (move) {

                float dot = Vector2.Dot(fingers[twoFinger0].deltaPosition.normalized, fingers[twoFinger1].deltaPosition.normalized);

                // Pinch
                if (enablePinch && fingerDistance != oldFingerDistance) {
                    // Pinch
                    if (Mathf.Abs(fingerDistance - oldFingerDistance) >= minPinchLength) {
                        complexCurrentGesture = GestureType.Pinch;
                    }

                    // update pinch
                    if (complexCurrentGesture == GestureType.Pinch) {
                        //complexCurrentGesture = GestureType.Acquisition;				
                        if (fingerDistance < oldFingerDistance) {

                            // Send end message
                            if (oldGesture != GestureType.Pinch) {
                                CreateStateEnd2Fingers(oldGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, false);
                                startTimeAction = Time.time;
                            }

                            // Send pinch
                            CreateGesture2Finger(EventName.On_PinchIn, startPosition2Finger, position, deltaPosition, timeSinceStartAction, GetSwipe(fingers[twoFinger0].complexStartPosition, fingers[twoFinger0].position), 0, Vector2.zero, 0, Mathf.Abs(fingerDistance - oldFingerDistance));
                            complexCurrentGesture = GestureType.Pinch;

                        } else if (fingerDistance > oldFingerDistance) {
                            // Send end message
                            if (oldGesture != GestureType.Pinch) {
                                CreateStateEnd2Fingers(oldGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, false);
                                startTimeAction = Time.time;
                            }

                            // Send pinch
                            CreateGesture2Finger(EventName.On_PinchOut, startPosition2Finger, position, deltaPosition, timeSinceStartAction, GetSwipe(fingers[twoFinger0].complexStartPosition, fingers[twoFinger0].position), 0, Vector2.zero, 0, Mathf.Abs(fingerDistance - oldFingerDistance));
                            complexCurrentGesture = GestureType.Pinch;
                        }
                    }
                }

                // Twist
                if (enableTwist) {

                    if (Mathf.Abs(TwistAngle()) > minTwistAngle) {

                        // Send end message
                        if (complexCurrentGesture != GestureType.Twist) {
                            CreateStateEnd2Fingers(complexCurrentGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, false);
                            startTimeAction = Time.time;
                        }
                        complexCurrentGesture = GestureType.Twist;
                    }

                    // Update Twist
                    if (complexCurrentGesture == GestureType.Twist) {
                        CreateGesture2Finger(EventName.On_Twist, startPosition2Finger, position, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, TwistAngle(), 0);
                    }

                    fingers[twoFinger0].oldPosition = fingers[twoFinger0].position;
                    fingers[twoFinger1].oldPosition = fingers[twoFinger1].position;
                }

                // Drag
                if (dot > 0) {
                    if (pickObject2Finger && !twoFingerDragStart) {
                        // Send end message
                        if (complexCurrentGesture != GestureType.Tap) {
                            CreateStateEnd2Fingers(complexCurrentGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, false);
                            startTimeAction = Time.time;
                        }
                        //
                        CreateGesture2Finger(EventName.On_DragStart2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, 0, 0);
                        twoFingerDragStart = true;
                    } else if (!pickObject2Finger && !twoFingerSwipeStart) {
                        // Send end message
                        if (complexCurrentGesture != GestureType.Tap) {
                            CreateStateEnd2Fingers(complexCurrentGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, false);
                            startTimeAction = Time.time;
                        }
                        //

                        CreateGesture2Finger(EventName.On_SwipeStart2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, 0, 0);
                        twoFingerSwipeStart = true;
                    }
                } else {
                    if (dot < 0) {
                        twoFingerDragStart = false;
                        twoFingerSwipeStart = false;
                    }
                }

                //
                if (twoFingerDragStart) {
                    CreateGesture2Finger(EventName.On_Drag2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, GetSwipe(oldStartPosition2Finger, position), 0, deltaPosition, 0, 0);
                }

                if (twoFingerSwipeStart) {
                    CreateGesture2Finger(EventName.On_Swipe2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, GetSwipe(oldStartPosition2Finger, position), 0, deltaPosition, 0, 0);
                }

            } else {
                // Long tap update
                if (complexCurrentGesture == GestureType.LongTap) {
                    CreateGesture2Finger(EventName.On_LongTap2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, SwipeType.None, 0, Vector2.zero, 0, 0);
                }
            }

            CreateGesture2Finger(EventName.On_TouchDown2Fingers, startPosition2Finger, position, deltaPosition, timeSinceStartAction, GetSwipe(oldStartPosition2Finger, position), 0, deltaPosition, 0, 0);


            oldFingerDistance = fingerDistance;
            oldStartPosition2Finger = position;
            oldGesture = complexCurrentGesture;
        } else {
            CreateStateEnd2Fingers(complexCurrentGesture, startPosition2Finger, position, deltaPosition, timeSinceStartAction, true);
            complexCurrentGesture = GestureType.None;
            pickObject2Finger = null;
            twoFingerSwipeStart = false;
            twoFingerDragStart = false;
        }
    }

    private int GetTwoFinger(int index) {

        int i = index + 1;
        bool find = false;

        while (i < 100 && !find) {
            if (fingers[i] != null) {
                if (i >= index) {
                    find = true;
                }
            }
            i++;
        }
        i--;

        return i;
    }

    private void CreateStateEnd2Fingers(GestureType gesture, Vector2 startPosition, Vector2 position, Vector2 deltaPosition, float time, bool realEnd) {

        switch (gesture) {
            // Tap
            case GestureType.Tap:

            if (fingers[twoFinger0].tapCount < 2 && fingers[twoFinger1].tapCount < 2) {
                CreateGesture2Finger(EventName.On_SimpleTap2Fingers, startPosition, position, deltaPosition,
                time, SwipeType.None, 0, Vector2.zero, 0, 0);
            } else {
                CreateGesture2Finger(EventName.On_DoubleTap2Fingers, startPosition, position, deltaPosition,
                time, SwipeType.None, 0, Vector2.zero, 0, 0);
            }
            break;

            // Long tap
            case GestureType.LongTap:
            CreateGesture2Finger(EventName.On_LongTapEnd2Fingers, startPosition, position, deltaPosition,
            time, SwipeType.None, 0, Vector2.zero, 0, 0);
            break;

            // Pinch 
            case GestureType.Pinch:
            CreateGesture2Finger(EventName.On_PinchEnd, startPosition, position, deltaPosition,
            time, SwipeType.None, 0, Vector2.zero, 0, 0);
            break;

            // twist
            case GestureType.Twist:
            CreateGesture2Finger(EventName.On_TwistEnd, startPosition, position, deltaPosition,
            time, SwipeType.None, 0, Vector2.zero, 0, 0);
            break;
        }

        if (realEnd) {
            // Drag
            if (twoFingerDragStart) {
                CreateGesture2Finger(EventName.On_DragEnd2Fingers, startPosition, position, deltaPosition,
                time, GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0, 0);
            };

            // Swipe
            if (twoFingerSwipeStart) {
                CreateGesture2Finger(EventName.On_SwipeEnd2Fingers, startPosition, position, deltaPosition,
                time, GetSwipe(startPosition, position), (position - startPosition).magnitude, position - startPosition, 0, 0);
            }

            CreateGesture2Finger(EventName.On_TouchUp2Fingers, startPosition, position, deltaPosition, time, SwipeType.None, 0, Vector2.zero, 0, 0);
        }
    }

    private void CreateGesture2Finger(EventName message, Vector2 startPosition, Vector2 position, Vector2 deltaPosition,
    float actionTime, SwipeType swipe, float swipeLength, Vector2 swipeVector, float twist, float pinch) {

        //Creating the structure with the required information
        Gesture gesture = new Gesture();

        gesture.touchCount = 2;
        gesture.fingerIndex = -1;
        gesture.startPosition = startPosition;
        gesture.position = position;
        gesture.deltaPosition = deltaPosition;

        gesture.actionTime = actionTime;

        if (fingers[twoFinger0] != null)
            gesture.deltaTime = fingers[twoFinger0].deltaTime;
        else if (fingers[twoFinger1] != null)
            gesture.deltaTime = fingers[twoFinger1].deltaTime;
        else
            gesture.deltaTime = 0;

        gesture.swipe = swipe;
        gesture.swipeLength = swipeLength;
        gesture.swipeVector = swipeVector;

        gesture.deltaPinch = pinch;
        gesture.twistAngle = twist;

        if (message != EventName.On_Cancel2Fingers) {
            gesture.pickObject = pickObject2Finger;
        } else {
            gesture.pickObject = oldPickObject2Finger;
        }

        gesture.otherReceiver = receiverObject;

        if (useBroadcastMessage) {
            SendGesture2Finger(message, gesture);
        } else {
            RaiseEvent(message, gesture);
        }
    }

    private void SendGesture2Finger(EventName message, Gesture gesture) {

        // Sent to user GameObject
        if (receiverObject != null) {
            if (receiverObject != gesture.pickObject) {
                receiverObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
            }
        }

        // Sent to the  GameObject who is selected
        if (gesture.pickObject != null) {
            gesture.pickObject.SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
        }
            // sent to gameobject
        else {
            SendMessage(message.ToString(), gesture, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void RaiseEvent(EventName evnt, Gesture gesture) {

        switch (evnt) {
            case EventName.On_Cancel:
            if (On_Cancel != null)
                On_Cancel(gesture);
            break;
            case EventName.On_Cancel2Fingers:
            if (On_Cancel2Fingers != null)
                On_Cancel2Fingers(gesture);
            break;
            case EventName.On_TouchStart:
            if (On_TouchStart != null)
                On_TouchStart(gesture);
            break;
            case EventName.On_TouchDown:
            if (On_TouchDown != null)
                On_TouchDown(gesture);
            break;
            case EventName.On_TouchUp:
            if (On_TouchUp != null)
                On_TouchUp(gesture);
            break;
            case EventName.On_SimpleTap:
            if (On_SimpleTap != null)
                On_SimpleTap(gesture);
            break;
            case EventName.On_DoubleTap:
            if (On_DoubleTap != null)
                On_DoubleTap(gesture);
            break;
            case EventName.On_LongTapStart:
            if (On_LongTapStart != null)
                On_LongTapStart(gesture);
            break;
            case EventName.On_LongTap:
            if (On_LongTap != null)
                On_LongTap(gesture);
            break;
            case EventName.On_LongTapEnd:
            if (On_LongTapEnd != null)
                On_LongTapEnd(gesture);
            break;
            case EventName.On_DragStart:
            if (On_DragStart != null)
                On_DragStart(gesture);
            break;
            case EventName.On_Drag:
            if (On_Drag != null)
                On_Drag(gesture);
            break;
            case EventName.On_DragEnd:
            if (On_DragEnd != null)
                On_DragEnd(gesture);
            break;
            case EventName.On_SwipeStart:
            if (On_SwipeStart != null)
                On_SwipeStart(gesture);
            break;
            case EventName.On_Swipe:
            if (On_Swipe != null)
                On_Swipe(gesture);
            break;
            case EventName.On_SwipeEnd:
            if (On_SwipeEnd != null)
                On_SwipeEnd(gesture);
            break;
            case EventName.On_TouchStart2Fingers:
            if (On_TouchStart2Fingers != null)
                On_TouchStart2Fingers(gesture);
            break;
            case EventName.On_TouchDown2Fingers:
            if (On_TouchDown2Fingers != null)
                On_TouchDown2Fingers(gesture);
            break;
            case EventName.On_TouchUp2Fingers:
            if (On_TouchUp2Fingers != null)
                On_TouchUp2Fingers(gesture);
            break;
            case EventName.On_SimpleTap2Fingers:
            if (On_SimpleTap2Fingers != null)
                On_SimpleTap2Fingers(gesture);
            break;
            case EventName.On_DoubleTap2Fingers:
            if (On_DoubleTap2Fingers != null)
                On_DoubleTap2Fingers(gesture);
            break;
            case EventName.On_LongTapStart2Fingers:
            if (On_LongTapStart2Fingers != null)
                On_LongTapStart2Fingers(gesture);
            break;
            case EventName.On_LongTap2Fingers:
            if (On_LongTap2Fingers != null)
                On_LongTap2Fingers(gesture);
            break;
            case EventName.On_LongTapEnd2Fingers:
            if (On_LongTapEnd2Fingers != null)
                On_LongTapEnd2Fingers(gesture);
            break;
            case EventName.On_Twist:
            if (On_Twist != null)
                On_Twist(gesture);
            break;
            case EventName.On_TwistEnd:
            if (On_TwistEnd != null)
                On_TwistEnd(gesture);
            break;
            case EventName.On_PinchIn:
            if (On_PinchIn != null)
                On_PinchIn(gesture);
            break;
            case EventName.On_PinchOut:
            if (On_PinchOut != null)
                On_PinchOut(gesture);
            break;
            case EventName.On_PinchEnd:
            if (On_PinchEnd != null)
                On_PinchEnd(gesture);
            break;
            case EventName.On_DragStart2Fingers:
            if (On_DragStart2Fingers != null)
                On_DragStart2Fingers(gesture);
            break;
            case EventName.On_Drag2Fingers:
            if (On_Drag2Fingers != null)
                On_Drag2Fingers(gesture);
            break;
            case EventName.On_DragEnd2Fingers:
            if (On_DragEnd2Fingers != null)
                On_DragEnd2Fingers(gesture);
            break;
            case EventName.On_SwipeStart2Fingers:
            if (On_SwipeStart2Fingers != null)
                On_SwipeStart2Fingers(gesture);
            break;
            case EventName.On_Swipe2Fingers:
            if (On_Swipe2Fingers != null)
                On_Swipe2Fingers(gesture);
            break;
            case EventName.On_SwipeEnd2Fingers:
            if (On_SwipeEnd2Fingers != null)
                On_SwipeEnd2Fingers(gesture);
            break;
        }
    }


    private GameObject GetPickeGameObject(Vector2 screenPos) {

        Ray ray = mainCam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, pickableLayers)) {
            return hit.collider.gameObject;
        }

        return null;

    }

    private SwipeType GetSwipe(Vector2 start, Vector2 end) {

        Vector2 linear;
        linear = (end - start).normalized;

        if (Mathf.Abs(linear.y) > Mathf.Abs(linear.x)) {
            if (Vector2.Dot(linear, Vector2.up) >= swipeTolerance)
                return SwipeType.Up;

            if (Vector2.Dot(linear, -Vector2.up) >= swipeTolerance)
                return SwipeType.Down;
        } else {
            if (Vector2.Dot(linear, Vector2.right) >= swipeTolerance)
                return SwipeType.Right;

            if (Vector2.Dot(linear, -Vector2.right) >= swipeTolerance)
                return SwipeType.Left;
        }

        return SwipeType.Other;
    }

    private bool FingerInTolerance(Finger finger) {

        if ((finger.position - finger.startPosition).sqrMagnitude <= (StationnaryTolerance * StationnaryTolerance)) {
            return true;
        } else {
            return false;
        }
    }

    private float DeltaAngle(Vector2 start, Vector2 end) {

        var tmp = (start.x * end.y) - (start.y * end.x);
        return Mathf.Atan2(tmp, Vector2.Dot(start, end));

    }

    private float TwistAngle() {

        Vector2 dir = (fingers[twoFinger0].position - fingers[twoFinger1].position);
        Vector2 refDir = (fingers[twoFinger0].oldPosition - fingers[twoFinger1].oldPosition);
        float angle = Mathf.Rad2Deg * DeltaAngle(refDir, dir);

        return angle;
    }

    /// <summary>
    /// Enables or disables Easy Touch.
    /// </summary>
    /// <param name='enable'>
    /// true = enabled
    /// false = disabld
    /// </param>
    public static void SetEnabled(bool enable) {
        EasyTouch.instance.enable = enable;
    }

    /// <summary>
    /// Return if EasyTouch is enabled or disabled
    /// </summary>
    /// <returns>
    /// True = Enabled
    /// False = Disabled
    /// </returns>
    public static bool GetEnabled() {
        return EasyTouch.instance.enable;
    }

    /// <summary>
    /// Return the current touches count.
    /// </summary>
    /// <returns>
    /// int
    /// </returns>
    public static int GetTouchCount() {
        return EasyTouch.instance.input.TouchCount();
    }

    /// <summary>
    /// Return the camera used by EasyTouch for the auto-selection. EasyTouch research at start the camera with the tag MainCamera.
    /// </summary>
    /// <returns>
    /// Camera
    /// </returns>
    public static Camera GetCamera() {
        return EasyTouch.instance.mainCam;
    }

    /// <summary>
    /// Enables or disables the recognize of 2 fingers gesture.
    /// </summary>
    /// <param name='enable'>
    /// true = enabled
    /// false = disabled
    /// </param>
    public static void SetEnable2FingersGesture(bool enable) {
        EasyTouch.instance.enable2FingersGesture = enable;
    }

    /// <summary>
    /// Return if 2 fingers gesture is enabled or disabled
    /// </summary>
    /// <returns>
    /// true = enabled
    /// false = disabled
    /// </returns>
    public static bool GetEnable2FingersGesture() {
        return EasyTouch.instance.enable2FingersGesture;
    }

    /// <summary>
    /// Enables or disables the recognize of twist gesture
    /// </summary>
    /// <param name='enable'>
    /// true = enabled
    /// false = disables
    /// </param>
    public static void SetEnableTwist(bool enable) {
        EasyTouch.instance.enableTwist = enable;
    }

    /// <summary>
    /// Return if 2 twist gesture is enabled or disabled
    /// </summary>
    /// <returns>
    /// true = enabled
    /// false = disables
    /// </returns>
    public static bool GetEnableTwist() {
        return EasyTouch.instance.enableTwist;
    }

    /// <summary>
    /// Enables or disables the recognize of pinch gesture
    /// </summary>
    /// <param name='enable'>
    /// true = enabled
    /// false = disables
    /// </param>
    public static void SetEnablePinch(bool enable) {
        EasyTouch.instance.enablePinch = enable;
    }

    /// <summary>
    /// Return if 2 pinch gesture is enabled or disabled
    /// </summary>
    /// <returns>
    /// true = enabled
    /// false = disables
    /// </returns>
    public static bool GetEnablePinch() {
        return EasyTouch.instance.enablePinch;
    }

    /// <summary>
    /// Enables or disables auto select.
    /// </summary>
    /// <param name='enable'>
    /// true = enabled
    /// false = disables
    /// </param>
    public static void SetEnableAutoSelect(bool enable) {
        EasyTouch.instance.autoSelect = enable;
    }

    /// <summary>
    /// Return if auto select is enabled or disabled
    /// </summary>
    /// <returns>
    /// true = enabled
    /// false = disables
    /// </returns>
    public static bool GetEnableAutoSelect() {
        return EasyTouch.instance.autoSelect;
    }


    /// <summary>
    /// Sets the other receiver for EasyTouch event.
    /// </summary>
    /// <param name='receiver'>
    /// GameObject.
    /// </param>
    public static void SetOtherReceiverObject(GameObject receiver) {
        EasyTouch.instance.receiverObject = receiver;
    }

    /// <summary>
    /// Return the other event receiver.
    /// </summary>
    /// <returns>
    /// GameObject
    /// </returns>
    public static GameObject GetOtherReceiverObject() {
        return EasyTouch.instance.receiverObject;
    }

    /// <summary>
    /// Sets the stationnary tolerance.
    /// </summary>
    /// <param name='tolerance'>
    /// float Tolerance.
    /// </param>
    public static void SetStationnaryTolerance(float tolerance) {
        EasyTouch.instance.StationnaryTolerance = tolerance;
    }

    /// <summary>
    /// Return the stationnary tolerance.
    /// </summary>
    /// <returns>
    /// Float
    /// </returns>
    public static float GetStationnaryTolerance() {
        return EasyTouch.instance.StationnaryTolerance;
    }

    /// <summary>
    /// Set the long tap time in second
    /// </summary>
    /// <param name='time'>
    /// Float
    /// </param>
    public static void SetlongTapTime(float time) {
        EasyTouch.instance.longTapTime = time;
    }

    /// <summary>
    ///  Return the longs the tap time.
    /// </summary>
    /// <returns>
    /// Float.
    /// </returns>
    public static float GetlongTapTime() {
        return EasyTouch.instance.longTapTime;
    }

    /// <summary>
    /// Sets the swipe tolerance.
    /// </summary>
    /// <param name='tolerance'>
    /// Float
    /// </param>
    public static void SetSwipeTolerance(float tolerance) {
        EasyTouch.instance.swipeTolerance = tolerance;
    }

    /// <summary>
    /// Return the swipe tolerance.
    /// </summary>
    /// <returns>
    /// Float.
    /// </returns>
    public static float GetSwipeTolerance() {
        return EasyTouch.instance.swipeTolerance;
    }

    /// <summary>
    /// Sets the minimum length of the pinch.
    /// </summary>
    /// <param name='length'>
    /// Float.
    /// </param>
    public static void SetMinPinchLength(float length) {
        EasyTouch.instance.minPinchLength = length;
    }

    /// <summary>
    /// Return the minimum length of the pinch.
    /// </summary>
    /// <returns>
    /// Float
    /// </returns>
    public static float GetMinPinchLength() {
        return EasyTouch.instance.minPinchLength;
    }

    /// <summary>
    /// Sets the minimum twist angle.
    /// </summary>
    /// <param name='angle'>
    /// Float
    /// </param>
    public static void SetMinTwistAngle(float angle) {
        EasyTouch.instance.minTwistAngle = angle;
    }

    /// <summary>
    /// Gets the minimum twist angle.
    /// </summary>
    /// <returns>
    /// Float
    /// </returns>
    public static float GetMinTwistAngle() {
        return EasyTouch.instance.minTwistAngle;
    }

    /// <summary>
    /// Gets the current picked object.
    /// </summary>
    /// <returns>
    /// The current picked object.
    /// </returns>
    /// <param name='fingerIndex'>
    /// Finger index.
    /// </param>
    public static GameObject GetCurrentPickedObject(int fingerIndex) {
        return EasyTouch.instance.GetPickeGameObject(EasyTouch.instance.fingers[fingerIndex].position);
    }

    /// <summary>
    /// Determines whether a touch is under a specified rect.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this a touch is rect under the specified rect; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='rect'>
    /// If set to <c>true</c> rect.
    /// </param>
    public static bool IsRectUnderTouch(Rect rect) {

        bool find = false;

        for (int i = 0; i < 100; i++) {
            if (EasyTouch.instance.fingers[i] != null) {
                find = rect.Contains(EasyTouch.instance.fingers[i].position);
                break;
            }
        }

        return find;
    }
}
