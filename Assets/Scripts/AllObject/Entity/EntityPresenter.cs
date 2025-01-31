using System.Collections.Generic;
using System.Threading.Tasks;
using AllObject.Entity;
using Card.Status;
using Manager;
using R3;
using SkillSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntityPresenter : ICardPresenter
    {
        private EntityModel _model;
        private EntityView _view;

        private bool _isFront;
        
        public EntityPresenter(EntityModel model, EntityView view)
        {
            _model = model;
            _view = view;
            _view.Init(this, model.id);
            
            _view.OnClicked += ()=>
            {
                if(_isFront)
                    BattleManager.Instance.Attack(_model);
            };
            
            _model.hp.Subscribe(_ => ChangeHp(_model.hp, _model.maxHp));
            _model.maxHp.Subscribe(_ => ChangeHp(_model.hp, _model.maxHp));
            _model.atk.Subscribe(value => view.SetStatus(StatusType.Ad,value));
            _model.def.Subscribe(value => view.SetStatus(StatusType.Def,value));
        }

        private async void ChangeHp(ReactiveProperty<int> hp, ReactiveProperty<int> maxHp)
        {
            if(maxHp.Value < hp.Value)
                hp.Value = maxHp.Value;
            
            await _view.SetHpBar(hp.Value, maxHp.Value);
            
            if (hp.Value <= 0) //죽음 판정 여기서 하는게 과연 맞는가?
                StageManager.Instance.DeathAction(this);
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

        public bool UsedSkill(List<Skill> skillList)
        {
            BattleManager.Instance.UsedSkill(skillList, _model);
            return true;
        }
    }
}