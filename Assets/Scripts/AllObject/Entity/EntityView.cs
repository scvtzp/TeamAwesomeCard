using System;
using System.Collections.Generic;
using Card.Status;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Manager;
using SkillSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AllObject.Entity
{
    public class EntityView : CardAble
    {
        [SerializeField] private TextMeshProUGUI entityName;
        [SerializeField] private Image entityImage;
        
        [SerializeField] private StatusView status;
        [SerializeField] private HpBar hpBar;
        
        private EntityPresenter _presenter;
        
        public void Init(EntityPresenter presenter, string id)
        {
            _presenter = presenter;
            UpdateData(id);
            status?.Init();
        }
        
        public override void SetCardFace(bool isFront = true)
        {
            base.SetCardFace(isFront);
            
            status?.gameObject.SetActive(isFront); //그냥 캔버스를 끄자.
            hpBar?.gameObject.SetActive(isFront);
        }
        
        public void SetStatus(StatusType type, int value) => status?.SetStatus(type, value);
        
        public async UniTask SetHpBar(int hp, int maxHp)
        {
            if(hpBar != null) 
                await hpBar.SetHpBar(hp, maxHp);
        }

        public override bool UsedSkill(List<Skill> skillList)
        {
            return _presenter.UsedSkill(skillList);
        }

        public override void UpdateData(string id)
        {
            if(entityImage != null)
                entityImage.sprite = SpriteManager.Instance.GetSprite($"Card_{id}");
            entityName?.SetText(id);
        }
    }
}