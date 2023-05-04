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

    public GenericClass.E_MonsterType _type;
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
        if (Input.GetKey(KeyCode.C))
        {
            PlayerPrefs.DeleteKey("nbr"); //
            for (int i = 0; i < PlayerPrefs.GetInt("nbr"); i++)
            {
                PlayerPrefs.DeleteKey("Monster" + i + "name");
            }
            PlayerPrefs.DeleteAll();
        }
    }

    public void TakeDamage(int amount, GenericClass.E_Action KillerAction)
    {
        //_health -= amount;
        //_healthbar.fillAmount = _health / _startHealth;

        ChangeHealthValue((int)_health - amount);
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
        PlayerPrefs.SetFloat(transform.name, _health);
    }

    public void Die()
    {
        if (transform.tag == "MyMonster")
        {
            // PlayerPrefs.SetInt("nbr", PlayerPrefs.GetInt("nbr") - 1); 
            /*PlayerPrefs.DeleteKey(transform.name);  //Supprime les donn�es du monstre
            PlayerPrefs.DeleteKey(transform.name + "Health");  //Supprime les donn�es du monstre
            PlayerPrefs.DeleteKey(transform.name + "type");  //Supprime les donn�es du monstre
            PlayerPrefs.DeleteKey(transform.name + "zone");  //Supprime les donn�es du monstre
            plyCtrl_Script.armyManager_Script.Army.Remove(transform.gameObject);
            Destroy(transform.gameObject);*/

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

            PlayerPrefs.SetInt("nbr", PlayerPrefs.GetInt("nbr")+1); // Incremente le nbr de monster
            transform.name = "Monster" + PlayerPrefs.GetInt("nbr"); // Attribue Id au nom du monstre
            PlayerPrefs.SetFloat(transform.name, _health); // Definie le nbr de points de vie
            PlayerPrefs.SetInt(transform.name+"type", (int)_type); // Definie le type
            PlayerPrefs.SetInt(transform.name+"level", _level); // Definie le type

            plyCtrl_Script.armyManager_Script.Army.Add(transform.gameObject);
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
        plyCtrl_Script.CanvasLevel.text = _level.ToString();
    }

    public void setEnemyBarToGreen()
    {
        _healthbar.GetComponent<Image>().color = EnemyBar;
    }


    public void MonsterDieDestroy(Transform monster, float DestroyTime)
    {
        PlayerPrefs.DeleteKey(monster.name);  //Supprime les donn�es du monstre
        PlayerPrefs.DeleteKey(monster.name + "Health");  //Supprime les donn�es du monstre
        PlayerPrefs.DeleteKey(monster.name + "type");  //Supprime les donn�es du monstre
        PlayerPrefs.DeleteKey(monster.name + "zone");  //Supprime les donn�es du monstre
        plyCtrl_Script.armyManager_Script.Army.Remove(monster.gameObject);
        Destroy(monster.gameObject);
    }
}