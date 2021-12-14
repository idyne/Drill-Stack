using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrillSlot : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] private float power = 10;
    [SerializeField] private bool randomPower = false;
    [SerializeField] private float min = 10, max = 20;
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private DrillSlotTrigger drillSlotTrigger;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private BoxCollider obstacleCollider;
    private Animator anim;
    private float currentPower = 10;

    public float CurrentPower { get => currentPower; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        SetPower();
    }
    public void SetPower()
    {
        if (randomPower)
            power = Random.Range(min, max);
        power = Mathf.Ceil(power);
        currentPower = power;
        SetPowerText();
    }

    public bool GetPenetrated(float powerDiff)
    {
        if (currentPower <= 0) return false;
        currentPower -= powerDiff;
        drillSlotTrigger?.OnDrillSlotInteraction(1 - currentPower / power);
        if (currentPower <= 0)
        {
            TriggerTarget();
        }
        SetPowerText();
        return true;
    }
    private void TriggerTarget()
    {
        anim.SetTrigger("CLOSE");
        obstacleCollider.enabled = false;
        if (targetAnimator)
            targetAnimator.SetTrigger("OPEN");
    }

    private void SetPowerText()
    {
        powerText.text = Mathf.CeilToInt(power - currentPower).ToString() + "/" + Mathf.CeilToInt(power).ToString();
    }
}
