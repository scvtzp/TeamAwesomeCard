using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class HpBar : MonoBehaviour
    {
        [SerializeField] RectTransform fillTransform;
        [SerializeField] TextMeshProUGUI text;

        public async UniTask SetHpBar(int curHp, int maxHp, bool isAnimated = true)
        {
            float fillAmount = (float)curHp / maxHp;
            if (fillAmount < 0) fillAmount = 0;

            if(isAnimated)
                await fillTransform.DOAnchorMax(new Vector2(fillAmount, 1), 0.5f).SetEase(Ease.OutQuint).ToUniTask();
            else
                fillTransform.localScale = new Vector3(fillAmount, 1, 1);
            
            text.text = $"{curHp}/{maxHp}";
        }
        
        public async UniTask SetHpBar(ReactiveProperty<int> curHp, ReactiveProperty<int> maxHp, bool isAnimated = true)
        {
            await SetHpBar(curHp.Value, maxHp.Value, isAnimated);
        }
    }
}