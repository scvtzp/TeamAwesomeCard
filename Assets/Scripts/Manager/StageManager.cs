using System.Collections.Generic;
using System.Linq;
using AllObject;
using AllObject.Entity;
using AllObject.Item;
using DefaultNamespace;
using Manager.Generics;
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

        public List<List<ICardPresenter>> cardPresenterList { get; private set; }
        public List<List<CardAble>> cardList { get; private set; }

        private void Start()
        {
            SetStage();
        }

        public void SetStage()
        {
            //todo: 스테이지 데이터 임시로 넣음.

            //테스트용 초기화 코드.
            for (int i = 0; i < cardListParents.Count; i++)
            {
                for (int j = 0; j < cardListParents[i].childCount; j++)
                    Destroy(cardListParents[i].GetChild(j).gameObject);
            }
            
            // 초기화
            cardPresenterList = new List<List<ICardPresenter>>();
            cardList = new List<List<CardAble>>();
            for (int i = 0; i < 5; i++)
            {
                cardPresenterList.Add(new List<ICardPresenter>());
                cardList.Add(new List<CardAble>());
            }
            
            // 라인별로 무작위 갯수만큼 카드 생성.
            for (int i = 0; i < cardPresenterList.Count; i++)
            {
                for (int j = 0; j < Random.Range(1, 5); j++)
                {
                    if(Random.Range(1, 3) != 2)
                    {
                        var model = new EntityModel();
                        model.maxHp.Value = Random.Range(1, 16);
                        model.hp.Value = model.maxHp.Value;
                        model.atk.Value = Random.Range(3, 6);
                        model.def.Value = Random.Range(0, 3);
                        
                        var view = Instantiate(entityViewPrefab, cardListParents[i]);
                        var presenter = new EntityPresenter(model, view);
                        
                        cardList[i].Add(view);
                        cardPresenterList[i].Add(presenter);
                    }
                    else
                    {
                        var model = new ItemModel();
                        var view = Instantiate(ItemViewPrefab, cardListParents[i]);
                        var presenter = new ItemPresenter(model, view);
                        
                        cardList[i].Add(view);
                        cardPresenterList[i].Add(presenter);
                    }
                }
            }

            SortCardPos();
            CloseAllCard();
            OpenLastCard();
        }

        private void CloseAllCard()
        {
            foreach (var list in cardPresenterList)
                foreach (var card in list)
                    card.SetCardFace(false);
        }

        /// <summary>
        /// 카드 정렬.
        /// </summary>
        private void SortCardPos()
        {
            for (var index = 0; index < cardList.Count; index++)
            {
                for (int i = cardList[index].Count - 1; i >= 0; i--)
                {
                    var pos = (i - (cardList[index].Count - 1)) * -30f;
                    cardList[index][i].SetPos(new Vector3(0, pos, 0), cardListParents[index]);
                }
            }
        }
        
        // 특정 카드가 죽으면 아래 카드 뒤집어준다.
        private void OpenLastCard()
        {
            //맨 앞 카드 뒤집어줌
            for (int i = 0; i < cardPresenterList.Count; i++)
            {
                if(cardPresenterList[i].Count > 0)
                    cardPresenterList[i][^1].SetCardFace(true);
            }
        }

        public void DeathAction(ICardPresenter presenter)
        {
            presenter.Death();
            
            foreach (var subList in cardPresenterList)
            {
                if (subList.Remove(presenter))
                    break;
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (cardPresenterList.All(subList => subList.Count == 0))
                SetStage();

            OpenLastCard();
        }
    }
}