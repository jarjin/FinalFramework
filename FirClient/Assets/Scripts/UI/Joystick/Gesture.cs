using UnityEngine;
using System.Collections;

/// <summary>
/// This is the class passed as parameter by EasyTouch events, that containing all informations about the touch that raise the event,
/// or by the tow fingers gesture that raise the event.
/// </summary>
public class Gesture{
	
	/// <summary>
	/// The index of the finger that raise the event (Starts at 0), or -1 for a two fingers gesture.
	/// </summary>
	public int fingerIndex;				
	/// <summary>
	/// The touches count.
	/// </summary>
	public int touchCount;				
	/// <summary>
	/// The start position of the current gesture, or the center position between the two touches for a two fingers gesture.
	/// </summary>
	public Vector2 startPosition;		
	/// <summary>
	/// The current position of the touch that raise the event,  or the center position between the two touches for a two fingers gesture.
	/// </summary>
	public Vector2 position;
	/// <summary>
	/// The position delta since last change.
	/// </summary>
	public Vector2 deltaPosition;		
	/// <summary>
	/// Time since the beginning of the gesture.
	/// </summary>
	public float actionTime;			
	/// <summary>
	/// Amount of time passed since last change.
	/// </summary>
	public float deltaTime;				
	/// <summary>
	/// The siwpe or drag  type ( None, Left, Right, Up, Down, Other => look at EayTouch.SwipeType enumeration).
	/// </summary>
	public EasyTouch.SwipeType swipe;	
	/// <summary>
	/// The length of the swipe.
	/// </summary>
	public float swipeLength;				
	/// <summary>
	/// The swipe vector direction.
	/// </summary>
	public Vector2 swipeVector;			
	/// <summary>
	/// The pinch length delta since last change.
	/// </summary>
	public float deltaPinch;	
	/// <summary>
	/// The angle of the twist.
	/// </summary>
	public float twistAngle;			
	/// <summary>
	/// The current picked gameObject under the touch that raise the event.
	/// </summary>
	public GameObject pickObject;		
	/// <summary>
	/// Other receiver of the event.
	/// </summary>
	public GameObject otherReceiver; 	
	
	/// <summary>
	/// Transforms touch position into world space, or the center position between the two touches for a two fingers gesture.
	/// </summary>
	/// <returns>
	/// Vector3 : world position
	/// </returns>
	/// <param name='z'>
	/// The z position in world units from the camera
	/// </param>
	public Vector3 GetTouchToWordlPoint(float z){
		return  EasyTouch.GetCamera().ScreenToWorldPoint( new Vector3( position.x, position.y,z));	
	}
	
	/// <summary>
	/// Gets the swipe or drag angle. (calculate from swipe Vector)
	/// </summary>
	/// <returns>
	/// Float : The swipe or drag angle.
	/// </returns>
	public float GetSwipeOrDragAngle(){
		return Mathf.Atan2( swipeVector.normalized.y,swipeVector.normalized.x) * Mathf.Rad2Deg;	
	}
	
	/// <summary>
	/// Determines whether the touch is in a specified rect.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is in rect the specified rect; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='rect'>
	/// If set to <c>true</c> rect.
	/// </param>
	public bool IsInRect( Rect rect){
		return rect.Contains( position);	
	}
}

