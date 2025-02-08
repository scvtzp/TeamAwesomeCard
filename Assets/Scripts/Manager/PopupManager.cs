using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Manager.Generics;
using UI.UIBase;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Manager
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        protected override bool DontDestroy => false;
        
        private Dictionary<string, PopupBase> _popupDictionary = new Dictionary<string, PopupBase>();
        private GameObject _root;

        public async UniTask ShowView<T>() where T : PopupBase
        {
            var viewName = typeof(T).Name;

            await ShowView(viewName);
        }

        public void ShowViewVoid(string viewName)
        {
            ShowView(viewName).Forget(); // UniTask 실행 후 예외 무시
        }
        
        private async UniTask ShowView(string viewName)
        {
            if (_popupDictionary.TryGetValue(viewName, out PopupBase view))
                await ShowView(view);
            
            else
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Assets/Prefabs/Popup/{viewName}.prefab");
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var prefab = handle.Result;
                        
                    view = GameObject.Instantiate(prefab, GetCanvas()).GetComponent<PopupBase>();
                    _popupDictionary.Add(viewName, view);
                    view.Init();
                    await ShowView(view);
                }
                else
                    Debug.LogWarning($"어드레서블에서 {viewName}를 찾을 수 없습니다.");
            }
        }
        
        private Transform GetCanvas()
        {
            if(_root != null)
                return _root.transform;
            
            var obj = GameObject.Find("Canvas");
            if (obj == null)
            {
                Debug.LogWarning($"Canvas를 찾을 수 없습니다.");
                return null;
            }
            else
            {
                _root = obj;
                return _root.transform;
            }
        }
        
        private async Task ShowView(PopupBase popup)
        {
            await popup.ShowStart();
            popup.Show();
            await popup.ShowEnd();
        }
        
        public void HideView<T>() where T : PopupBase => HideView(typeof(T).Name);
        public void HideView(string viewName) => HideView(_popupDictionary[viewName]);
        private void HideView(PopupBase popup)
        {
            popup.HideStart();
            popup.Hide();
            popup.HideEnd();
        }
    }
}