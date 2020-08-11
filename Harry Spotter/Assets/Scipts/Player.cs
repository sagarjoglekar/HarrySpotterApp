using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _playerMaxHealth = 200;
    [SerializeField] private int _playerMinHealth = 0;

    private int _playerCurrentHealth;
    public bool isPlayerDead = false;

    [SerializeField] private HealthBar _playerHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        _playerCurrentHealth = _playerMaxHealth;
        _playerHealthBar.SetMaxHealth(_playerCurrentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDame(int damge)
    {
        if (_playerCurrentHealth > 0)
        {
            _playerCurrentHealth = _playerCurrentHealth - damge;
            _playerHealthBar.SetHealth(_playerCurrentHealth);
        }
        else
        {
            _playerCurrentHealth = 0;
            isPlayerDead = true;
        }

    }


}
