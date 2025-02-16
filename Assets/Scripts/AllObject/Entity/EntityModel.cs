using System;
using AllObject;
using R3;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntityModel : Model, IStat, IHasID
    {
        public EntityModel()
        {
            ModelType = ModelType.Entity;
        }
        
        public EntityModel(EntityModel model) : this()
        {
            this.id = model.id;
            
            this.hp = new ReactiveProperty<int>(model.hp.Value); 
            this.maxHp = new ReactiveProperty<int>(model.maxHp.Value); 
            this.atk = new ReactiveProperty<int>(model.atk.Value); 
            this.def = new ReactiveProperty<int>(model.def.Value); 
        }
        
        public EntityModel(string id, string hp, string atk, string def) : this()
        {
            int hpValue = int.Parse(hp);
            int atkValue = int.Parse(atk);
            int defValue = int.Parse(def);

            this.id = id;
            
            this.hp = new ReactiveProperty<int>(hpValue); 
            this.maxHp = new ReactiveProperty<int>(hpValue); 
            this.atk = new ReactiveProperty<int>(atkValue); 
            this.def = new ReactiveProperty<int>(defValue); 
        }
        
        public ReactiveProperty<int> hp { get; set; } = new ReactiveProperty<int>(20);
        public ReactiveProperty<int> maxHp { get; set; } = new ReactiveProperty<int>(20);
        public ReactiveProperty<int> atk { get; set; } = new ReactiveProperty<int>(5);
        public ReactiveProperty<int> def { get; set; } = new ReactiveProperty<int>(0);
        
        public string id { get; set; }
    }
}