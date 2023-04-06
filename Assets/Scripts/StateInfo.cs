using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateInfo : MonoBehaviour
{
    public Image image;
    private SoldierBehavior Soldier;

    public Texture2D attackTexture;
    public Texture2D followTexture;
    // Start is called before the first frame update
    void Start()
    {
        Soldier = transform.parent.GetComponent<SoldierBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        GenericClass.E_Action action = Soldier._actionState;
        switch (action)
        {
            case GenericClass.E_Action.Attack:
                image.sprite = Sprite.Create(attackTexture, new Rect(0, 0, attackTexture.width, attackTexture.height), new Vector2(0.5f, 0.5f)); ;
                break;
            case GenericClass.E_Action.Follow:
                image.sprite = Sprite.Create(followTexture, new Rect(0, 0, followTexture.width, followTexture.height), new Vector2(0.5f, 0.5f)); ;
                break;
        }
    }
}
