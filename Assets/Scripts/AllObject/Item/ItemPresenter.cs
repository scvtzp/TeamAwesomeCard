using DefaultNamespace;
using Manager;
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
    }
}