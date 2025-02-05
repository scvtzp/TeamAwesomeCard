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
    }
}