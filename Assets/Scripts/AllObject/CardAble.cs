using System;
using System.Collections.Generic;
using DefaultNamespace;
using Manager;
using SkillSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AllObject
{
    public enum CardType
    {
        Monster,
        Skill,
        Item,
    }
    
    /// <summary>
    /// 카드라면 가능한 기본적인 이동사항들.
    /// </summary>
    public abstract class CardAble : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private GameObject backObject;
        [SerializeField] private Image selectImage;
        [SerializeField] private Collider2D collider;
        
        protected CardType CardType = CardType.Monster;
        protected CardAble TargetCard;
        protected bool IsFront = false;
        protected bool IsDrag = false;
        
        public event Action OnClicked;
        
        private Collider2D _targetCollider;
        
        private Vector3 _startPosition;
        private Transform _parentTransform;
        private bool _isCanClick = false;
        
        
        public virtual void SetCardFace(bool isFront = true)
        {
            if(backObject != null)
                backObject.SetActive(!isFront);
            
            if(collider != null)
                collider.enabled = isFront;
            
            IsFront = isFront;
        }

        public virtual void SetSelect(bool isSelected = true)
        {
            selectImage?.gameObject.SetActive(isSelected);
        }

        public void SetPos(Vector3 position, Transform parent)
        {
            _startPosition = position;
            transform.localPosition = position;
            _parentTransform = parent;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsFront && IsDrag)
            {
                if (other.CompareTag("Card"))
                {
                    //이미 선택된 카드 있다면 그 카드의 셀렉은 품.
                    TargetCard?.SetSelect(false);
                    
                    _targetCollider = other;
                    TargetCard = other.GetComponent<CardAble>();
                    TargetCard.SetSelect(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsFront && IsDrag)
            {
                if (other == _targetCollider)
                {
                    ClearTarget();
                }
            }
        }

        private void ClearTarget()
        {
            TargetCard?.SetSelect(false);
            _targetCollider = null;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (IsFront)
            {
                IsDrag = true;
                //캔버스 상 타 카드보다 상단 올 수 있도록 부모 수정.
                gameObject.transform.SetParent(StageManager.Instance.dragParents); 
                transform.position = Input.mousePosition;
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if(_isCanClick)
                OnClicked?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (IsDrag)
            {
                IsDrag = false;
                _isCanClick = false;
                
                gameObject.transform.SetParent(_parentTransform);
                transform.localPosition = _startPosition;
            }
            else
                _isCanClick = true;
            
            ClearTarget();
        }

        public virtual bool UsedSkill(List<Skill> skillList)
        {
            Debug.Log("스킬이 사용되지 않았습니다 " + gameObject.name);
            return false;
        }

        public abstract void UpdateData(string id);
    }
}