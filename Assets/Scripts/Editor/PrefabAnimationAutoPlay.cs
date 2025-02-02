using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PrefabAnimationAutoPlay
{
    static PrefabAnimationAutoPlay()
    {
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
    }

    private static void OnPrefabStageOpened(PrefabStage stage)
    {
        Animation[] animations = stage.prefabContentsRoot.GetComponentsInChildren<Animation>(true);
        if (animations.Length > 0)
        {
            EditorApplication.delayCall += () =>
            {
                foreach (var animation in animations)
                {
                    // 애니메이션이 실행되지 않았으면 실행
                    if (!animation.isPlaying)
                    {
                        animation.Play();
                    }
                    
                    // 애니메이션 갱신 강제 실행
                    animation.Sample();
                    
                    // 애니메이션 다시 샘플링해서 초기화
                    animation.Sample(); 
                }
            };
        }
    }
}