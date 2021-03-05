﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class TowersDefault : MonoBehaviour
{
    private float _timer;
    [SerializeField] public LayerMaskVariable EnemyLayerMask;
    [SerializeField] public float ActivateEveryX;
    [SerializeField] public float TowerRadius;
    [SerializeField] public float TowerDamage;

    [SerializeField] protected GameObject floatingDmg;

    [SerializeField] protected ParticleSystem pulseFX;
    private void Start()
    {

    }
    private void OnEnable()
    {
        var emission = pulseFX.emission;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 1, 1000, ActivateEveryX) });
        var main = pulseFX.main;
        main.startLifetime = ActivateEveryX;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= ActivateEveryX)
        {
            MyCollisions();
            _timer = 0;
        }
    }

    protected virtual void MyCollisions()
    {
        Debug.Log("Select a specifc tower script");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TowerRadius);
    }
}
