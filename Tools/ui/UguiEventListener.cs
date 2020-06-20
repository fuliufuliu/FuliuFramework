using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UguiEventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, 
    IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler,
    ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
    public string _Event = "";
    public Action<GameObject> onClick
    {
        get { return _onClick; }
        set
        {
            if (value == null)
            {
                _Event = _Event.Replace("_onClick ", "");
            }
            else
            {
                if (!_Event.Contains("_onClick "))
                {
                    _Event += "_onClick ";
                }
            }
            _onClick = value;
        }
    }

    public Action<PointerEventData> onClickEvent;
    public Action<GameObject> onDown;
    public Action<PointerEventData> onDownEvent;
    public Action<GameObject> onEnter;
    public Action<PointerEventData> onEnterEvent;
    public Action<GameObject> onExit;
    public Action<PointerEventData> onExitEvent;
    public Action<GameObject> onUp;
    public Action<PointerEventData> onUpEvent;
    public Action<GameObject> onSelect;
    public Action<BaseEventData> onSelectEvent;
    public Action<GameObject> onUpdateSelect;
    public Action<BaseEventData> onUpdateSelectEvent;
    public Action<GameObject> onBeginDrag;
    public Action<PointerEventData> onBeginDragEvent;
    public Action<GameObject> onCancel;
    public Action<BaseEventData> onCancelEvent;
    public Action<GameObject> onDeselect;
    public Action<BaseEventData> onDeselectEvent;
    public Action<GameObject> onDrag;
    public Action<PointerEventData> onDragEvent;
    public Action<GameObject, GameObject> onDrop;
    public Action<PointerEventData> onDropEvent;
    public Action<GameObject> onEndDrag;
    public Action<PointerEventData> onEndDragEvent;
    public Action<GameObject> onInitializePotentialDrag;
    public Action<PointerEventData> onInitializePotentialDragEvent;
    public Action<GameObject> onMove;
    public Action<AxisEventData> onMoveEvent;
    public Action<GameObject> onScroll;
    public Action<PointerEventData> onScrollEvent;
    public Action<GameObject> onSubmit;
    public Action<BaseEventData> onSubmitEvent;

    private Action<GameObject> _onClick;

    private IInitializePotentialDragHandler iInitializePotentialDragHandler;
    private IBeginDragHandler iBeginDragHandler;
    private IEndDragHandler iEndDragHandler;
    private IDragHandler iDragHandler;
    private IScrollHandler iScrollHandler;

    public static UguiEventListener Get(GameObject go)
    {
        UguiEventListener listener = go.GetComponent<UguiEventListener>();

        if (listener == null)
        {
            listener = go.AddComponent<UguiEventListener>();
            var parent = listener.transform.parent;
            if (parent)
            {
                listener.iInitializePotentialDragHandler = parent.GetComponentInParent<IInitializePotentialDragHandler>();
                listener.iBeginDragHandler = parent.GetComponentInParent<IBeginDragHandler>();
                listener.iEndDragHandler = parent.GetComponentInParent<IEndDragHandler>();
                listener.iDragHandler = parent.GetComponentInParent<IDragHandler>();
                listener.iScrollHandler = parent.GetComponentInParent<IScrollHandler>();
            }
        }
        return listener;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
        if (onClickEvent != null) onClickEvent(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
        if (onDownEvent != null) onDownEvent(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
        if (onEnterEvent != null) onEnterEvent(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
        if (onExitEvent != null) onExitEvent(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
        if (onUpEvent != null) onUpEvent(eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
        if (onSelectEvent != null) onSelectEvent(eventData);
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
        if (onUpdateSelectEvent != null) onUpdateSelectEvent(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
        if (onBeginDragEvent != null) onBeginDragEvent(eventData);
        if (iBeginDragHandler != null)
        {
            iBeginDragHandler.OnBeginDrag(eventData);
        }
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(gameObject);
        if (onCancelEvent != null) onCancelEvent(eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null) onDeselect(gameObject);
        if (onDeselectEvent != null) onDeselectEvent(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
        if (onDragEvent != null) onDragEvent(eventData);
        if (iDragHandler != null)
        {
            iDragHandler.OnDrag(eventData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(gameObject, eventData.lastPress);
        if (onDropEvent != null) onDropEvent(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
        if (onEndDragEvent != null) onEndDragEvent(eventData);
        if (iEndDragHandler != null)
        {
            iEndDragHandler.OnEndDrag(eventData);
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
        if (onInitializePotentialDragEvent != null) onInitializePotentialDragEvent(eventData);
        if (iInitializePotentialDragHandler != null)
        {
            iInitializePotentialDragHandler.OnInitializePotentialDrag(eventData);
        }
    }

    public void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(gameObject);
        if (onMoveEvent != null) onMoveEvent(eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(gameObject);
        if (onScrollEvent != null) onScrollEvent(eventData);
        if (iScrollHandler != null)
        {
            iScrollHandler.OnScroll(eventData);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(gameObject);
        if (onSubmitEvent != null) onSubmitEvent(eventData);
    }


}
