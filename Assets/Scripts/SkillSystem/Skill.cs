using System;
using System.Collections.Generic;
using AllObject;
using Manager;

namespace SkillSystem
{
    [Flags]
    public enum TargetType
    {
        None = 0, //그냥 내면 됨.
        Select = 1<<0, //내가 따로 선택한 단일 개체.
        Player = 1<<1, //본인
        Ally = 1<<2, //아군
        Enemy = 1<<3, //적군
        All = 1<<4, //모두
        AllEnemy = 1<<5, //모든 적
        SelectRight = 1<<6, //선택한 적의 오른족
        SelectLeft = 1<<7, //선택한 적의 왼쪽
        
    }
    
    public enum TriggerType
    {
        None, //트리거 없음
        SkillStart,
        SkillEnd,
        LowerTargetHP, //남은 HP가 수치보다 낮다면
        TurnCycle, //턴 시작 시
    }

    public enum SkillCommonValue
    {
        Value,
        AffectType,
        AttackType,
        TargetType,
        TriggerValue,
        Duration,
    }

    // public class UnionType
    // {
    //     private int intValue;
    //     private string stringValue;
    //     private TargetType targetType;
    // }
    
    public abstract class Skill
    {
        // 카드 안에 여러 기능 있을때 엮기 위해서 사용. (적에게 피해를 3 주고, 본체에도 피해를 2준다)
        // 해당 부분에서 본체에도 피해 2줄때는 고정 타겟 있어야 해서 ISkill class로 바꾸고 그냥 타겟 같이 넣음.
        // IAutoSelect 같은거 만들어서 넣어줘야 하나 싶은데 그럼 iskill로 하나로 뭉치기 좀 힘들어서 고민필요할듯.
        // override가 아니라 무조건 만들어줘야 하는거라 인터페이스가 더 이쁘긴 한데.. 씁
        protected Dictionary<SkillCommonValue, string> SkillValues;
        
        protected TargetType TargetType = TargetType.None;
        protected IStat Target;
        
        protected TriggerType TriggerType = TriggerType.None;

        public bool NeedSelectTarget()
        {
            if(TargetType.HasFlag(TargetType.Select))
                return true;
            if(TargetType.HasFlag(TargetType.Enemy))
                return true;
            if(TargetType.HasFlag(TargetType.SelectRight))
                return true;
            if(TargetType.HasFlag(TargetType.SelectLeft))
                return true;
            return false;
        }

        public Skill()
        {
            
        }
        
        public Skill(TargetType targetType, TriggerType triggerType, Dictionary<string,string> dataCache)
        {
            TargetType = targetType;
            TriggerType = triggerType;
            
            SkillValues = new Dictionary<SkillCommonValue, string>();
            foreach (var kvp in dataCache)
            {
                SkillCommonValue type = Enum.Parse<SkillCommonValue>(kvp.Key);
                SkillValues.Add(type, kvp.Value);
            }
        }
        
        public Skill(TargetType targetTypeType, TriggerType triggerType, params int[] value) : this()
        {
            TargetType = targetTypeType;
            TriggerType = triggerType;
        }

        protected abstract void StartSkill(IStat selectTarget);

        public virtual void AddTriggerAction(IStat target)
        {
            // 패시브 추가랑 이번 한번만 쓸거 추가 따로 넣어줘야함.
            TriggerManager.Instance.AddTriggerAction(TriggerType, StartSkill, target, 0);
        }
        
        public abstract Skill Clone();
    }
}