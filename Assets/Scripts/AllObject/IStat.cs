using Manager;
using R3;
using SkillSystem;

namespace AllObject
{
    public interface IStat
    {
        public ReactiveProperty<int> hp { get; set; }
        public ReactiveProperty<int> maxHp { get; set; }
        public ReactiveProperty<int> atk { get; set; }
        public ReactiveProperty<int> def { get; set; }
        
        public void ChangeHp(int value)
        {
            hp.Value += value;
            
            if (value > 0) //0보다 작으면 힐임.
            {
                TriggerManager.Instance.ExecuteTrigger(TriggerType.GetDamage, this);
                TriggerManager.Instance.ExecuteTrigger(TriggerType.LowerTargetHP, this);
            }
        }
    }
}