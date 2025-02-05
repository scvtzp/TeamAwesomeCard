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
            _view.Init(this);
            
            //테스트용.
            if (model.SkillList[0] is Heal skill)
            {
                _view.UpdateData("회복 포션");
            }
                
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

        public bool NeedSelectTarget() => _model.SkillList[0].NeedSelectTarget;
    }
}