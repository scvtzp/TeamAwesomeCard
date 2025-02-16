namespace AllObject
{
    public enum ModelType
    {
        Entity,
        Item,
        Skill
    }
    
    public class Model
    {
        public ModelType ModelType { get; protected set; }
    }
}