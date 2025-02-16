using DefaultNamespace;
using Manager;
using SkillSystem;
using UnityEngine;

namespace AllObject.Item
{
    public class ItemPresenter : ICardPresenter
    {
        private ItemModel _model;
        private ItemView _view;

        private bool _isFront;
        
        public ItemPresenter(ItemModel model, ItemView view)
        {
            _model = model;
            _view = view;
            _view.Init(this, _model.id);
        }
        
        public void Death()
        {
            Object.Destroy(_view.gameObject);
        }

        public void SetCardFace(bool isFront = true)
        {
            _view.SetCardFace(isFront);
            _isFront = isFront;
        }

        public bool UsedCard(CardAble targetCard)
        {
            return targetCard.UsedSkill(_model.SkillList);
        }

        public void UsedCard()
        {
            BattleManager.Instance.UsedSkill(_model.SkillList);
        }
        
        /// 이 아이템이 대상 지정을 필요로 하는가? (0번이 주 스킬이라 이게 타겟형이 아니면 타겟형 아님)
        public bool NeedSelectTarget() => _model.SkillList[0].NeedSelectTarget();
    }
}