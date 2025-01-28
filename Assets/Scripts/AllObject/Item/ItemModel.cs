using System.Collections.Generic;
using SkillSystem;

namespace AllObject.Item
{
    public class ItemModel
    {
        public List<Skill> SkillList = new List<Skill>();

        //테스트용.
        public ItemModel()
        {
            SkillList.Add(new Damage(new int[] {6}));
        }
    }
}