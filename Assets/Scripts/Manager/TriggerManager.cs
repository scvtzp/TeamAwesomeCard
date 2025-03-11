using System;
using System.Collections.Generic;
using System.Linq;
using AllObject;
using Manager.Generics;
using SkillSystem;

namespace Manager
{
    public class SkillData
    {
        public SkillData(Action<IStat> action, IStat target, int invokeLimit)
        {
            Action = action;
            Target = target;
            InvokeLimit = invokeLimit;
        }
        
        public Action<IStat> Action;
        public IStat Target;
        public int InvokeLimit;
    }
    
    public class TriggerManager : MonoSingleton<TriggerManager>
    {
        private Dictionary<TriggerType, List<SkillData>> _triggerActionDictionary = new();
        
        public void AddTriggerAction(TriggerType triggerType, Action<IStat> action, IStat target, int invokeLimit)
        {
            if (!_triggerActionDictionary.ContainsKey(triggerType))
                _triggerActionDictionary[triggerType] = new List<SkillData>(); // 리스트 초기화
            
            _triggerActionDictionary[triggerType].Add(new SkillData(action, target, invokeLimit));
        }

        public void ExecuteTrigger(TriggerType triggerType)
        {
            // 스킬 사용이 끝나면 일단 일회용 스킬들 모두 삭제하는 것 부터.
            if (triggerType == TriggerType.SkillEnd)
                RemoveTemporaryTriggerAction();
            
            if (!_triggerActionDictionary.TryGetValue(triggerType, out var targetList)) 
                return;
            
            foreach (var skillData in targetList.ToList())
            {
                skillData.Action.Invoke(skillData.Target);
                skillData.InvokeLimit--;
                if(skillData.InvokeLimit <= 0)
                    targetList.Remove(skillData);
            }
        }

        private void RemoveTemporaryTriggerAction()
        {
            // 해당 카드를 사용한 시점에서만 적용되는 "즉시" 효과들은 모두 시작 InvokeLimit가 -1이다.
            foreach (var pair in _triggerActionDictionary)
            {
                foreach (var skillData in pair.Value.ToList())
                {
                    if(skillData.InvokeLimit == -1)
                        pair.Value.Remove(skillData);
                }
            }
        }
    }
}