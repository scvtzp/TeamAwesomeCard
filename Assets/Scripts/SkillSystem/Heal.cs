using System.Collections.Generic;
using AllObject;
using DefaultNamespace;
using Manager;

namespace SkillSystem
{
    /// <summary>
    /// 데미지에 -하면 힐이지만, 가독성을 위해서 그냥 클래스 나눔.
    /// Value 0 : 힐량
    /// </summary>
    public class Heal : Skill
    {
        public Heal(TargetType targetTypeType, TriggerType triggerType, Dictionary<SkillCommonValue, string> skillValues) : base(targetTypeType, triggerType, skillValues) { }
        public Heal(TargetType targetType, TriggerType triggerType, Dictionary<string, string> dataCache) : base(targetType, triggerType, dataCache) { }

        protected override void StartSkill(IStat selectTarget)
        {
            foreach (var target in TargetManager.Instance.GetTarget(TargetType, selectTarget))
            {
                target.ChangeHp(SkillValues[SkillCommonValue.Value].ToInt());
            }
        }

        public override SkillSystem.Skill Clone()
        {
            //value 삭제한 개선본으로 수정필요
            return new Heal(TargetType, TriggerType, SkillValues);
        }
    }
}