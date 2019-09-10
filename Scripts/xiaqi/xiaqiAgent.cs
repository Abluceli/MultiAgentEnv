using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiaqiAgent : Agent
{
    public xiaqiAcademy qipan;
    public GameObject heiqi;
    public GameObject baiqi;
    public override void CollectObservations()
    {

    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        GameObject g = new GameObject();
    }

    public override void AgentReset()
    {

    }

    public void test()
    {
        _ = this.qipan.qipanInfo[1, 2];
    }

}
