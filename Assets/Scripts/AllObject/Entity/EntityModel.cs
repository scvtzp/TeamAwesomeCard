using R3;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntityModel : IStat
    {
        public ReactiveProperty<int> hp { get; set; } = new ReactiveProperty<int>(20);
        public ReactiveProperty<int> maxHp { get; set; } = new ReactiveProperty<int>(20);
        public ReactiveProperty<int> atk { get; set; } = new ReactiveProperty<int>(5);
        public ReactiveProperty<int> def { get; set; } = new ReactiveProperty<int>(0);

        public void ChangeHp(int value)
        {
            
        }
    }
}