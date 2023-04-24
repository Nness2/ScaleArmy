using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistique : MonoBehaviour
{

    public float _startHealth = 100;
    //[HideInInspector]

    public Image _healthbar;
    public Color EnemyBar;

    public PlayerController plyCtrl_Script;

    //Statistique
    public Sprite monsterImage;
    public float _health;
    public int damage;
    public float speed;
    public float attackSpeed;
    public float attackDistance;
    public float detectDistance;


    void Start()
    {
        plyCtrl_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();

        if (_health == 0)
        {
            _health = _startHealth;
        }
        else
        {
            ChangeHealthValue((int)_health);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(int amount, GenericClass.E_Action KillerAction)
    {
        _health -= amount;

        _healthbar.fillAmount = _health / _startHealth;
        if (_health <= 0)
        {
            Die();
            GetComponent<SoldierBehavior>()._actionState = KillerAction;
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

    private void OnMouseUp()
    {

    }


    void OnMouseDown()
    {
        plyCtrl_Script.CanvasInfos.SetActive(true);
        plyCtrl_Script.CanvasImage.sprite = monsterImage;
        plyCtrl_Script.CanvasHealth.text = _health.ToString();
        plyCtrl_Script.CanvasDamage.text = damage.ToString();
        plyCtrl_Script.CanvasAtkSpeed.text = attackSpeed.ToString();


    }
}