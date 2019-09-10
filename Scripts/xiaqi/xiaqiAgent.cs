using MLAgents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xiaqiAgent : Agent
{
    public xiaqiAcademy qipan;

    //黑棋和白棋的对象，用于生成棋子
    public GameObject heiqi;
    public GameObject baiqi;

    //黑棋的控制者还是白棋的控制者
    public string agentType;

    public override void CollectObservations()
    {
        for(int i=0; i<10; i++)
        {
            for(int j=0; j<10; j++)
            {
                this.AddVectorObs(this.qipan.qipanInfo[i,j]);
            }
        }
        
    }

    public override void AgentReset()
    {
        this.qipan.qipanReset();
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if(this.agentType == this.qipan.blackorwhite)
        {
            int i = Mathf.FloorToInt(vectorAction[0]);
            int j = Mathf.FloorToInt(vectorAction[1]);
            if(this.qipan.qipanInfo[i,j] == 0)
            {
                float x = -1, z = -1;
                switch (i)
                {
                    case 0:
                        x = 4.5f;
                        break;
                    case 1:
                        x = 3.5f;
                        break;
                    case 2:
                        x = 2.5f;
                        break;
                    case 3:
                        x = 1.5f;
                        break;
                    case 4:
                        x = 0.5f;
                        break;
                    case 5:
                        x = -0.5f;
                        break;
                    case 6:
                        x = -1.5f;
                        break;
                    case 7:
                        x = -2.5f;
                        break;
                    case 8:
                        x = -3.5f;
                        break;
                    case 9:
                        x = -4.5f;
                        break;

                }
                switch (j)
                {
                    case 0:
                        z = 4.5f;
                        break;
                    case 1:
                        z = 3.5f;
                        break;
                    case 2:
                        z = 2.5f;
                        break;
                    case 3:
                        z = 1.5f;
                        break;
                    case 4:
                        z = 0.5f;
                        break;
                    case 5:
                        z = -0.5f;
                        break;
                    case 6:
                        z = -1.5f;
                        break;
                    case 7:
                        z = -2.5f;
                        break;
                    case 8:
                        z = -3.5f;
                        break;
                    case 9:
                        z = -4.5f;
                        break;

                }
                if (this.qipan.qizi_num == 100)
                {
                    Done();
                }
                else
                {
                    GameObject qizi;
                    if (this.agentType == "black")
                    {
                        qizi = Instantiate(heiqi);
                        qizi.transform.position = new Vector3(x, 0, z);
                        this.qipan.qipanInfo[i, j] = -1;
                        this.qipan.qizis[this.qipan.qizi_num] = qizi;
                        this.qipan.qizi_num++;
                        this.qipan.blackorwhite = "white";

                    }
                    else
                    {
                        qizi = Instantiate(baiqi);
                        qizi.transform.position = new Vector3(x, 0, z);
                        this.qipan.qipanInfo[i, j] = 1;
                        this.qipan.qizis[this.qipan.qizi_num] = qizi;
                        this.qipan.qizi_num++;
                        this.qipan.blackorwhite = "black";
                    }
                }

                

            }
            else
            {
                this.AddReward(-0.1f);
            }
        }

        
        
    }



}
