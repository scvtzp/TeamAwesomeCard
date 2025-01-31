using R3;

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
        }
    }
}