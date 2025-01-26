using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] EntityView entityViewPrefab;
        [SerializeField] List<Transform> cardListParents;
        
        private List<List<IBattleAble>> _cardList = new List<List<IBattleAble>>();

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
            _cardList = new List<List<IBattleAble>>();
            for (int i = 0; i < 5; i++)
                _cardList.Add(new List<IBattleAble>());
            
            // 라인별로 무작위 갯수만큼 카드 생성.
            for (int i = 0; i < _cardList.Count; i++)
            {
                for (int j = 0; j < Random.Range(1, 5); j++)
                {
                    var model = new EntityModel();
                    model.maxHp.Value = Random.Range(1, 16);
                    model.hp.Value = model.maxHp.Value;
                    model.atk.Value = Random.Range(3, 6);
                    model.def.Value = Random.Range(0, 3);
                    
                    var view = Instantiate(entityViewPrefab, cardListParents[i]);
                    
                    var presenter = new EntityPresenter(model, view);
                    
                    _cardList[i].Add(presenter);
                }
            }

            CloseAllCard();
            OpenLastCard();
        }

        private void CloseAllCard()
        {
            foreach (var list in _cardList)
                foreach (var card in list)
                    card.SetCardFace(false);
        }
        
        // 특정 카드가 죽으면 아래 카드 뒤집어준다.
        private void OpenLastCard()
        {
            //맨 앞 카드 뒤집어줌
            for (int i = 0; i < _cardList.Count; i++)
                _cardList[i][^1].SetCardFace(true);
        }

        public void DeathAction(IBattleAble presenter)
        {
            foreach (var subList in _cardList)
            {
                if (subList.Remove(presenter))
                    break;
            }

            //테스트용. 원래는 이동 카드 먹어야 이동함.
            if (_cardList.All(subList => subList.Count == 0))
                SetStage();

            OpenLastCard();
        }
    }
}