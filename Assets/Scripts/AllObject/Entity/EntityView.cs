using Card.Status;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Manager;
using R3;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour, ICardAble
{
    public Button button;
    
    [SerializeField] private Image entityImage;
    [SerializeField] private HpBar hpBar;
    [SerializeField] private GameObject backObject;
    [SerializeField] private StatusView status;

    public void Init()
    {
        status?.Init();

        UpdateData();
    }
    
    private void UpdateData()
    {
        
    }

    public void SetCardFace(bool isFront = true)
    {
        if(backObject != null)
            backObject.SetActive(!isFront);
    }

    public void SetStatus(StatusType type, int value) => status?.SetStatus(type, value);
    
    public async UniTask SetHpBar(int hp, int maxHp)
    {
        if(hpBar != null) 
            await hpBar.SetHpBar(hp, maxHp);
    }
}
