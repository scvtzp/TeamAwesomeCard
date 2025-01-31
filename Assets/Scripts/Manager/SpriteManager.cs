using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Manager.Generics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace Manager
{
    public class SpriteManager : MonoSingleton<SpriteManager>
    {
        private bool isLoadDone { get; set; }
        private readonly Dictionary<string, Sprite> _spriteDic = new Dictionary<string, Sprite>();
        
        // 초기화 순서 때문에 임시로 스테이지 메니저에서 초기화 시키는중.
        // 나중에 젠젝트 써서 초기화 순서 직접 정해주자.
        private void Start()
        {
            //LoadMainSceneAssets();
        }

        public async UniTask LoadMainSceneAssets()
        {
            if (isLoadDone)
                return;

            var tasks = new List<UniTask>();
            tasks.Add(LoadAtlasByTag("Atlas"));
            await UniTask.WhenAll(tasks);

            Debug.Log("리소스 로드 완료");
            
            isLoadDone = true;
        }

        /// 한번에 모두 로드해두는 방식. 나중에 최적화 필요하면 수정 필요
        private UniTask LoadAtlasByTag(string tag)
        {
            return Addressables.LoadAssetsAsync<SpriteAtlas>(tag, atlas =>
            {
                var allSprites = new Sprite[atlas.spriteCount];
                atlas.GetSprites(allSprites);

                foreach (var sprite in allSprites)
                {
                    string name = sprite.name.Replace("(Clone)", "");
                    _spriteDic.Add(name, sprite);
                }
            }).ToUniTask();
        }

        public Sprite GetSprite(string name)
        {
            return _spriteDic[name];
        }
    }
}