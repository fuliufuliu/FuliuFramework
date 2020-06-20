using UnityEngine;
using UnityEngine.UI;


public class SimpleTouchController : MonoBehaviour {

	// PUBLIC
	public delegate void TouchDelegate(Vector2 value, Vector2 deltaValue);
	public event TouchDelegate DragValueEvent;

	public delegate void TouchStateDelegate(bool touchPresent);
	public event TouchStateDelegate DragStateEvent;
	public event TouchStateDelegate PointerDownUpEvent;
	
	public delegate void PointerClickDelegate();
	public event PointerClickDelegate PointerClickEvent;


	// PRIVATE
	[SerializeField]
	private RectTransform joystickArea;
	private bool touchPresent = false;
	private Vector2 movementVector;
	private SuperScollRect scrollRect;

	private void Awake()
	{
		scrollRect = GetComponent<SuperScollRect>();
	}
	
	public Vector2 GetTouchPosition => movementVector;


	public void BeginPointerDown()
	{
		PointerDownUpEvent?.Invoke(true);
	}
	
	public void BeginPointerUp()
	{
		PointerDownUpEvent?.Invoke(false);
	}
	
	public void PointerClick()
	{
		PointerClickEvent?.Invoke();
	}

	public void BeginDrag()
	{
		touchPresent = true;
		DragStateEvent?.Invoke(touchPresent);
	}

	public void EndDrag()
	{
		touchPresent = false;
		movementVector = joystickArea.anchoredPosition = Vector2.zero;

		DragStateEvent?.Invoke(touchPresent);

	}

	public void OnValueChanged(Vector2 value, Vector2 deltaValue)
	{
		if(touchPresent)
		{
			// convert the value between 1 0 to -1 +1
			movementVector.x = ((1 - value.x) - 0.5f) * 2f;
			movementVector.y = ((1 - value.y) - 0.5f) * 2f;

			DragValueEvent?.Invoke(movementVector, deltaValue);
		}

	}

	public void ResetPos()
	{
		scrollRect.normalizedPosition = Vector2.zero;
		scrollRect.UpdateBounds();
	}
}
