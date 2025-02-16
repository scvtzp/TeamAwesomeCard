using System.Collections.Generic;
using System.Linq;
using AllObject;
using AllObject.Entity;
using AllObject.Item;
using DefaultNamespace;
using Manager.Generics;
using UI.Popup.StageReward;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    /// <summary>
    /// 스테이지 이동과 적 생성 등을 관리.
    /// </summary>
    public class StageManager : MonoSingleton<StageManager>
    {
        public Transform dragParents;
        [SerializeField] EntityView entityViewPrefab;
        [SerializeField] ItemView ItemViewPrefab;
        
        [SerializeField] List<Transform> cardListParents;
        public List<List<(Model, CardAble, ICardPresenter)>> CardList { get; private set; }

        private async void Start()
        {
            await SpriteManager.Instance.LoadMainSceneAssets();
            SetStage();
        }

        /// <summary>
        /// 스테이지 변경 시 호출
        /// </summary>
        public void SetStage()
        {
            //테스트용 초기화 코드.
            for (int i = 0; i < cardListParents.Count; i++)
            {
                for (int j = 0; j < cardListParents[i].childCount; j++)
                    Destroy(cardListParents[i].GetChild(j).gameObject);
            }
            
            // 초기화
            CardList = new List<List<(Model, CardAble, ICardPresenter)>>();
            for (int i = 0; i < 5; i++)
            {
                CardList.Add(new List<(Model, CardAble, ICardPresenter)>());
            }
            
            // 라인별로 무작위 갯수만큼 카드 생성.
            for (int i = 0; i < CardList.Count; i++)
            {
                for (int j = 0; j < Random.Range(1, 5); j++)
                {
                    if(Random.Range(1, 4) != 2)
                    {
                        var model = new EntityModel(DataSettingManager.Instance.EntityData.GetRandomValue());
                        var view = Instantiate(entityViewPrefab, cardListParents[i]);
                        var presenter = new EntityPresenter(model, view);
                        
                        CardList[i].Add((model, view, presenter));
                    }
                    else
                    {
                        ItemModel model = DataSettingManager.Instance.ItemData.GetRandomValue();
                        var view = Instantiate(ItemViewPrefab, cardListParents[i]);
                        var presenter = new ItemPresenter(model, view);
                        
                        CardList[i].Add((model, view, presenter));
                    }
                }
            }

            SortCardPos();
            CloseAllCard();
            OpenLastCard();
        }

        private void CloseAllCard()
        {
            foreach (var list in CardList)
                foreach (var card in list)
                    card.Item3.SetCardFace(false);
        }

        /// <summary>
        /// 카드 정렬.
        /// </summary>
        private void SortCardPos()
        {
            for (var index = 0; index < CardList.Count; index++)
            {
                for (int i = CardList[index].Count - 1; i >= 0; i--)
                {
                    var pos = (i - (CardList[index].Count - 1)) * -30f;
                    CardList[index][i].Item2.SetPos(new Vector3(0, pos, 0), cardListParents[index]);
                }
            }
        }
        
        /// 특정 카드가 죽으면 아래 카드 뒤집어준다.
        private void OpenLastCard()
        {
            //맨 앞 카드 뒤집어줌
            for (int i = 0; i < CardList.Count; i++)
            {
                if(CardList[i].Count > 0)
                    CardList[i][^1].Item3.SetCardFace(true);
            }
        }

        public void DeathAction(ICardPresenter presenter)
        {
            presenter.Death();
            
            foreach (var subList in CardList)
            {
                for (var index = 0; index < subList.Count; index++)
                {
                    var tuple = subList[index];
                    if (tuple.Item3 == presenter)
                    {
                        if (subList.Remove(tuple))
                            break;
                    }
                }
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (CardList.All(subList => subList.Count == 0))
                ChangeStage();

            OpenLastCard();
        }

        public void KeepCard(CardAble card)
        {
            foreach (var subList in CardList)
            {
                for (var index = 0; index < subList.Count; index++)
                {
                    var tuple = subList[index];
                    if (tuple.Item2 == card)
                    {
                        if (subList.Remove(tuple))
                            break;
                    }
                }
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (CardList.All(subList => subList.Count == 0))
                ChangeStage();

            OpenLastCard();
        }

        private void ChangeStage()
        {
            PopupManager.Instance.ShowView<StageRewardView>();
        }
        
        /// 맨 앞줄의 리스트 반환. 해당 줄이 비어있거나, 몬스터가 아니면 null로 반환.
        public List<IStat> GetAllFirstEntity()
        {
            List<IStat> list = new List<IStat>();
            
            foreach (var variable in CardList)
            {
                if (variable.Count > 0 && variable[^1].Item1.ModelType == ModelType.Entity)
                    list.Add(variable[^1].Item1 as IStat);
                else
                    list.Add(null);
            }

            return list;
        }
    }
}