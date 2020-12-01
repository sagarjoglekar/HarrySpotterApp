using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameNetwork;
public class Enemy : MonoBehaviour
{

    [SerializeField] private int _enemyMaxHealth = 200;
    [SerializeField] private int _enmeyMinHealth = 0;
    [SerializeField] private int _enemyAttackRate = 10;
    [SerializeField] private int _enemyAttackDamage = 1;
    private int _enemyCurrentHealth;
    private bool _isEnemyDead = false;

    [SerializeField] private HealthBar _healthBar;

    [SerializeField] private Animator _animator;

    private Player _player;

    private bool _gotCalled = false;

    [SerializeField] private LocalUser _localUser;
    [SerializeField] private GameNetwork _gameNetwork;
    [SerializeField] private UIManagerFight _uIManagerFight;
    // Start is called before the first frame update
    void Start()
    {
        _gotCalled = false;
        _enemyCurrentHealth = _enemyMaxHealth;
        print(_enemyCurrentHealth);
        _healthBar.SetMaxHealth(_enemyCurrentHealth);


        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
      //  print(_player.isPlayerDead);
      if(_gotCalled == false && FightManager.startFgiht == true)
        {
            StartCoroutine(EnemeyAttackRoutin(_enemyAttackRate));
            _gotCalled = true;
        }

    }


    private void OnMouseDown()
    {
        if (FightManager.startFgiht == true && _player.isPlayerDead == false)
        {
            TakeDame(_localUser.currentUserDefenderScore);//player damage rate
            print(_enemyCurrentHealth);
        }
    }


    IEnumerator EnemeyAttackRoutin(int attackRate)
    {
        while(_isEnemyDead == false)
        {
            yield return new WaitForSeconds(attackRate);
            if(_player.isPlayerDead == false)
            {
                _animator.SetTrigger("Attack1Trigger");
                _player.TakeDame(_localUser.currentDefenderScore + 100);//Enemy Attach damage Rate
            } else
            {
                if(_isEnemyDead != true)
                {
                    FightSet fightSet = new FightSet(_localUser.currentEventId, _localUser.userID, false, _localUser.currentMutex);
                    StartCoroutine(_gameNetwork.SetFightSatus("https://harryspotter-backend.portmap.io:26214/setFightStatus", fightSet.Serialize().ToString()));
                    _uIManagerFight.DisplayEndPanel(false);//player lost show panel
                    break;
                }
            }

            print(_player.isPlayerDead);
        }
    }

    private void TakeDame(int damge)
    {
        if(_enemyCurrentHealth > 0)
        {
            _enemyCurrentHealth = _enemyCurrentHealth - damge;
            _healthBar.SetHealth(_enemyCurrentHealth);
        } else
        {
            _enemyCurrentHealth = 0;
            _isEnemyDead = true;
            FightSet fightSet = new FightSet(_localUser.currentEventId, _localUser.userID, true, _localUser.currentMutex);
            StartCoroutine(_gameNetwork.SetFightSatus("https://harryspotter-backend.portmap.io:26214/setFightStatus", fightSet.Serialize().ToString()));
            _uIManagerFight.DisplayEndPanel(true);//player won show panel
        }

    }
}
