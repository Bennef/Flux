﻿using UnityEngine;

public class Player : MonoBehaviour
{
    float _angle = 0;
    int _xSpeed = 3;
    int _ySpeed = 3;
    Rigidbody2D _rb;
    Vector2 _pos;
    GameManager _gameManager;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        MovePLayer();
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            _rb.AddForce(new Vector2(0, _ySpeed));
        }
        else
        {
            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(new Vector2(0, -_ySpeed));
            }
            else
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
            }
        }
    }
    
    private void MovePLayer()
    {
        _pos = transform.position;
        _pos.x = Mathf.Cos(_angle) * 2;
        transform.position = _pos;
        _angle += Time.deltaTime * _xSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dead();
    }

    private void Dead()
    {
        _gameManager.GameOver();
    }
}