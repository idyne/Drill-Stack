using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private LevelPart.LevelPartType[] levelChain;
    [SerializeField] private LevelPart[] levelParts;
    [Header("Prefabs")]
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject buildingPrefab;

    

    public void GenerateLevel()
    {
        DestroyImmediate(GameObject.Find("Level Parts"));
        Transform container = (new GameObject("Level Parts")).transform;
        container.parent = GameObject.Find("Level").transform;
        float currentDistance = 15;
        float currentHeight = 0;
        List<Road.RoadGap> roadGaps = new List<Road.RoadGap>();
        for (int i = 0; i < levelChain.Length; i++)
        {
            if (levelChain[i] == LevelPart.LevelPartType.ELEVATOR)
            {
                /*
                roadGaps.Add(new Road.RoadGap(currentDistance));
                currentDistance += 25;
                currentHeight += 5;
                */
            }
            else
            {
                LevelPart levelPart = levelParts.Where((lp) => { return lp.Type == levelChain[i]; }).ToArray()[0];
                Instantiate(levelPart.gameObject, new Vector3(0, currentHeight, currentDistance), Quaternion.identity, container);
                currentDistance += levelPart.Length;
            }
        }
        GenerateRoad(roadGaps.ToArray(), currentDistance);
        GenerateEnvironment(currentDistance);
    }
    private void GenerateRoad(Road.RoadGap[] roadGaps, float roadLength)
    {
        roadLength = Mathf.CeilToInt(roadLength / 5) * 5;
        Transform container = GameObject.Find("Level").transform;
        DestroyImmediate(FindObjectOfType<Road>()?.gameObject);
        Road road = Instantiate(roadPrefab, Vector3.zero, roadPrefab.transform.rotation).GetComponent<Road>();
        road.GenerateRoad(roadLength, roadGaps);
        road.transform.parent = container;
    }

    private void GenerateEnvironment(float roadLength)
    {
        DestroyImmediate(GameObject.Find("Buildings"));
        Transform container = (new GameObject("Buildings")).transform;
        container.parent = GameObject.Find("Environment").transform;
        float currentDistance = 0;
        while (currentDistance < roadLength + 180)
        {
            float z = Random.Range(15, 25);
            float x = Random.Range(9, 20f);
            Instantiate(buildingPrefab, new Vector3(4 + x, 0, currentDistance + z), Quaternion.identity, container);
            currentDistance += z;
        }
        currentDistance = 0;
        while (currentDistance < roadLength + 180)
        {
            float z = Random.Range(15, 25);
            float x = Random.Range(9, 20f);
            Instantiate(buildingPrefab, new Vector3(-4 - x, 0, currentDistance + z), Quaternion.identity, container);
            currentDistance += z;
        }
    }

}
