using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class show_xiaqi : MonoBehaviour
{
    //-1代表黑棋，1代表白棋，0代表没有落子
    private int[,] qipanInfo = new int[59, 59];
    //黑棋和白棋的对象，用于生成棋子
    public GameObject heiqi;
    public GameObject baiqi;

    private UdpClient udpClient;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 59; i++)
        {
            for (int j = 0; j < 59; j++)
            {
                GameObject qizi = Instantiate(heiqi);
                qizi.transform.position = new Vector3(i, 0.1f, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
