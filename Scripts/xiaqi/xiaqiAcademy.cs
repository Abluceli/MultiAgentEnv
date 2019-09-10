using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class xiaqiAcademy : Academy 
{
    //-1代表黑棋，1代表白棋，0代表没有落子
    public int[,] qipanInfo = new int[10, 10];
    //“black”表示现在是黑方，“white”表示现在是白方
    public string blackorwhite = "";
    //记录棋盘中棋子实体
    public GameObject[] qizi = new GameObject[100];


    public void qipanReset()
    {
        for(int i=0;i<10;i++)
        {
            for (int j=0;j<10;j++)
            {
                this.qipanInfo[i, j] = 0;
            }
        }
        foreach(GameObject g in this.qizi)
        {
            Destroy(g);
        }
    }
}
