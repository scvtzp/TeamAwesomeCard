using System.Collections.Generic;
using SkillSystem;
namespace AllObject.Item
{
    public class ItemModel : Model, IHasID
    {
        public string id { get; set; } //id를 직접 들고있는게 맞나?
        public List<Skill> SkillList = new List<Skill>();

        public ItemModel(string id)
        {
            this.id = id;
            ModelType = ModelType.Item;
        }
    }
}