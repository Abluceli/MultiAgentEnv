using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class xiaqiAcademy : Academy 
{
    //-1代表黑棋，1代表白棋，0代表没有落子
    public int[,] qipanInfo = new int[10, 10];

    //“black”表示现在是黑方，“white”表示现在是白方
    public string blackorwhite = "black";

    //记录棋盘中棋子实体
    public GameObject[] qizis = new GameObject[100];

    public int qizi_num = 0;

    

    public void qipanReset()
    {
        for(int i=0;i<10;i++)
        {
            for (int j=0;j<10;j++)
            {
                this.qipanInfo[i, j] = 0;
            }
        }
        foreach(GameObject g in this.qizis)
        {
            Destroy(g);
        }
        qizi_num = 0;
        blackorwhite = "black";
    }

    //判断当前下的棋子是否结束游戏，x,y为下棋的位置,player为黑方或者白方（-1，1）
    public bool determine(int x, int y, int player)
    {
        int[] DIRECTIONS_x = new int[3] { 1, -1, 0 };
        int[] DIRECTIONS_y = new int[3] { 1, -1, 0 };
        int nx, ny;
        for (int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if (i==2 && j==2)
                {
                    continue;
                }
                nx = x + DIRECTIONS_x[i];
                ny = y + DIRECTIONS_y[j];
                if (is_outta_range(nx,ny))
                {
                    continue;
                }
                if (this.qipanInfo[nx, ny] == player)
                {
                    while (this.qipanInfo[nx, ny] == player)
                    {
                        nx = nx + DIRECTIONS_x[i];
                        ny = ny + DIRECTIONS_y[j];
                        if (is_outta_range(nx, ny))
                        {
                            break;
                        }
                    }

                    nx = nx - DIRECTIONS_x[i];
                    ny = ny - DIRECTIONS_y[j];

                    bool is_end = _track(nx,ny, DIRECTIONS_x[i], DIRECTIONS_y[j]);

                    if(is_end)
                    {
                        return true;
                    }

                }
            }
            
        }

        return false;
    }

    private bool _track(int start_x, int start_y, int v1, int v2)
    {
        int x = start_x;
        int y = start_y;

        int original_player = this.qipanInfo[x, y];

        int step = 1;
        while (true)
        {
            x = x - v1;
            y = y - v2;
            if(is_outta_range(x,y)|| this.qipanInfo[x, y] != original_player)
            {
                if (step == 6)
                {
                    return true;
                }
                return false;
            }
            step += 1;
        }
    }
    public bool is_outta_range(int nx,int ny)
    {
        return nx < 0 || nx >= this.qipanInfo.GetLength(0) || ny < 0 || ny >= this.qipanInfo.GetLength(1);
    }

}
