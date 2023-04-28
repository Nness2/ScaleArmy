using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDetect : MonoBehaviour
{

    public GameObject UIEndPopup;
    public GameObject ButtonPanel;
    public GameObject UIJoyStick;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LocalPlayer")
        {
            UIEndPopup.SetActive(true);
            ButtonPanel.SetActive(false);
            UIJoyStick.SetActive(false);
        }
    }

}
