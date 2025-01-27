using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AllObject.Item
{
    public class ItemView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemDesc;
        
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 offset;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // 마우스 클릭 지점과 오브젝트 중심의 오프셋 계산
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out var localMousePosition);

            offset = rectTransform.anchoredPosition - localMousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform, 
                    eventData.position, 
                    eventData.pressEventCamera, 
                    out var localMousePosition))
            {
                // 오프셋을 적용하여 정확한 드래그 위치 계산
                //rectTransform.anchoredPosition = localMousePosition + offset;
            }
            
            transform.position = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // 드래그 완료 시 필요한 동작을 여기에 추가
        }
    }
}