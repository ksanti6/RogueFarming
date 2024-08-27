using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    private PlayerActionScript _input;
    private Rigidbody2D _rigidbody;
    private Vector2 _moveValue;


    private void Awake()
    {
        _input = new PlayerActionScript();

        _rigidbody = GetComponent<Rigidbody2D>();

        if (!_rigidbody)
        {
            Debug.LogError("NULL RIGIDBODY");
        }
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void FixedUpdate()
    {
        _moveValue = _input.Player.Movement.ReadValue<Vector2>();
        _rigidbody.velocity = _moveValue * _speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
