using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Card.Status
{
    public enum StatusType
    {
        Ad,
        Ap,
        Def,
        Res,
        Luk,
    }
    
    public class StatusView : MonoBehaviour
    {
        [SerializeField] private StatusCell cellPrefab;
        
        private Queue<StatusCell> _cellPool = new Queue<StatusCell>();
        private Dictionary<StatusType, StatusCell> _activeCellDic = new();

        public void Init()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                StatusCell childCell = child.GetComponent<StatusCell>();
                
                if (childCell != null)
                {
                    _cellPool.Enqueue(childCell);
                    child.gameObject.SetActive(false);
                }
            }
        }

        public void SetStatus(StatusType statType, int value)
        {
            if (!_activeCellDic.ContainsKey(statType))
            {
                StatusCell cell = _cellPool.Dequeue();
                cell.gameObject.SetActive(true);
                _activeCellDic.Add(statType, cell);
            }
            
            _activeCellDic[statType].SetData(statType, value);
        }
    }
}