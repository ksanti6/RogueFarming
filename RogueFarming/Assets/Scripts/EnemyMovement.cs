using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform _target;
    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _maxDistance = 1f;

    public GameObject t;
    public GameObject container;

    private PathingAlgorithm pathing;
    private List<Vector3> _path;

    private Vector3 _movementVector;
    // Start is called before the first frame update
    void Start()
    {
        pathing = new PathingAlgorithm();
        _path = new List<Vector3>();

        GetNewPath();
    }

    // Update is called once per frame
    void Update()
    {

        if(_path.Count > 0)
        {
            if(Vector3.Distance(_target.position, _path[_path.Count - 1]) > _maxDistance)
            {
                GetNewPath();
            }
            else
            {
                MoveAlongPath();
            }

            
        }
        else
        {
            GetNewPath();
        }
    }

    void NukeChildren()
    {
        int children = container.transform.childCount;

        for(int k = children - 1; k >= 0; --k)
        {
            Destroy(container.transform.GetChild(k).gameObject);
        }
        //Debug.Log(container.transform.childCount);
    }


    void GetNewPath()
    {
        NukeChildren();


        _path.Clear();
        Node start, end;

        GridMap g = GridMap.GetInstance;

        g.m_gridMap.TryGetValue(g.GetGridFromWorld(_target.position), out end);
        g.m_gridMap.TryGetValue(g.GetGridFromWorld(transform.position), out start);

        _path = pathing.FindPath(start, end);

        _path.RemoveAt(0);

        foreach(Vector3 v in _path)
        {
            GameObject b = Instantiate(t, container.transform);
            b.transform.position = v;
        }
    }

    void MoveAlongPath()
    {
        float step = _speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position,_path[0], step);

        if(Vector2.Distance(transform.position, _path[0]) < 0.0001f)
        {
            _path.RemoveAt(0);
        }
    }

    private void FixedUpdate()
    {
        


        //_movementVector = (_target.position - transform.position).normalized * _speed;
        //transform.position += _movementVector * Time.deltaTime;
    }
}
