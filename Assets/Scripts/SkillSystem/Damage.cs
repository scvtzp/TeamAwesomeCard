using System.Collections.Generic;
using AllObject;
using DefaultNamespace;
using Manager;

namespace SkillSystem
{
    /// <summary>
    /// Value 0 : 딜량 (음수면 힐됨)
    /// </summary>
    public class Damage : Skill
    {
        public Damage(TargetType targetTypeType, TriggerType triggerType, params int[] value) : base(targetTypeType, triggerType, value) { }
        public Damage(TargetType targetType, TriggerType triggerType, Dictionary<string, string> dataCache) : base(targetType, triggerType, dataCache) { }
        
        protected override void StartSkill(IStat selectTarget)
        {
            foreach (var target in TargetManager.Instance.GetTarget(TargetType, selectTarget))
            {
                target.ChangeHp(-SkillValues[SkillCommonValue.Value].ToInt());
            }            
        }

        public override void AddTriggerAction(IStat target)
        {
            TriggerManager.Instance.AddTriggerAction(TriggerType, StartSkill, target, 1);
        }
        
        public override Skill Clone()
        {
            return new Damage(TargetType, TriggerType);
        }
    }
}