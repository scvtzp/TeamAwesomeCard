using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Card.Status
{
    public class StatusCell : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public void SetData(StatusType type ,int value)
        {
            SetText(value);
        }
        
        private void SetText(int value)
        {
            text.SetText(value.ToString());
        }
    }
}