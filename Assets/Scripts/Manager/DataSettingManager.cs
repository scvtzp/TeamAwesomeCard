using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AllObject.Item;
using DefaultNamespace;
using Manager.Generics;
using SkillSystem;
using UnityEngine;

namespace Manager
{
    public class DataSettingManager : MonoSingleton<DataSettingManager>
    {
        //InstantiateClassByName 캐싱용
        private Dictionary<string, Type> typeCache = new();
        private Dictionary<Tuple<string, Type[]>, ConstructorInfo> constructorCache = new();
        
        public Dictionary<string, EntityModel> EntityData = new Dictionary<string, EntityModel>();
        public Dictionary<string, ItemModel> ItemData = new Dictionary<string, ItemModel>();
        
        private void Start()
        {
            LoadEntitySetting();
            LoadItemSetting();
        }

        private void LoadEntitySetting()
        {
            TextAsset csvFile = Resources.Load<TextAsset>("Data/EntitySetting");
            string[] lines = csvFile.text.Split('\n');

            for (var i = 1; i < lines.Length; i++) //0번은 헤더라서 뺌.
            {
                var line = lines[i];
                string[] columns = line.Split(',');

                var model = new EntityModel(columns[0], columns[1], columns[2],columns[3]);
                
                EntityData.Add(model.id, model);
            }
            
            Debug.Log("몬스터 세팅 완료.");
        }

        private void LoadItemSetting()
        {
            TextAsset csvFile = Resources.Load<TextAsset>("Data/ItemSetting");
            string[] lines = csvFile.text.Split('\n');
            
            //0행 (헤더) 키-열 매핑.
            Dictionary<string, int> headerCache = new Dictionary<string, int>();
            var headers = lines[0].Split(',');
            for (var i = 0; i < headers.Length; i++)
            {
                var str = headers[i];
                headerCache.Add(str, i);
            }

            // 파일 불러오기
            ItemModel itemModel = null;
            for (var i = 1; i < lines.Length; i++) //0번은 헤더라서 뺌.
            {
                var line = lines[i];
                string[] columns = line.Split(',');

                if (columns[0] != "")
                    itemModel = new ItemModel(columns[0]);
                
                //타겟 값 가져옴
                TargetType targetType = TargetType.None;
                var targetStr = columns[(int)getIndex("Target")].Split('|');
                foreach (var str in targetStr)
                    targetType |= Enum.Parse<TargetType>(str);
                
                //트리거 값 가져옴
                TriggerType triggerType;
                int? triggerIndex = getIndex($"Trigger");
                if (columns[(int)triggerIndex] != "")
                    triggerType = Enum.Parse<TriggerType>(columns[(int)triggerIndex]);
                else
                    triggerType = TriggerType.SkillStart;
                
                // CommonX 값들 다 모아줌.
                Dictionary<string,string> dataCache = new Dictionary<string, string>();
                for (int j = 1;; j++)
                {
                    int? index = getIndex($"Common{j}");
                    if (index != null && columns[(int)index] != "")
                    {
                        var str = columns[(int)index].Split('=');
                        dataCache.Add(str[0], str[1]);
                    }
                    else
                        break;
                }
                
                var skill = (Skill)InstantiateClassByName(columns[(int)getIndex("SkillType")], targetType, triggerType, dataCache);
                itemModel.SkillList.Add(skill);
                
                if(i+1 == lines.Length || lines[i+1].Split(',')[0] != "")
                    ItemData.Add(itemModel.id, itemModel);
            }
            
            Debug.Log("아이템 세팅 완료.");

            // 헤더 넣으면 해당 열의 인덱스 반환
            int? getIndex(string header)
            {
                if(headerCache.TryGetValue(header, out var value))
                    return value;
                
                return null;
            }
        }
        
        /// 자동으로 인자 넣어서 생성자로 생성해주는 함수
        private object InstantiateClassByName(string className, params object[] parameters)
        {
            if (!typeCache.TryGetValue(className, out Type type))
            {
                type = Type.GetType($"SkillSystem.{className}"); //네임스페이스 바뀌면 바꿔줘야함.
                typeCache[className] = type;
            }

            Type[] parameterTypes = parameters.Select(p => p.GetType()).ToArray();
            var cacheKey = new Tuple<string, Type[]>(className, parameterTypes);

            if (!constructorCache.TryGetValue(cacheKey, out ConstructorInfo constructor))
            {
                constructor = type.GetConstructor(parameterTypes);
                constructorCache[cacheKey] = constructor;
            }

            return constructor.Invoke(parameters);
        }
    }
}