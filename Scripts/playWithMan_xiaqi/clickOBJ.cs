using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickOBJ : MonoBehaviour
{
    public GameObject heiqi;
    public GameObject baiqi;

    public void clickEvent()
    {
        //GameObject qizi = null;
        GameObject.Find("palyWithMan_xiaqi").GetComponent<playWithMan_xiaqi>().step();

        
        
    }
}
