using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AllObject
{
    /// <summary>
    /// 카드라면 가능한 기본적인 이동사항들.
    /// </summary>
    public class CardAble : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private GameObject backObject;
        [SerializeField] private Image selectImage;
        [SerializeField] private Collider2D collider;
        
        protected bool IsFront = false;
        protected bool IsDrag = false;
        
        public event Action OnClicked;
        
        private CardAble _targetCard;
        private Collider2D _targetCollider;
        
        private Vector3 _startPosition;
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
            selectImage.gameObject.SetActive(isSelected);
        }

        public void SetPos(Vector3 position)
        {
            _startPosition = position;
            transform.localPosition = position;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsFront && IsDrag)
            {
                if (other.CompareTag("Card"))
                {
                    //이미 선택된 카드 있다면 그 카드의 셀렉은 품.
                    _targetCard?.SetSelect(false);
                    
                    _targetCollider = other;
                    _targetCard = other.GetComponent<CardAble>();
                    _targetCard.SetSelect(true);
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
            _targetCard?.SetSelect(false);
            _targetCollider = null;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (IsFront)
            {
                IsDrag = true;
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
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsDrag)
            {
                IsDrag = false;
                _isCanClick = false;
                
                transform.localPosition = _startPosition;
            }
            else
                _isCanClick = true;
            
            ClearTarget();
        }
    }
}