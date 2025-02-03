using AllObject;
using DefaultNamespace;

namespace SkillSystem
{
    /// <summary>
    /// Value 0 : 딜량 (음수면 힐됨)
    /// </summary>
    public class Damage : Skill
    {
        public Damage() { }
        public Damage(int[] value) : base(value) { }
        public Damage(TargetType targetTypeType, params int[] value) : base(targetTypeType, value) { }
        
        public override void StartSkill(IStat target)
        {
            target.ChangeHp(-Values[0]);
        }
        
        public override SkillSystem.Skill Clone()
        {
            return new Damage(TargetType, Values);
        }
    }
}