using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : DrillSlotTrigger
{
    [SerializeField] private GameObject container;
    private Vector3 targetContainerPosition = Vector3.zero;

    private void Awake()
    {
        targetContainerPosition = container.transform.localPosition;
    }

    private void Update()
    {
        container.transform.localPosition = Vector3.MoveTowards(container.transform.localPosition, targetContainerPosition, Time.deltaTime * 6);
    }
    public override void OnDrillSlotInteraction(float value)
    {
        Vector3 position = container.transform.localPosition;
        if (value < 1)
            position.y = value * 1.4f;
        else
            position.y = 40;
        targetContainerPosition = position;
    }
}