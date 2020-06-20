using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler,
    IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform imgRect;        //得到图片的ugui坐标
    Vector3 offset;    //用来得到鼠标和图片的差值
    public Vector3 imgReduceScale = new Vector3(1.2f, 1.2f, 1);   //设置图片缩放
    public Vector3 imgNormalScale = new Vector3(1, 1, 1);   //正常大小

    // Use this for initialization
    void Awake()
    {
        imgRect = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        print(imgRect);
    }

    //当鼠标按下时调用 接口对应  IPointerDownHandler
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, Input.mousePosition, eventData.pressEventCamera, out globalMousePos);
        offset = transform.position - globalMousePos;
    }

    //当鼠标拖动时调用   对应接口 IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(imgRect, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            transform.position = globalMousePos + offset;
        }
    }

    //当鼠标抬起时调用  对应接口  IPointerUpHandler
    public void OnPointerUp(PointerEventData eventData)
    {
        offset = Vector2.zero;
    }

    //当鼠标结束拖动时调用   对应接口  IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        offset = Vector2.zero;
    }

    //当鼠标进入图片时调用   对应接口   IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        imgRect.localScale = imgReduceScale;   //缩小图片
    }

    //当鼠标退出图片时调用   对应接口   IPointerExitHandler
    public void OnPointerExit(PointerEventData eventData)
    {
        imgRect.localScale = imgNormalScale;   //回复图片
    }
}

