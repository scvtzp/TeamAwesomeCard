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
        public List<List<(CardAble, ICardPresenter)>> cardList_1 { get; private set; }

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
            cardList_1 = new List<List<(CardAble, ICardPresenter)>>();
            for (int i = 0; i < 5; i++)
            {
                cardList_1.Add(new List<(CardAble, ICardPresenter)>());
            }
            
            // 라인별로 무작위 갯수만큼 카드 생성.
            for (int i = 0; i < cardList_1.Count; i++)
            {
                for (int j = 0; j < Random.Range(1, 5); j++)
                {
                    if(Random.Range(1, 4) != 2)
                    {
                        var model = new EntityModel(DataSettingManager.Instance.EntityData.GetRandomValue());
                        var view = Instantiate(entityViewPrefab, cardListParents[i]);
                        var presenter = new EntityPresenter(model, view);
                        
                        cardList_1[i].Add((view, presenter));
                    }
                    else
                    {
                        ItemModel model;
                        if (Random.Range(1, 3) != 2)
                        {
                            model = new ItemModel
                            {
                                id = "item_0"
                            };
                        }
                        else
                        {   //힐 생성.
                            model = new ItemModel(1)
                            {
                                id = "item_1"
                            }; 
                        }
                        
                        var view = Instantiate(ItemViewPrefab, cardListParents[i]);
                        var presenter = new ItemPresenter(model, view);
                        
                        cardList_1[i].Add((view, presenter));
                    }
                }
            }

            SortCardPos();
            CloseAllCard();
            OpenLastCard();
        }

        private void CloseAllCard()
        {
            foreach (var list in cardList_1)
                foreach (var card in list)
                    card.Item2.SetCardFace(false);
        }

        /// <summary>
        /// 카드 정렬.
        /// </summary>
        private void SortCardPos()
        {
            for (var index = 0; index < cardList_1.Count; index++)
            {
                for (int i = cardList_1[index].Count - 1; i >= 0; i--)
                {
                    var pos = (i - (cardList_1[index].Count - 1)) * -30f;
                    cardList_1[index][i].Item1.SetPos(new Vector3(0, pos, 0), cardListParents[index]);
                }
            }
        }
        
        // 특정 카드가 죽으면 아래 카드 뒤집어준다.
        private void OpenLastCard()
        {
            //맨 앞 카드 뒤집어줌
            for (int i = 0; i < cardList_1.Count; i++)
            {
                if(cardList_1[i].Count > 0)
                    cardList_1[i][^1].Item2.SetCardFace(true);
            }
        }

        public void DeathAction(ICardPresenter presenter)
        {
            presenter.Death();
            
            foreach (var subList in cardList_1)
            {
                for (var index = 0; index < subList.Count; index++)
                {
                    var tuple = subList[index];
                    if (tuple.Item2 == presenter)
                    {
                        if (subList.Remove(tuple))
                            break;
                    }
                }
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (cardList_1.All(subList => subList.Count == 0))
                ChangeStage();

            OpenLastCard();
        }

        public void KeepCard(CardAble card)
        {
            foreach (var subList in cardList_1)
            {
                for (var index = 0; index < subList.Count; index++)
                {
                    var tuple = subList[index];
                    if (tuple.Item1 == card)
                    {
                        if (subList.Remove(tuple))
                            break;
                    }
                }
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (cardList_1.All(subList => subList.Count == 0))
                ChangeStage();

            OpenLastCard();
        }

        private void ChangeStage()
        {
            PopupManager.Instance.ShowView<StageRewardView>();
        }
    }
}