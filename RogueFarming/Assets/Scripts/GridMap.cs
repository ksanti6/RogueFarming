using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.FilePathAttribute;

public class GridMap : MonoBehaviour
{
    private static GridMap m_instance;

    public static GridMap GetInstance { get { return m_instance; } }

    [SerializeField]
    private Tilemap m_floor;
    [SerializeField]
    private Tilemap m_obstacles;


    //public GameObject t;

    public Dictionary<Vector3Int, Node> m_gridMap;

    private void Awake()
    {
        if(m_instance && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_gridMap = new Dictionary<Vector3Int, Node>();

        BoundsInt mapBounds = m_floor.cellBounds;


        //loop thru all the tiles
        for(int y = mapBounds.min.y; y < mapBounds.max.y; ++y)
        {
            for(int x = mapBounds.min.x; x < mapBounds.max.x; ++x)
            {
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                if(m_floor.HasTile(tileLocation))
                {
                    Node tile = new Node();
                    if(!m_obstacles.HasTile(tileLocation))
                    {
                        tile.m_NotWall = true;
                    }
                    else
                    {
                        tile.m_NotWall = false;
                    }

                    tile.m_gridPosition = tileLocation;
                    m_gridMap.Add(tileLocation, tile);
                }
            }
        }


        SetGridDirections();
    }

    public void ClearNodesData()
    {
        foreach (KeyValuePair<Vector3Int, Node> node in m_gridMap)
        {
            node.Value.m_finalCost = 0;
            node.Value.m_givenCost = 0;
            node.Value.m_onList = false;
            node.Value.m_parent = null;
        }
    }


    public Vector3Int GetGridFromWorld(Vector3 position)
    {
        return m_floor.WorldToCell(position);
    }

    public Vector3 GetWorldFromGrid(Vector3Int location)
    {
        return m_floor.GetCellCenterWorld(location);
    }


    void SetGridDirections()
    {
        foreach(KeyValuePair<Vector3Int, Node> node in m_gridMap)
        {
            Vector3Int location = node.Value.m_gridPosition;
            Node nodeAtLocation;

            //left
            --location.x;
            
            if(m_gridMap.TryGetValue(location, out nodeAtLocation))
            {
               if(nodeAtLocation.m_NotWall)
                {
                    node.Value.m_directions |= NodeDirections.Left;
                }
            }

            //right
            location.x += 2;
            if (m_gridMap.TryGetValue(location, out nodeAtLocation))
            {
                if (nodeAtLocation.m_NotWall)
                {
                    node.Value.m_directions |= NodeDirections.Right;
                }
            }

            //down
            --location.x;
            --location.y;
            if (m_gridMap.TryGetValue(location, out nodeAtLocation))
            {
                if (nodeAtLocation.m_NotWall)
                {
                    node.Value.m_directions |= NodeDirections.Down;

                    //could you go left?
                    if((node.Value.m_directions & NodeDirections.Left) == NodeDirections.Left)
                    {
                        //down left
                        --location.x;
                        if (m_gridMap.TryGetValue(location, out nodeAtLocation))
                        {
                            if (nodeAtLocation.m_NotWall)
                            {
                                node.Value.m_directions |= NodeDirections.DownLeft;
                            }
                        }
                        ++location.x;
                    }

                    //could you go right?
                    if ((node.Value.m_directions & NodeDirections.Right) == NodeDirections.Right)
                    {
                        //downright
                        ++location.x;
                        if (m_gridMap.TryGetValue(location, out nodeAtLocation))
                        {
                            if (nodeAtLocation.m_NotWall)
                            {
                                node.Value.m_directions |= NodeDirections.DownRight;
                            }
                        }
                        --location.x;
                    }
                }
            }//end of down

            //up
            location.y += 2;
            if (m_gridMap.TryGetValue(location, out nodeAtLocation))
            {
                if (nodeAtLocation.m_NotWall)
                {
                    node.Value.m_directions |= NodeDirections.Up;

                    //could you go left?
                    if ((node.Value.m_directions & NodeDirections.Left) == NodeDirections.Left)
                    {
                        //up left
                        --location.x;
                        if (m_gridMap.TryGetValue(location, out nodeAtLocation))
                        {
                            if (nodeAtLocation.m_NotWall)
                            {
                                node.Value.m_directions |= NodeDirections.UpLeft;
                            }
                        }
                        ++location.x;
                    }

                    //could you go right?
                    if ((node.Value.m_directions & NodeDirections.Right) == NodeDirections.Right)
                    {
                        //up right
                        ++location.x;
                        if (m_gridMap.TryGetValue(location, out nodeAtLocation))
                        {
                            if (nodeAtLocation.m_NotWall)
                            {
                                node.Value.m_directions |= NodeDirections.UpRight;
                            }
                        }
                        --location.x;
                    }
                }
            }//end of up
        }//end of for each
    }
}
