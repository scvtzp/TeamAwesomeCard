using System.Collections.Generic;
using AllObject;
using Manager.Generics;
using SkillSystem;
using UnityEngine;

namespace Manager
{
    public class TargetManager : MonoSingleton<TargetManager>
    {
        public List<IStat> GetTarget(TargetType targetType, IStat selectTarget)
        {
            var returnList = new List<IStat>();

            if (targetType.HasFlag(TargetType.Me))
                returnList.Add(BattleManager.Instance.GetPlayer());
            
            if(targetType.HasFlag(TargetType.Select) && selectTarget != null)
                returnList.Add(selectTarget);
            else if(targetType.HasFlag(TargetType.Select) && selectTarget == null)
                Debug.Log("Select Target null");
            
            return returnList;
        }
    }
}