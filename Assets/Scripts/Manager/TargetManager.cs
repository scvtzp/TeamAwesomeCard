using System.Collections.Generic;
using AllObject;
using DefaultNamespace;
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

            // 고르는 타입이면 무조건 리턴. 다른거랑 같이 되는 일은 없다. 그런 경우 동일한 스킬 타입만 바꿔서 2줄 써주면 됨.
            // ex) 선택된 대상과 나의 생명력 3 회복 => 회복3(적[Select]) \n 회복3(나)
            if (targetType.NeedSelectTarget())
            {
                if(selectTarget != null)
                    returnList.Add(selectTarget);
                else if(selectTarget == null)
                    Debug.Log("Select Target null");

                return returnList;
            }
            
            if (targetType.HasFlag(TargetType.Player))
                returnList.Add(BattleManager.Instance.GetPlayer());

            if (targetType.HasFlag(TargetType.All))
            {
                foreach (var model in StageManager.Instance.GetAllFirstEntity())
                    if(model != null)
                        returnList.Add(model);
            }

            if (targetType.HasFlag(TargetType.SelectRight))
            {
                bool check = false;
                var allList = StageManager.Instance.GetAllFirstEntity();
                foreach (var model in allList)
                {
                    if (check)
                    {
                        if(model != null)
                            returnList.Add(model);
                        
                        break;
                    }
                    
                    if (model == selectTarget)
                        check = true;
                }
            }
            if (targetType.HasFlag(TargetType.SelectLeft))
            {
                bool check = false;
                var allList = StageManager.Instance.GetAllFirstEntity();
                for (var index = allList.Count-1; index >= 0 ; index--)
                {
                    var model = allList[index];
                    if (check)
                    {
                        if (model != null)
                            returnList.Add(model);

                        break;
                    }

                    if (model == selectTarget)
                        check = true;
                }
            }
            
            return returnList;
        }
    }
}