﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] FloatVariable _bulletLife;
    [SerializeField] FloatVariable _bulletDamage;
    [SerializeField] FloatVariable _bulletSpeed;
    [SerializeField] FloatVariable _critChance;
    [SerializeField] FloatVariable _critDamage;

    [SerializeField] GameObject floatingDmg;

    [SerializeField] Color _critDamageColor;
    [SerializeField] Color _normDmgColor;


    private void Awake()
    {

    }

    private void OnEnable()
    {
        Invoke("SetUnActive", _bulletLife.RuntimeValue);
    }

    void Update()
    {
        //Move bullet forward
        transform.Translate(Vector3.forward * _bulletSpeed.RuntimeValue * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            float randValue = Random.value;
            if (randValue < _critChance.RuntimeValue)
            {
                other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(_bulletDamage.RuntimeValue * _critDamage.RuntimeValue);
                FloatingTxt(_bulletDamage.RuntimeValue * _critDamage.RuntimeValue, other.transform,_critDamageColor);
            }
            else
            {
                other.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(_bulletDamage.RuntimeValue);
                FloatingTxt(_bulletDamage.RuntimeValue, other.transform,_normDmgColor);
            }
            SetUnActive();
        }

        if (other.gameObject.tag == "Wall")
        {
            SetUnActive();
        }
    }

    public void FloatingTxt(float damage, Transform transformToSpawnTxtAt,Color color)
    {
        GameObject points = Instantiate(floatingDmg, transformToSpawnTxtAt.position, Quaternion.identity);
        TextMeshPro textmeshPro = points.transform.GetChild(0).GetComponent<TextMeshPro>();
        textmeshPro.text = "-" + damage.ToString();
        textmeshPro.color = color;
    }


    void SetUnActive()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }
}
