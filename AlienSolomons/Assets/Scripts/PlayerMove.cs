﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] VirtualJoystick _moveJoystick;

    public bool _shooting;

    private Vector3 _moveInput;
    Vector3 _playerDirection;

    private Vector3 _moveVelocity;
    private Rigidbody _rb;

    Animator _anim;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _moveInput = _moveJoystick.InputDirection;
        _moveVelocity = _moveInput * _moveSpeed;

        Vector3 leftStickRot = _moveJoystick.InputDirection;
        _playerDirection = Vector3.right * leftStickRot.x + Vector3.forward * leftStickRot.z;
        // returns 1 if any rotation is being inputed from right joystick
        if (_playerDirection.sqrMagnitude > 0.0f)
            transform.rotation = Quaternion.LookRotation(_playerDirection * Time.deltaTime, Vector3.up);


        //Vector3 rightStickRot = _shootJoystick.InputDirection;
        //_playerDirection = Vector3.right * rightStickRot.x + Vector3.forward * rightStickRot.z;
        //// returns 1 if any rotation is being inputed from right joystick
        //if (_playerDirection.sqrMagnitude > 0.0f)
        //    transform.rotation = Quaternion.LookRotation(_playerDirection * Time.deltaTime, Vector3.up);

        if (_moveJoystick.InputDirection != Vector3.zero)
        {
            _anim.SetBool("IsRunning", true);
            _shooting = false;
        }
        else
        {
            _anim.SetBool("IsRunning", false);
            _shooting = true;
            transform.LookAt(GetClosestEnemy(GameStats.instance._enemies));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Currency")
        {
            Debug.Log("CURRENCY");
            GameStats.instance._currency += 1;
            other.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveVelocity;
    }

    public Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}

