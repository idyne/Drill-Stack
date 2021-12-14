using FateGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillBit : MonoBehaviour
{
    [SerializeField] private DrillBitObject[] drillBitObjects;
    private float power = 1;
    private float currentPower = 1;
    private DrillBitType type;
    private GameObject lastUsedUpgrader = null;
    public int ReversedIndex { get => MainLevelManager.Instance.Player.DrillStack.List.Count - 1 - MainLevelManager.Instance.Player.DrillStack.List.IndexOf(this); }
    public int Index { get => MainLevelManager.Instance.Player.DrillStack.List.IndexOf(this); }
    public float Power { get => power; }
    public DrillBitType Type { get => type; }
    public float CurrentPower { get => currentPower; }

    public void SetType(DrillBitType type)
    {

        this.type = type;
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
        SetPower();
    }
    private void SetPower()
    {
        switch (type)
        {
            case DrillBitType.PLASTIC:
                power = 1;
                break;
            case DrillBitType.SILVER:
                power = 2;
                break;
            case DrillBitType.GOLD:
                power = 3;
                break;
        }
        currentPower = power;
    }

    private void Upgrade()
    {
        //MainLevelManager.Instance.Player.DrillStack.WaveEffect();
        Hop(0);
        switch (type)
        {
            case DrillBitType.PLASTIC:
                SetType(DrillBitType.SILVER);
                break;
            case DrillBitType.SILVER:
                SetType(DrillBitType.GOLD);
                break;
        }
        MainLevelManager.Instance.Player.DrillStack.SetPowerText();
    }

    public void Hop(float delay)
    {
        StartCoroutine(HopCoroutine(delay));
    }

    private IEnumerator HopCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        LeanTween.cancel(gameObject);
        transform.LeanScale(new Vector3(2, 2, 1), 0.15f).setOnComplete(() =>
            {
                transform.LeanScale(Vector3.one, 0.15f);
            });
    }

    private void PenetrateWall(BonusWall wall)
    {
        float powerDiff = Mathf.Clamp(Time.deltaTime * MainLevelManager.Instance.Player.Speed.z + 1, 0, currentPower);
        powerDiff = Mathf.Clamp(powerDiff, 0, wall.CurrentPower);
        if (wall.GetPenetrated(powerDiff)) currentPower -= powerDiff;
        if (currentPower <= 0)
            MainLevelManager.Instance.Player.DrillStack.RemoveDrillBit(this);
    }

    private void PenetrateWall(Wall wall)
    {
        float powerDiff = Mathf.Clamp(Time.deltaTime * MainLevelManager.Instance.Player.Speed.z + 1, 0, currentPower);
        powerDiff = Mathf.Clamp(powerDiff, 0, wall.CurrentPower);
        print(powerDiff);
        if (wall.GetPenetrated(powerDiff)) currentPower -= powerDiff;
        if (currentPower <= 0)
            MainLevelManager.Instance.Player.DrillStack.RemoveDrillBit(this);
    }

    private void PenetrateLock(DrillSlot drillSlot)
    {
        float powerDiff = Mathf.Clamp(Time.deltaTime * MainLevelManager.Instance.Player.Speed.z + 1, 0, currentPower);
        powerDiff = Mathf.Clamp(powerDiff, 0, drillSlot.CurrentPower);
        print(powerDiff);
        if (drillSlot.GetPenetrated(powerDiff)) currentPower -= powerDiff;
        if (currentPower <= 0)
            MainLevelManager.Instance.Player.DrillStack.RemoveDrillBit(this, false);
    }

    private void Break()
    {
        MainLevelManager.Instance.Player.DrillStack.BreakDrillFrom(Index);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            ICollectible collectible = other.GetComponent<ICollectible>();
            collectible.GetCollected();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Wall wall = other.GetComponent<Wall>();
            if (ReversedIndex == 0)
                PenetrateWall(wall);
            else
            {
                Break();
            }
        }
        else if (other.CompareTag("Bonus Wall"))
        {
            BonusWall wall = other.GetComponent<BonusWall>();
            if (ReversedIndex == 0)
                PenetrateWall(wall);
            else
            {
                Break();
            }
        }
        else if (other.CompareTag("Obstacle"))
        {
            print(other.name);
            if (ReversedIndex == 0)
                MainLevelManager.Instance.Player.DrillStack.RemoveDrillBit(this);
            else
            {
                Break();
            }
        }
        else if (other.CompareTag("Upgrader"))
        {
            if (other.gameObject != lastUsedUpgrader)
            {
                Upgrade();
                lastUsedUpgrader = other.gameObject;
            }
        }
        else if (other.CompareTag("Lock"))
        {
            if (ReversedIndex == 0)
            {
                DrillSlot gate = other.GetComponentInParent<DrillSlot>();
                PenetrateLock(gate);
            }
            else
            {
                Break();
            }
        }
    }
}

public enum DrillBitType { PLASTIC, SILVER, GOLD }

[System.Serializable]
public class DrillBitObject
{
    [SerializeField] private DrillBitType type;
    [SerializeField] private GameObject gameObject;

    public GameObject GameObject { get => gameObject; }
    public DrillBitType Type { get => type; }
}