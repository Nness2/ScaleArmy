using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmStatistique : MonoBehaviour
{

    public float _startHealth = 100;
    //[HideInInspector]

    public Image _healthbar;
    public Color EnemyBar;
    

    //Statistique
    public Sprite monsterImage;
    public float _health;
    public int damage;
    public float speed;
    public float attackSpeed;
    public float attackDistance;
    public float detectDistance;

    //ElementForGameOver
    public GameObject UIEndPopup;
    public GameObject ButtonPanel;
    public GameObject UIJoyStick;

    void Start()
    {

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

        if (transform.tag == "MyMonster" || transform.tag == "Enemy")
        {
            if (_health <= 0)
            {
                Die();
                GetComponent<SoldierBehavior>()._actionState = KillerAction;
            }
        }


        if (transform.tag == "LocalPlayer")
        {
            if (_health <= 0)
            {
                UIEndPopup.SetActive(true);
                ButtonPanel.SetActive(false);
                UIJoyStick.SetActive(false);
            }
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

    
    /*void OnMouseDown()
    {
        plyCtrl_Script.CanvasInfos.SetActive(true);
        plyCtrl_Script.CanvasImage.sprite = monsterImage;
        plyCtrl_Script.CanvasHealth.text = _health.ToString();
        plyCtrl_Script.CanvasDamage.text = damage.ToString();
        plyCtrl_Script.CanvasAtkSpeed.text = attackSpeed.ToString();
    }*/
}