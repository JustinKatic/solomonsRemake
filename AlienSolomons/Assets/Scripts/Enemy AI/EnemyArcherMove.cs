﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyArcherMove : MonoBehaviour
{
    Transform destination;
    public NavMeshAgent navMeshAgent;
    GameObject player;

    [SerializeField] float _range;
    public bool _shooting;

    private bool _slowDebuff;
    private float _slowDurationTimer;


    void Start()
    {
        destination = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError(gameObject.name + "does not have a nav mesh agent.");
        }
    }


    private void OnDisable()
    {
        _slowDebuff = false;
        SetEnemyMoveSpeed(EnemyManager.instance._archerDefaultMoveSpeed);
    }

    private void Update()
    {
        SlowDebuff(EnemyManager.instance._archerDefaultMoveSpeed);

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > _range)
        {
            navMeshAgent.isStopped = false;
            _shooting = false;
            navMeshAgent.SetDestination(destination.position);
        }
        else
        {
            navMeshAgent.isStopped = true;
            _shooting = true;
            transform.LookAt(player.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            navMeshAgent.isStopped = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            navMeshAgent.isStopped = false;
    }

    public void SetEnemyMoveSpeed(float enemyMoveSpeed)
    {
        if (navMeshAgent != null)
            navMeshAgent.speed = enemyMoveSpeed;
    }

    public float GetEnemyMoveSpeed()
    {
        return navMeshAgent.speed;
    }

    public void SetSlowDebuffTrue(float reduceSpeedByX)
    {
        SetEnemyMoveSpeed(EnemyManager.instance._archerDefaultMoveSpeed - reduceSpeedByX);
        _slowDebuff = true;
        _slowDurationTimer = 0;
    }

    public void SlowDebuff(float EnemyDefaultSpeed)
    {
        if (_slowDebuff == true)
        {
            _slowDurationTimer += Time.deltaTime;
            if (_slowDurationTimer > TowerManager.instance._slowedDuration)
            {
                SetEnemyMoveSpeed(EnemyDefaultSpeed);
                _slowDurationTimer = 0;
                _slowDebuff = false;
            }
        }
    }
}