using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using SkillSystem;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace DefaultNamespace
{
    public static class GameUtil
    {
        private static readonly Random Random = new Random();

        public static TValue GetRandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                throw new InvalidOperationException("딕셔너리가 비어 있습니다.");

            int index = Random.Next(dictionary.Count);
            return dictionary.ElementAt(index).Value;
        }
        
        public static int ToInt(this string str) => int.Parse(str);

        public static bool NeedSelectTarget(this TargetType targetType)
        {
            if(targetType.HasFlag(TargetType.Select))
                return true;
            if(targetType.HasFlag(TargetType.Enemy))
                return true;
            return false;
        }

        private static string[] GetSmartKeys(string input)
        {
            var matches = Regex.Matches(input, @"\{([^{}]+)\}");
            string[] keys = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                keys[i] = matches[i].Groups[1].Value;
            }
            return keys;
        }
        
        /// 로컬라이징에서 스마트 스트링 키값만 골라서 추출해줌. 
        public static async UniTask<string[]> CheckLocalizedStringKeys(string tableName, string entryKey)
        {
            var handle = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
            await handle.Task;
            if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load table: {tableName}");
                return Array.Empty<string>();
            }

            var table = handle.Result;
            if (table == null)
            {
                Debug.LogError($"Table '{tableName}' not found!");
                return Array.Empty<string>();
            }

            var entry = table.GetEntry(entryKey);
            if (entry == null)
            {
                Debug.LogError($"Entry '{entryKey}' not found in table '{tableName}'!");
                return Array.Empty<string>();
            }

            string localizedText = entry.LocalizedValue;
            string[] keys = GetSmartKeys(localizedText);

            Debug.Log($"Smart Keys in '{entryKey}': " + string.Join(", ", keys));
            return keys;
        }
    }
    
    public static class LocalizeTable
    {
        public const string InGameObject = "InGameObject";
    }
}