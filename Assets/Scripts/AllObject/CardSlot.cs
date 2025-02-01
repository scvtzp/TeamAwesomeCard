using System;
using AllObject.Item;
using UnityEngine;
using UnityEngine.UI;

namespace AllObject
{
    public class CardSlot : MonoBehaviour
    {
        [SerializeField] private Image selectImage;
        
        public void SetSelect(bool isSelect)
        {
            selectImage.gameObject.SetActive(isSelect);
        }
    }
}