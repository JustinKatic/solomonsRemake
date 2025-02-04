﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
    private float _currentHealth;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject floatingDmg;
    [SerializeField] float MyMaxHealth;
    [SerializeField] FloatVariable numberOfActiveEnemies;
    [SerializeField] GameEvent ExperienceIncreasedEvent;
    [SerializeField] bool IHaveAHealthBar;

    [SerializeField] GameObject currencyObject;
    [SerializeField] GameObject currencySpawn;

    [HideInInspector] public bool plagueDebuff;
    private float plagueDurationTimer;

    float plagueDebuffDuration;
    float plagueTickDamage;
    float plagueTickRate;
    private float plagueTickTimer;


    [SerializeField] GameObject deathParticle;
    [SerializeField] Animator damageFXAnim;
    [SerializeField] GameObject model;
    private new SkinnedMeshRenderer renderer;
    private Material newMat;

    [SerializeField] GameObject plagueParticle;



    private void Start()
    {
        renderer = model.GetComponent<SkinnedMeshRenderer>();
        newMat = new Material(renderer.material);
        renderer.material = newMat;
    }

    private void OnEnable()
    {
        _currentHealth = MyMaxHealth;
        if (IHaveAHealthBar)
            healthBar.SetMaxHealth(MyMaxHealth);
    }
    private void OnDisable()
    {
        plagueDebuff = false;
    }

    private void Update()
    {
        PlagueDebuff();
        if (_currentHealth <= 0)
            Death();
    }

    public void HurtEnemy(float damage)
    {
        //damageFXAnim.Play("EnemyScaleOnHitFX");
        StartCoroutine(OnHurtEffect());
        _currentHealth -= damage;
        if (IHaveAHealthBar)
            healthBar.SetHealth(_currentHealth);
    }

    public void Death()
    {
        numberOfActiveEnemies.RuntimeValue -= 1;
        InstantiateCurrency(currencyObject);

        SFXAudioManager.instance.Play("EnemyDeath");


        InstantiateDeathParticle(deathParticle);

        gameObject.SetActive(false);
    }

    public void InstantiateCurrency(GameObject CurrencyObj)
    {
        if (CurrencyObj)
            Instantiate(CurrencyObj, currencySpawn.transform.position, currencySpawn.transform.rotation);
        else
            Debug.Log("no currency Obj added" + gameObject.name);
    }



    public void InstantiateDeathParticle(GameObject deathParticle)
    {
        if (deathParticle)
            Instantiate(deathParticle, currencySpawn.transform.position, transform.rotation);
        else
            Debug.Log("no deathParticle Obj added" + gameObject.name);
    }

    public void PlagueDebuff()
    {
        if (plagueDebuff == true)
        {
            plagueParticle.SetActive(true);
            plagueTickTimer += Time.deltaTime;
            plagueDurationTimer += Time.deltaTime;


            if (plagueTickTimer > plagueTickRate)
            {
                HurtEnemy(plagueTickDamage);
                FloatingTxt(plagueTickDamage, transform, "-", Color.white);
                plagueTickTimer = 0;
            }

            if (plagueDurationTimer > plagueDebuffDuration)
            {
                plagueDurationTimer = 0;
                plagueDebuff = false;
                plagueParticle.SetActive(false);
            }
        }
    }

    public void SetPlagueDebuffTrue(float tickDmg, float tickrate, float duration)
    {
        plagueDebuff = true;
        plagueDurationTimer = 0;
        plagueTickDamage = tickDmg;
        plagueTickRate = tickrate;
        plagueDebuffDuration = duration;
    }

    private void OnTriggerStay(Collider other)
    {
        if (plagueDebuff && other.gameObject.layer == 10)
            other.gameObject.GetComponent<EnemyHealthManager>().SetPlagueDebuffTrue(plagueTickDamage, plagueTickRate, plagueDebuffDuration);

    }
    public void FloatingTxt(float damage, Transform transformToSpawnTxtAt, string type, Color32 color)
    {
        GameObject points = ObjectPooler.SharedInstance.GetPooledObject("FloatingTxt");
        points.transform.position = transformToSpawnTxtAt.position;
        points.transform.rotation = Quaternion.identity;
        TextMeshPro txt = points.transform.GetChild(0).GetComponent<TextMeshPro>();
        txt.text = type + damage.ToString();
        txt.color = color;
        points.SetActive(true);
    }
    IEnumerator OnHurtEffect()
    {
        renderer.material.EnableKeyword("_EMISSION");
        yield return new WaitForSeconds(0.1f);
        renderer.material.DisableKeyword("_EMISSION");
    }
}
