using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistique : MonoBehaviour
{

    public float _startHealth = 100;
    //[HideInInspector]
    public float _health;

    public Image _healthbar;
    public Color EnemyBar;


    void Start()
    {
        _health = _startHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;

        _healthbar.fillAmount = _health / _startHealth;
        if (_health <= 0)
        {
            Die();
        }
    }

    public void ChangeHealthValue(int amount)
    {
        _health = amount;
        _healthbar.fillAmount = _health / _startHealth;
    }

    public void Die()
    {
        if (transform.tag == "MyMonster")
        {
            Destroy(transform.gameObject);
        }
        else if (transform.tag == "Enemy")
        {
            GetComponent<Statistique>().ChangeHealthValue((int)_startHealth/2);
            _healthbar.GetComponent<Image>().color = EnemyBar;
            transform.tag = "MyMonster";
            GetComponent<EnemyBehavior>().enabled = false;
            GetComponent<SoldierBehavior>().enabled = true;
        }
    }
}