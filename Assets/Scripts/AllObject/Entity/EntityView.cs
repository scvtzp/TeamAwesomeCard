using System;
using Card.Status;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace AllObject.Entity
{
    public class EntityView : CardAble
    {
        [SerializeField] private Image entityImage;
        [SerializeField] private StatusView status;
        [SerializeField] private HpBar hpBar;
        
        public void Init()
        {
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
    }
}