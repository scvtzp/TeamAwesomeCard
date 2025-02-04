using System;
using System.Collections.Generic;
using AllObject;
using Manager.Generics;
using SkillSystem;

namespace Manager
{
    public struct SkillData
    {
        public SkillData(Action<IStat> action, IStat target)
        {
            Action = action;
            Target = target;
        }
        
        public Action<IStat> Action;
        public IStat Target;
    }
    
    public class TriggerManager : MonoSingleton<TriggerManager>
    {
        private Dictionary<TriggerType, List<SkillData>> _triggerActionDictionary = new();
        
        public void AddTriggerAction(TriggerType triggerType, Action<IStat> action, IStat target)
        {
            if (!_triggerActionDictionary.ContainsKey(triggerType))
                _triggerActionDictionary[triggerType] = new List<SkillData>(); // 리스트 초기화
            
            _triggerActionDictionary[triggerType].Add(new SkillData(action, target));
        }

        public void ExecuteTrigger(TriggerType triggerType)
        {
            if (!_triggerActionDictionary.TryGetValue(triggerType, out var actions)) 
                return;
            
            foreach (var skillData in actions)
            {
                // 이미 타겟이 사라지면 리턴.
                if (skillData.Target == null)
                    return;
                
                skillData.Action.Invoke(skillData.Target);
            }
        }
    }
}