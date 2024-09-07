using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum NodeDirections : byte
{
    Up = 1,
    Right = 1 << 1,
    Down = 1 << 2,
    Left = 1 << 3,
    UpRight = 1 << 4,
    UpLeft = 1 << 5,
    DownRight = 1 << 6,
    DownLeft = 1 << 7
}

public class Node
{
    public Node m_parent;

    public Vector3Int m_gridPosition;

    public float m_finalCost;
    public float m_givenCost;
    public bool m_NotWall;
    public bool m_onList;

    public NodeDirections m_directions;

}
