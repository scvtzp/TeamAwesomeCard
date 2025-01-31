using System;
using System.Collections.Generic;
using DefaultNamespace;
using Manager.Generics;
using UnityEngine;

namespace Manager
{
    public class DataSettingManager : MonoSingleton<DataSettingManager>
    {
        public Dictionary<string, EntityModel> EntityData = new Dictionary<string, EntityModel>();

        private void Start()
        {
            LoadEntitySetting();
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
    }
}