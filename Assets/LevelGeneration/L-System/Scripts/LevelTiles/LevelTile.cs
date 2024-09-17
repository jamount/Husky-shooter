using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    End,
    Straight,
    Corner,
    ThreeWay,
    FourWay
}

public class LevelTile : MonoBehaviour
{
    [SerializeField]
    private TileType tileType;


}
