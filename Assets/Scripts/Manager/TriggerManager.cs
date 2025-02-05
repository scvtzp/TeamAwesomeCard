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
            // 해당 스킬 사용이 끝난거면 일단 일회용 스킬들 모두 치우는거부터.
            if (triggerType == TriggerType.SkillEnd)
                RemoveTemporaryTriggerAction();
            
            if (!_triggerActionDictionary.TryGetValue(triggerType, out var actions)) 
                return;
            
            foreach (var skillData in actions.ToList())
            {
                skillData.Action.Invoke(skillData.Target);
                skillData.InvokeLimit--;
                if(skillData.InvokeLimit <= 0)
                    actions.Remove(skillData);
            }
        }

        private void RemoveTemporaryTriggerAction()
        {
            // 일회용 스킬들은 스킬 제한이 시작부터 -1으로 들어가있다.
            foreach (var pair in _triggerActionDictionary)
            {
                foreach (var skillData in pair.Value.ToList())
                {
                    if(skillData.InvokeLimit <= -1)
                        pair.Value.Remove(skillData);
                }
            }
        }
    }
}