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
        private static float frame;
        private static List<Animation> _animation = new List<Animation>();
        
        static PrefabAnimationAutoPlay()
        {
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        }

        private static void OnPrefabStageOpened(PrefabStage stage)
        {
            selectedObject = stage.prefabContentsRoot;
            _animation.Clear();
            
            Animation[] animations = stage.prefabContentsRoot.GetComponentsInChildren<Animation>(true);
            if (animations.Length > 0)
            {
                EditorApplication.delayCall += () =>
                {
                    foreach (var animation in animations)
                    {
                        _animation.Add(animation);
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

            foreach (var ani in _animation)
            {
                frame += Time.deltaTime * ani.clip.frameRate;

                if (frame > ani.clip.frameRate)
                    frame = 0;
                
                float time = (frame / ani.clip.frameRate); // 프레임 번호를 초로 변환
                
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