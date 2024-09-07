using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PathingAlgorithm
{

    
    public List<Vector3> FindPath(Node _start, Node _end)
    {
        List<Node> openList = new List<Node>
        {
            _start
        };

        List<Vector3> finalPath = new List<Vector3>();
        GridMap g = GridMap.GetInstance;

        g.ClearNodesData();

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(x => x.m_finalCost).First();

            openList.Remove(currentNode);

            //is this the goal?
            if(currentNode == _end)
            {
                //finalize our path

                Node n = currentNode;

                while (true)
                {
                    finalPath.Add(g.GetWorldFromGrid(n.m_gridPosition));

                    if(n.m_parent == null)
                    {
                        break;
                    }

                    n = n.m_parent;
                }

                finalPath.Reverse();
                break;

            }

            //no

            //get the children in all directions and add to open list
            for (int dir = (int)NodeDirections.Up; dir <= (int)NodeDirections.DownLeft; dir *= 2)
            {
                if((currentNode.m_directions & (NodeDirections)dir) == (NodeDirections)dir)
                {
                    Node n = FindChild(currentNode, (NodeDirections)dir, _end);

                    if(n != null)
                    {
                        openList.Add(n);
                    }
                }
            }
        }//end of while

        return finalPath;
    }



    Node FindChild(Node current, NodeDirections dir, Node _end)
    {
        Node child;
        Vector3Int childPosition = current.m_gridPosition;


        float givenCost = 1;

        switch (dir)
        {
            case NodeDirections.Up:
                {
                    ++childPosition.y;
                    break;
                }
            case NodeDirections.Left:
                {
                    --childPosition.x;
                    break;
                }
            case NodeDirections.Right:
                {
                    ++childPosition.x;
                    break;
                }
            case NodeDirections.Down:
                {
                    --childPosition.y;
                    break;
                }
            case NodeDirections.UpRight:
                {
                    ++childPosition.y;
                    ++childPosition.x;
                    givenCost = 1.4f;
                    break;
                }
            case NodeDirections.UpLeft:
                {
                    ++childPosition.y;
                    --childPosition.x;
                    givenCost = 1.4f;
                    break;
                }
            case NodeDirections.DownLeft:
                {
                    --childPosition.y;
                    --childPosition.x;
                    givenCost = 1.4f;
                    break;
                }
            case NodeDirections.DownRight:
                {
                    --childPosition.y;
                    ++childPosition.x;
                    givenCost = 1.4f;
                    break;
                }
        }

        givenCost += current.m_givenCost;

        GridMap.GetInstance.m_gridMap.TryGetValue(childPosition, out child);

        if(child == current.m_parent)
        {
            return null;
        }

        float cost = CalculateCost(child, _end);

        cost += givenCost;

        if(!child.m_onList || cost < child.m_finalCost)
        {
            child.m_finalCost = cost;
            child.m_givenCost = givenCost;
            child.m_parent = current;
            child.m_onList = true;

            return child;
        }



        return null;
    }



    float CalculateCost(Node node, Node _end)
    {
        int xDiff = Mathf.Abs(_end.m_gridPosition.x - node.m_gridPosition.x);
        int yDiff = Mathf.Abs(_end.m_gridPosition.y - node.m_gridPosition.y);

        int min = Mathf.Min(xDiff, yDiff);
        int max = Mathf.Max(xDiff, yDiff);

        return min * 1.41421356f + max - min;
    }
    
}

