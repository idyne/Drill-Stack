using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour
{
    [SerializeField] private float length = 20;

    [SerializeField] private LevelPartType type;

    public float Length { get => length; }

    public LevelPartType Type { get => type; }

    public enum LevelPartType
    {
        DRILL_BIT_5_1,
        DRILL_BIT_5_2,
        DRILL_BIT_5_3,
        DRILL_BIT_10_1,
        DRILL_BIT_10_2,
        DRILL_BIT_10_3,
        DRILL_BIT_15_1,
        DRILL_BIT_15_2,
        DRILL_BIT_15_3,
        ELEVATOR,
        GATE_1,
        GATE_2,
        GATE_3,
        GATE_4,
        GIYOTIN,
        MOVABLE_SAW_H_L,
        MOVABLE_SAW_H_M,
        MOVABLE_SAW_H_R,
        MOVABLE_SAW_V_L,
        MOVABLE_SAW_V_M,
        MOVABLE_SAW_V_R,
        RASP_L,
        RASP_M,
        RASP_R,
        SAW_H_L,
        SAW_H_M,
        SAW_H_R,
        SAW_V_L,
        SAW_V_M,
        SAW_V_R,
        UPGRADER_L,
        UPGRADER_M,
        UPGRADER_R,
        WEIGHT_L,
        WEIGHT_M,
        WEIGHT_R,
        WALL_0001,
        WALL_0010,
        WALL_0011,
        WALL_0100,
        WALL_0101,
        WALL_0110,
        WALL_0111,
        WALL_1000,
        WALL_1001,
        WALL_1010,
        WALL_1011,
        WALL_1100,
        WALL_1101,
        WALL_1110,
        WALL_1111,
    }
}