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

    //ElementForGameOver
    public GameObject UIEndPopup;
    public GameObject ButtonPanel;
    public GameObject UIJoyStick;
    public int _level = 1;

    private bool InfoUpadted;

    void Start()
    {
        InfoUpadted = false;
        plyCtrl_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();

        /*if (_health == 0)
        {
            _health = _startHealth;
        }

        else
        {
            ChangeHealthValue((int)_health);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void TakeDamage(int amount, GenericClass.E_Action KillerAction)
    {
        //_health -= amount;
        //_healthbar.fillAmount = _health / _startHealth;
        int newHealth = (int)_health - amount;
        ChangeHealthValue(newHealth);
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
                PlayerPrefs.DeleteAll();
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
            MonsterDieDestroy(transform, 0);
        }
        else if (transform.tag == "Enemy")
        {
            GetComponent<Statistique>().ChangeHealthValue((int)_startHealth/2);
            //_healthbar.GetComponent<Image>().color = EnemyBar;
            setEnemyBarToGreen();
            transform.tag = "MyMonster";
            GetComponent<EnemyBehavior>().enabled = false;
            GetComponent<SoldierBehavior>().enabled = true;
            gameObject.transform.SetParent(plyCtrl_Script.ArmyParent.transform);


            plyCtrl_Script.armyManager_Script.Army.Add(transform.gameObject);
        }
    }

    private void OnMouseUp()
    {

    }


    void OnMouseDown()
    {
        if(tag == "LocalPlayer")
        {
            UpdateStatInfoPanel();
        }
    }

    private void OnMouseOver()
    {
        if(InfoUpadted == false)
        {
            if (tag == "MyMonster" || tag == "Enemy")
            {
                UpdateStatInfoPanel();
                InfoUpadted = true;
            }
        }
    }

    private void OnMouseExit()
    {
        InfoUpadted = false;
        plyCtrl_Script.CanvasInfos.SetActive(false);
    }

    public void setEnemyBarToGreen()
    {
        _healthbar.GetComponent<Image>().color = EnemyBar;
    }


    public void MonsterDieDestroy(Transform monster, float DestroyTime)
    {
        plyCtrl_Script.armyManager_Script.Army.Remove(monster.gameObject);
        Destroy(monster.gameObject);
    }

    public void UpdateStatInfoPanel()
    {
        plyCtrl_Script.CanvasInfos.SetActive(true);
        plyCtrl_Script.CanvasImage.sprite = monsterImage;
        plyCtrl_Script.CanvasHealth.text = _health.ToString();
        plyCtrl_Script.CanvasDamage.text = damage.ToString();
        plyCtrl_Script.CanvasAtkSpeed.text = attackSpeed.ToString();
        plyCtrl_Script.CanvasLevel.text = _level.ToString();
        InfoUpadted = true;
    }
}