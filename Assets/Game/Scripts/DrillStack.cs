using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames;
using TMPro;

public class DrillStack : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 90;
    [SerializeField] private TextMeshProUGUI powerText ;
    private List<DrillBit> list;
    private float angle = 0;
    private float _power = 0;

    public List<DrillBit> List { get => list; }

    private float Power
    {
        get
        {
            float result = 0;
            for (int i = 0; i < list.Count; i++)
            {
                result += list[i].CurrentPower;
            }
            return result;
        }
    }

    private void Awake()
    {
        list = new List<DrillBit>();
    }

    private void Update()
    {
        _power = Power;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.IN_GAME)
        {
            TurnDrill();
            AdjustDrillBitPositions();
        }
    }

    private void TurnDrill()
    {
        angle = (angle - Time.fixedDeltaTime * turnSpeed) % 360;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void AddDrillBit(DrillBitType type)
    {
        Vector3 position = list.Count > 0 ? list[list.Count - 1].transform.position : transform.position;
        position.z++;
        DrillBit drillBit = ObjectPooler.Instance.SpawnFromPool("Drill Bit", position, Quaternion.identity).GetComponent<DrillBit>();
        list.Add(drillBit);
        drillBit.SetType(type);
        WaveEffect();
        SetPowerText();
    }

    public DrillBit RemoveDrillBit(int index)
    {
        if (index >= 0 && index < list.Count)
        {
            DrillBit drillBit = list[index];
            RemoveDrillBit(drillBit);
            return drillBit;
        }
        return null;
    }

    public void RemoveDrillBit(DrillBit drillBit, bool effect = true)
    {
        if (effect)
        {
            string typeString = drillBit.Type.ToString();
            ObjectPooler.Instance.SpawnFromPool(typeString + " Drill Bit Break Effect", drillBit.transform.position, Quaternion.identity);
        }
        list.Remove(drillBit);
        drillBit.gameObject.SetActive(false);
        SetPowerText();
    }

    public void SetPowerText()
    {
        powerText.text = Mathf.CeilToInt(Power).ToString();
    }

    public void WaveEffect()
    {
        WaveEffect(list.Count);
    }

    public void WaveEffect(int index)
    {
        int size = list.Count;
        for (int i = 0; i < index; i++)
        {
            list[i].GetComponent<DrillBit>().Hop((size - i) * 0.05f);
        }
    }

    public void BreakDrillFrom(int index)
    {
        if (index < list.Count)
        {
            Vector3 position = new Vector3(0, list[index].transform.position.y - 1, list[index].transform.position.z + 10);
            while (list.Count > index)
            {
                DrillBit drillBit = RemoveDrillBit(index);
                CollectibleDrillBit collectibleDrillBit = ObjectPooler.Instance.SpawnFromPool("Collectible Drill Bit", drillBit.transform.position, Quaternion.identity).GetComponent<CollectibleDrillBit>();
                collectibleDrillBit.SetType(drillBit.Type);
                Vector3 targetPosition = position + new Vector3((MainLevelManager.Instance.Road.Width / 2 - 0.5f) * (Random.value * 2f - 1), 0, Random.Range(-4f, 4f));
                collectibleDrillBit.transform.LeanMoveX(targetPosition.x, 0.35f);
                collectibleDrillBit.transform.LeanMoveZ(targetPosition.z, 0.35f);
                float height = targetPosition.y;
                collectibleDrillBit.transform.LeanMoveY(height + 2, 0.17f).setEaseInSine().setOnComplete(() =>
                {
                    collectibleDrillBit.transform.LeanMoveY(height, 0.18f).setEaseOutBounce();
                });
            }
        }
    }

    private void AdjustDrillBitPositions()
    {
        if (list.Count > 0)
        {
            list[0].transform.position = transform.position;
            for (int i = 1; i < list.Count; i++)
            {
                Transform drillBit = list[i].transform;
                Transform previousDrillBit = list[i - 1].transform;
                Vector3 targetPosition = previousDrillBit.position;
                targetPosition.x = drillBit.position.x;
                targetPosition.z += 1;
                drillBit.position = targetPosition;
                targetPosition.x = previousDrillBit.position.x;
                drillBit.position = Vector3.Lerp(drillBit.position, targetPosition, Time.fixedDeltaTime * MainLevelManager.Instance.Player.Speed.x * 1.3f);
                targetPosition = drillBit.position;
                targetPosition.y = transform.position.y;
                drillBit.position = targetPosition;
            }
        }
    }
}
