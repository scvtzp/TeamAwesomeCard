using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace AllObject.Item
{
    public class ItemView : CardAble
    {
        [SerializeField] private LocalizeStringEvent itemName;
        [SerializeField] private LocalizeStringEvent itemDesc;
        [SerializeField] private Image itemImage;
        
        [SerializeField] private GameObject nameObject;

        private ItemPresenter _presenter;
        private string id;
        
        public void Init(ItemPresenter presenter, string itemId)
        {
            CardType = CardType.Item;
            _presenter = presenter;
            id = itemId;
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

        //아래 있는거랑 통합필요.
        public override void UpdateData(string id)
        {
            throw new System.NotImplementedException();
        }

        public async UniTaskVoid UpdateData(Dictionary<string, string> smartStringData)
        {
            itemImage.sprite = SpriteManager.Instance.GetSprite(id);
            itemName.StringReference.SetReference(LocalizeTable.InGameObject, id);

            // SmartString의 key들을 가져옴
            var keys = await GameUtil.CheckLocalizedStringKeys(LocalizeTable.InGameObject, $"{id}_Desc");
            // 가져온 key값들에 인자로 받은 Dictionary에서 값을 적용
            foreach (var key in keys)
                itemDesc.StringReference.Add(key, new StringVariable(){Value = smartStringData[key]});
            itemDesc.StringReference.SetReference(LocalizeTable.InGameObject, $"{id}_Desc");
        }
    }
}