﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Zombie行動ルーチン
  *製作者　篠﨑*/
public class ZombieController : EnemyBase
{
    private float m_attckCount = 0;       //攻撃カウント
    private float m_attckTime     = 0.5f;    //攻撃速度

    Animator anim;
    private string EnemyDeathEffect = "EnemyDeathEffect";

    void Update()
    {
        if (Mathf.Approximately(Time.timeScale, 0.0f))
            return;

        if (state != "Caught" && state != "Die")
        {
            if (state == "Sleep")
            {
                m_currentTime += Time.deltaTime;
                if (m_stopTime < m_currentTime)
                {
                    m_currentTime = 0;
                    state = "RandamMove";
                }
            }
            else
            {
                ActionPolicy();
            }
        }
        if (state == "Die")
        {
            anim.Play("Zombie_Tangled");
            EffectManager.instance.playInPlace(transform.position, EnemyDeathEffect);
        }
        StateCheck();
    }
    /// <summary>
    /// 行動方針を決定する
    /// </summary>
    private void ActionPolicy()
    {
        //距離測定関数
        DistancePlayer();
        //追跡関数
        PlayerChase();
      
        if (m_distance < playerDistance + 1)
        {
            state = "Sleep";
        }
        if (m_distance < playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Away";

        }
        else if (m_distance > playerDistance && m_distance < MAXDISTANCE)
        {
            state = "Approach";
        }
        else if (m_distance > MAXDISTANCE)
        {
            state = "RandamMove";
        }
    }
   
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == PLAYER_NAME && state !="Caught")
        {
            m_attckCount += Time.deltaTime;
            if (m_attckTime < m_attckCount)
            {
                //攻撃エフェクトはここに
                PlayerHealthController.instance.DamagePlayer();
                m_attckCount = 0;
            }
        }
    }
}