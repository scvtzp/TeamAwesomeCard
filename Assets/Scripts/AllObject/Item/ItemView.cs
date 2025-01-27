using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AllObject.Item
{
    public class ItemView : CardAble
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemDesc;
        
        private RectTransform rectTransform;
        
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public override void SetCardFace(bool isFront = true)
        {
            
        }
    }
}