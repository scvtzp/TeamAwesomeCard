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
        private static Object selectedObject;
        private static List<float> frame = new List<float>();
        private static List<Animation> _animation = new List<Animation>();
        
        static PrefabAnimationAutoPlay()
        {
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        }

        private static void OnPrefabStageOpened(PrefabStage stage)
        {
            selectedObject = stage.prefabContentsRoot;
            _animation.Clear();
            frame.Clear();
            
            Animation[] animations = stage.prefabContentsRoot.GetComponentsInChildren<Animation>(true);
            if (animations.Length > 0)
            {
                EditorApplication.delayCall += () =>
                {
                    foreach (var animation in animations)
                    {
                        _animation.Add(animation);
                        frame.Add(0);
                        animation.Play();
                        Debug.Log(animation.clip.name);
                    }
                };
            }
        }
        
        private static void Update () 
        {
            if (Selection.activeObject != selectedObject)
                return;
            if (Selection.activeObject == null)
                return;

            var deltaTime = Time.deltaTime;
            
            for (var i = 0; i < _animation.Count; i++)
            {
                var ani = _animation[i];
                
                frame[i] += deltaTime * ani.clip.frameRate;

                if (frame[i] > ani.clip.frameRate)
                    frame[i] = 0;

                float time = (frame[i] / ani.clip.frameRate); // 프레임 번호를 초로 변환

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