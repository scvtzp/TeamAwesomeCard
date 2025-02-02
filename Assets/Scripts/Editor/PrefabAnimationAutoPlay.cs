using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public static class PrefabAnimationAutoPlay
    {
        private static Object _selectedObject;
        private static List<float> _frame = new List<float>();
        private static List<bool> _oneCycleCheck = new List<bool>();
        private static List<Animation> _animation = new List<Animation>();
        
        static PrefabAnimationAutoPlay()
        {
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        }

        private static void OnPrefabStageOpened(PrefabStage stage)
        {
            _selectedObject = stage.prefabContentsRoot;
            _animation.Clear();
            _frame.Clear();
            _oneCycleCheck.Clear();
            
            Animation[] animations = stage.prefabContentsRoot.GetComponentsInChildren<Animation>(true);
            if (animations.Length > 0)
            {
                EditorApplication.delayCall += () =>
                {
                    foreach (var animation in animations)
                    {
                        _animation.Add(animation);
                        _frame.Add(0);
                        _oneCycleCheck.Add(false);
                        
                        animation.Play();
                        Debug.Log(animation.clip.name);
                    }
                };
            }
        }
        
        private static void Update () 
        {
            if (Selection.activeObject != _selectedObject)
                return;
            if (Selection.activeObject == null)
                return;

            var deltaTime = Time.deltaTime;
            
            for (var i = 0; i < _animation.Count; i++)
            {
                if(_oneCycleCheck[i])
                    continue;
                
                var ani = _animation[i];
                
                _frame[i] += deltaTime * ani.clip.frameRate;

                if (_frame[i] > ani.clip.frameRate)
                {
                    _oneCycleCheck[i] = true;
                    _frame[i] = 0;
                }

                float time = (_frame[i] / ani.clip.frameRate); // 프레임 번호를 초로 변환

                ani[ani.clip.name].time = time;
                ani.Sample();
            }
        }
        
        [InitializeOnLoadMethod, UsedImplicitly]
        private static void SubscribeToUpdate () {
            EditorApplication.update += Update;
        }
    }
}