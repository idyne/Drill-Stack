using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDrillBit : MonoBehaviour, ICollectible
{
    [SerializeField] private DrillBitObject[] drillBitObjects;
    [SerializeField] private DrillBitType type = DrillBitType.PLASTIC;
    private BoxCollider boxCollider = null;

    public DrillBitType Type { get => type; }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        SetType();
    }
    public void GetCollected()
    {
        MainLevelManager.Instance.Player.DrillStack.AddDrillBit(type);
        boxCollider.enabled = false;
        gameObject.SetActive(false);
    }
    public void SetType(DrillBitType type)
    {
        this.type = type;
        SetType();
    }
    public void SetType()
    {
        switch (type)
        {
            case DrillBitType.PLASTIC:
                for (int i = 0; i < drillBitObjects.Length; i++)
                {
                    if (drillBitObjects[i].Type == DrillBitType.PLASTIC) drillBitObjects[i].GameObject.SetActive(true);
                    else drillBitObjects[i].GameObject.SetActive(false);
                }
                break;
            case DrillBitType.SILVER:
                for (int i = 0; i < drillBitObjects.Length; i++)
                {
                    if (drillBitObjects[i].Type == DrillBitType.SILVER) drillBitObjects[i].GameObject.SetActive(true);
                    else drillBitObjects[i].GameObject.SetActive(false);
                }
                break;
            case DrillBitType.GOLD:
                for (int i = 0; i < drillBitObjects.Length; i++)
                {
                    if (drillBitObjects[i].Type == DrillBitType.GOLD) drillBitObjects[i].GameObject.SetActive(true);
                    else drillBitObjects[i].GameObject.SetActive(false);
                }
                break;
        }
    }
}
