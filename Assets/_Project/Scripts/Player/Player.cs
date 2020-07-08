﻿using Scripts.Cameras;
using Scripts.Core;
using Scripts.Inputs;
using Scripts.UI;
using UnityEngine;

namespace Scripts.Actors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] GameObject _deathParticles, _itemEffect;
        [SerializeField] float _maxVelocity;

        float _angle = 0;
        int _xSpeed = 3;
        int _ySpeed = 2;
        Vector2 _pos;

        Rigidbody2D _rb;
        GameManager _gameManager;
        InputHandler _inputHandler;
        BackgroundColor _backgroundColor;
        CameraShake _cameraShake;

        SpriteRenderer _spriteRenderer;

        public bool IsDead { get; set; }

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _gameManager = FindObjectOfType<GameManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _backgroundColor = FindObjectOfType<BackgroundColor>();
            _cameraShake = FindObjectOfType<CameraShake>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (!IsDead)
            {
                MovePLayer();
                GetInput();
            }
        }

        void GetInput()
        {
            if (_inputHandler.GetForwardButton() || _inputHandler.GetTouch())
            {
                _rb.AddForce(new Vector2(0, _ySpeed));
                CapVelocity();
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

        void CapVelocity() 
        {
            if (_rb.velocity.magnitude > _maxVelocity) 
            {
                _rb.AddForce(new Vector2(0, -_ySpeed));
            }
        }

        void MovePLayer()
        {
            _pos = transform.position;
            _pos.x = Mathf.Cos(_angle) * 2;
            transform.position = _pos;
            _angle += Time.deltaTime * _xSpeed;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                Dead();
            }
            else if (other.gameObject.CompareTag("Item"))
            {
                GetItem(other);
            }
        }

        void GetItem(Collider2D other)
        {
            _backgroundColor.SetBackgroundColor();
            Destroy(Instantiate(_itemEffect, other.transform.position, Quaternion.identity), 0.5f);
            Destroy(other.gameObject.transform.parent.gameObject);
            _gameManager.AddScore();
        }

        void Dead()
        {
            IsDead = true;
            _cameraShake.CallShake(); 
            Destroy(Instantiate(_deathParticles, transform.position, Quaternion.identity), 0.5f);
            _spriteRenderer.enabled = false;
            _rb.velocity = new Vector2(0, 0);
            _rb.isKinematic = true;
            _gameManager.CallGameOver();
        }
    }
}