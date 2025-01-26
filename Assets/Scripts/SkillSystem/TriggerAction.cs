// using System.Collections.Generic;
// using DefaultNamespace;
// using Manager;
//
// namespace SkillSystem
// {
//     public class TriggerAction : Skill 
//     {
//         public TriggerAction(TriggerType triggerType, TargetType triggerSource, TargetType target, List<Skill> skills, params int[] value) : base(value)
//         {
//             _triggerData = new TriggerData(triggerType, triggerSource, target, Values[0], skills);
//         }
//         
//         public TriggerAction(TargetType targetType, TriggerType triggerType, TargetType triggerSource, TargetType target, List<Skill> skills, params int[] value) : base(targetType, value)
//         {
//             _triggerData = new TriggerData(triggerType, triggerSource, target, Values[0], skills);
//         }
//
//         public TriggerAction(TargetType targetType, TriggerData triggerData, params int[] value) : base(targetType, value)
//         {
//             _triggerData = new TriggerData(triggerData);
//         }
//
//         private TriggerData _triggerData;
//         
//         public override void StartSkill(Entity target)
//         {
//             TriggerManager.Instance.AddAction(_triggerData);
//         }
//
//         public override Skill Clone()
//         {
//             return new TriggerAction(Target, _triggerData, Values);
//         }
//     }
// }