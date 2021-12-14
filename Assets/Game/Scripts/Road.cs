using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private float length = 10;
    [SerializeField] private float width = 8;
    [SerializeField] private RoadGap[] gaps;
    [SerializeField] private GameObject roadPartPrefab;
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private GameObject elevatorPrefab;

    public float Length { get => length; }
    public float Width { get => width; }

    public void GenerateRoad(float length)
    {
        GenerateRoad(length, width);
    }
    public void GenerateRoad(float length, RoadGap[] gaps)
    {
        this.gaps = gaps;
        GenerateRoad(length);
    }
    public void GenerateRoad(float length, float width)
    {
        this.length = length;
        this.width = width;
        GenerateRoad();
    }
    public void GenerateRoad()
    {
        float currentHeight = 0;
        for (int i = 0; i <= gaps.Length; i++)
        {
            Vector3 position = Vector3.zero;
            if (i > 0)
            {
                RoadGap gap = gaps[i - 1];
                Instantiate(elevatorPrefab, new Vector3(0, currentHeight, gap.Position), elevatorPrefab.transform.rotation, transform);
                currentHeight += 5;
                position.z = gap.Position + 20;
                position.y = currentHeight;
            }
            float roadPartLength = gaps.Length > i ? gaps[i].Position - position.z : length - position.z;
            for (int j = 0; j < roadPartLength / 5; j++)
            {
                Transform roadPart = Instantiate(roadPartPrefab, position + j * Vector3.forward * 5, roadPartPrefab.transform.rotation, transform).transform;
                Vector3 localScale = roadPart.localScale;
                localScale.x = width;
                roadPart.localScale = localScale;
            }
        }
        Instantiate(bonusPrefab, new Vector3(0, currentHeight, length), Quaternion.identity, transform);
    }
    [System.Serializable]
    public class RoadGap
    {
        [SerializeField] private float position = 0;
        public float Position { get => position; }

        public RoadGap(float position)
        {
            this.position = position;
        }
    }
}
