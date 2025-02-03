using AllObject;
using DefaultNamespace;
using SkillSystem;

namespace SkillSystem
{
    /// <summary>
    /// 데미지에 -하면 힐이지만, 가독성을 위해서 그냥 클래스 나눔.
    /// Value 0 : 힐량
    /// </summary>
    public class Heal : Skill
    {
        public Heal() { }
        public Heal(int[] value) : base(value) { }
        public Heal(TargetType targetTypeType, params int[] value) : base(targetTypeType, value) { }
        
        public override void StartSkill(IStat target)
        {
            target.ChangeHp(Values[0]);
        }
        
        public override SkillSystem.Skill Clone()
        {
            return new Heal(TargetType, Values);
        }
    }
}