using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : DrillSlotTrigger
{
    [SerializeField] private GameObject container;
    private Vector3 targetContainerPosition = Vector3.zero;
    private State state = State.IDLE;


    private void Update()
    {
        if (state == State.STARTED && Vector3.Distance(container.transform.localPosition, targetContainerPosition) <= 0.01f)
        {
            state = State.FINISHED;
            MainLevelManager.Instance.Player.Continue();
        }
        container.transform.localPosition = Vector3.MoveTowards(container.transform.localPosition, targetContainerPosition, Time.deltaTime * 6);
    }
    public override void OnDrillSlotInteraction(float value)
    {
        if (state == State.IDLE)
            state = State.STARTED;
        Vector3 position = container.transform.localPosition;
        position.y = value * 5;
        targetContainerPosition = position;
        if (value >= 1)
        {


            MainLevelManager.Instance.Player.Wait();
        }
    }
    private enum State { IDLE, STARTED, FINISHED }
}
