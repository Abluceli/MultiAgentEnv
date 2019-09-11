using MLAgents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        for(int i=0; i<59; i++)
        {
            for(int j=0; j<59; j++)
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
        //Thread.Sleep(1000);
        if (this.qipan.qizi_num == 3481)
        {
            Done();
        }
        else
        {
            if (this.agentType == this.qipan.blackorwhite)
            {

                int x = Mathf.FloorToInt(vectorAction[0]);
                int z = Mathf.FloorToInt(vectorAction[1]);
                if (this.qipan.qipanInfo[x, z] == 0)
                {
                    GameObject qizi;
                    if (this.agentType == "black")
                    {
                        qizi = Instantiate(heiqi);
                        qizi.transform.position = new Vector3(x, 0.1f, z);
                        this.qipan.qipanInfo[x, z] = -1;
                        this.qipan.qizis[this.qipan.qizi_num] = qizi;
                        this.qipan.qizi_num++;
                        this.qipan.blackorwhite = "white";
                        if (this.qipan.determine(x, z, -1))
                        {
                            
                            AddReward(100f);
                            Done();
                        }
                        else
                        {
                            if (this.qipan.qizi_num == 3481)
                            {
                                AddReward(10f);
                                Done();
                            }
                            else
                            {
                                AddReward(-1f);
                            } 
                        }

                    }
                    else
                    {
                        qizi = Instantiate(baiqi);
                        qizi.transform.position = new Vector3(x, 0.1f, z);
                        this.qipan.qipanInfo[x, z] = 1;
                        this.qipan.qizis[this.qipan.qizi_num] = qizi;
                        this.qipan.qizi_num++;
                        this.qipan.blackorwhite = "black";
                        if (this.qipan.determine(x, z, 1))
                        {

                            AddReward(100f);
                            Done();
                        }
                        else
                        {
                            if (this.qipan.qizi_num == 3481)
                            {
                                AddReward(-10f);
                                Done();
                            }
                            else
                            {
                                AddReward(-1f);
                            }
                        }
                    }
                }
                else
                {
                    this.AddReward(-1f);
                }
            }
        }

         

        
        


    }

    

}
