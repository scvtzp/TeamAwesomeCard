using System;
using DefaultNamespace;

namespace SkillSystem
{
    [Flags]
    public enum TargetType
    {
        None = 0, //그냥 내면 됨.
        Me = 1<<0, //본인
        Ally = 1<<1, //아군
        Enemy = 1<<2, //적군
        
        Summoner = 1<<3, //소환된 잡몹
        Select = 1<<4, //내가 따로 선택한 단일 개체.
    }
    
    public abstract class Skill : ICardAble
    {
        // 카드 안에 여러 기능 있을때 엮기 위해서 사용. (적에게 피해를 3 주고, 본체에도 피해를 2준다)
        // 해당 부분에서 본체에도 피해 2줄때는 고정 타겟 있어야 해서 ISkill class로 바꾸고 그냥 타겟 같이 넣음.
        // IAutoSelect 같은거 만들어서 넣어줘야 하나 싶은데 그럼 iskill로 하나로 뭉치기 좀 힘들어서 고민필요할듯.
        // override가 아니라 무조건 만들어줘야 하는거라 인터페이스가 더 이쁘긴 한데.. 씁
        public TargetType Target = TargetType.None;
        protected int[] Values;
        public bool NeedSelectTarget => Target == TargetType.None;
        
        public Skill(params int[] value)
        {
            Values = new int[value.Length];
            for (var i = 0; i < value.Length; i++)
                Values[i] = value[i];
        }
        public Skill(TargetType targetType, params int[] value) : this(value)
        {
            Target = targetType;
        }

        public abstract void StartSkill(EntityModel target);
        public abstract Skill Clone();
        public int[] GetValues() => Values;
        
        public void SetCardFace(bool isFront = true)
        {
            throw new NotImplementedException();
        }
    }
}