using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDragScript : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 lastMousePosition;
        
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }   
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();
        //dragRactTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        if (!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }   
    public void OnEndDrag(PointerEventData eventData)
    {
               
    }    
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, maxWidth, maxHeight);
        foreach (Vector3 corner in corners)
        {            
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }
    [SerializeField] RectTransform dragRactTransform;
    [SerializeField] Canvas canvas;
   
    float maxWidth; // horizontal maximum drag range   
    float maxHeight; // vertical maximum drag range
    [SerializeField] int borderThickness;
    void Awake()
    {
        if (dragRactTransform == null)
        {
            dragRactTransform = transform.parent.GetComponent<RectTransform>();
        }
        if (canvas == null)
        {
            Transform CanvasTranform = transform.parent;
            while (CanvasTranform != null)
            {
                canvas = CanvasTranform.GetComponent<Canvas>();
                if(canvas != null)
                {
                    break;
                }
                CanvasTranform = CanvasTranform.parent;
            }
        }
               
    }
    void Start()
    {
        maxHeight = Screen.height - borderThickness;
        maxWidth = Screen.width - borderThickness;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragRactTransform.SetAsLastSibling();        
    }
    private void OnEnable()
    {
        dragRactTransform.SetAsLastSibling();
    }

}
