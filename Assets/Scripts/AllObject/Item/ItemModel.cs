using System.Collections.Generic;
using SkillSystem;

namespace AllObject.Item
{
    public class ItemModel : IHasID
    {
        public List<Skill> SkillList = new List<Skill>();

        //테스트용.
        public ItemModel()
        {
            SkillList.Add(new Damage(TargetType.Select, TriggerType.SkillStart, new int[] {6}));
        }
        
        //테스트용.
        public ItemModel(int a)
        {
            SkillList.Add(new Heal(TargetType.Me, TriggerType.SkillStart, new int[] {5}));
        } 

        public string id { get; set; }
    }
}