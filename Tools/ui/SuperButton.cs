using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  [AddComponentMenu("UI/SuperButton", 30)]
  public class SuperButton : Selectable, IPointerClickHandler, ISubmitHandler
  {
    [FormerlySerializedAs("onClick")] [SerializeField]
    private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
    [SerializeField]
    public UnityEvent onPointerEnter = new UnityEvent();
    [SerializeField]
    public UnityEvent onPointerExit = new UnityEvent();
    [SerializeField]
    public UnityEvent onPointerUp = new UnityEvent();
    [SerializeField]
    public UnityEvent onPointerDown = new UnityEvent();

    private bool isPointerEnter;
    [NonSerialized]
    public Camera myCamera;

    protected override void Start()
    {
      base.Start();
      var myCanvas = GetComponentInParent<Canvas>();
      myCamera = myCanvas?.rootCanvas?.worldCamera;
    }

    protected SuperButton()
    {
    }

    public Button.ButtonClickedEvent onClick
    {
      get { return this.m_OnClick; }
      set { this.m_OnClick = value; }
    }
    
    private void Press()
    {
      if (!this.IsActive() || !this.IsInteractable())
        return;
      UISystemProfilerApi.AddMarker("Button.onClick", (UnityEngine.Object) this);
      this.m_OnClick.Invoke();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.Press();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.Press();
      if (!this.IsActive() || !this.IsInteractable())
        return;
      this.DoStateTransition(Selectable.SelectionState.Pressed, false);
      this.StartCoroutine(this.OnFinishSubmit());
    }

    [DebuggerHidden]
    private IEnumerator OnFinishSubmit()
    {
      throw new NotImplementedException();
    }

    [Serializable]
    public class ButtonClickedEvent : UnityEvent
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      onPointerEnter.Invoke();
      isPointerEnter = true;
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      onPointerDown.Invoke();
    }
    
    public override void OnPointerUp(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      onPointerUp.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      if (Application.isMobilePlatform)
      {
        if (IsPointerInSelf())
        {
          OnPointerUp(new PointerEventData(EventSystem.current));
        }
        else
        {
          base.OnPointerExit(eventData);
          onPointerExit.Invoke();
          isPointerEnter = false;
        }
      }
      else
      {
        base.OnPointerExit(eventData);
        onPointerExit.Invoke();
        isPointerEnter = false;
      }
    }

    private void Update()
    {
      if (isPointerEnter && IsPointUp())
      {
        OnPointerUp(new PointerEventData(EventSystem.current));
      }
    }

    bool IsPointerInSelf()
    {
      if (Application.isMobilePlatform)
      {
        for (int i = 0; i < Input.touches.Length; i++)
        {
          var touch = Input.touches[i];
          Debug.Log($"touch.fingerId : {touch.fingerId} ----------------------------------touch.phase : {touch.phase}," +
                    $"IsPointerOverGameObject :  {EventSystem.current.IsPointerOverGameObject(touch.fingerId)} " +
                    $" RectangleContainsScreenPoint: {RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.position, myCamera)}");
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
              bool isContain = RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, touch.position, myCamera);
              if (isContain)
              {
                return true;
              }
            }
        }
      }

      return false;
    }

    private bool IsPointUp()
    {
      if (Application.isMobilePlatform)
      {
        // 此处判断不了，不如直接返回false
        return false;
      }
      else
      {
        if (Input.GetMouseButtonUp(0))
        {
          return true;
        }
      }
      return false;
    }
  }
}
