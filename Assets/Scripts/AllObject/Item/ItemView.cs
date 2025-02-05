using DefaultNamespace;
using Manager;
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
        
        [SerializeField] private GameObject nameObject;
        [SerializeField] private TextMeshProUGUI itemDesc;

        private ItemPresenter _presenter;
        
        public void Init(ItemPresenter presenter)
        {
            CardType = CardType.Item;
            _presenter = presenter;
        }
        
        public override void SetCardFace(bool isFront = true)
        {
            base.SetCardFace(isFront);
            nameObject.SetActive(isFront);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (TargetSlot != null && TargetCard != null)
            {
                Debug.Log("중복타겟 버그");
                return;
            }
            
            if (CardType == CardType.Item && TargetSlot != null)
            {
                KeepCard(TargetSlot);
            }
            else if (CardType == CardType.Item && _presenter.NeedSelectTarget() && TargetCard != null)
            {
                if (_presenter.UsedCard(TargetCard))
                    StageManager.Instance.DeathAction(_presenter);
            }
            else if (CardType == CardType.Item && !_presenter.NeedSelectTarget() && TargetCard == null)
            {
                _presenter.UsedCard();
                StageManager.Instance.DeathAction(_presenter);
            }
            
            base.OnPointerUp(eventData);
        }
        
        public override void UpdateData(string id)
        {
            //itemImage.sprite = SpriteManager.Instance.GetSprite(id);
            itemName.SetText(id);
        }
    }
}