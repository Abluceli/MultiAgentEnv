//引入库
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Json;
//treelet
using Newtonsoft.Json;
//treelet
using System.Collections;

public class show_xiaqi : MonoBehaviour
{
    //-1代表黑棋，1代表白棋，0代表没有落子
    private int[,] qipanInfo = new int[59, 59];
    private bool change = false;
    //private ArrayList[,] qipanInfo = new ArrayList[59, 59];
    //黑棋和白棋的对象，用于生成棋子
    public GameObject heiqi;
    public GameObject baiqi;

    private string self_ip = "58.199.160.185";
    private int myPort = 8888;
    private UdpClient recserver;

    //记录棋盘中棋子实体
    public GameObject[] qizis = new GameObject[3481];
    //记录期盼中棋子的个数
    public int qizi_num = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.startServer();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.change)
        {
            drawQiPan();
        }
    }
    public void drawQiPan()
    {
        if(this.change)
        {
            foreach (GameObject g in this.qizis)
            {
                Destroy(g);
            }
            qizi_num = 0;
            for (int i = 0; i < 59; i++)
            {
                for (int j = 0; j < 59; j++)
                {
                    GameObject qizi = null;
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == -1)
                    {
                        qizi = Instantiate(heiqi);
                        qizis[qizi_num] = qizi;
                        qizi_num++;
                    }
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == 1)
                    {
                        qizi = Instantiate(baiqi);
                        qizis[qizi_num] = qizi;
                        qizi_num++;
                    }
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == 0)
                    {
                        continue;
                    }
                    qizi.transform.position = new Vector3(i, 0.1f, j);
                }
            }
        }
        
    }
    public void startServer()
    {
        ParameterizedThreadStart s = new ParameterizedThreadStart(this.ReciveMsg);
        Thread t = new Thread(s);
        this.recserver = new UdpClient(new IPEndPoint(IPAddress.Parse(self_ip), myPort));
        t.IsBackground = true;
        t.Start(recserver);
    }
    /// <summary>  
    /// 向特定ip的主机的端口发送数据报 
    /// "111.186.116.189"), 8001
    /// </summary>  
    public void sendMsg(object obj, IPEndPoint point, string type)
    {
        string msg = "";
        if (msg != "")
        {
            string msg_new = type + "#" + msg;
            //recserver.SendTo(Encoding.UTF8.GetBytes(msg_new), point);
            recserver.Send(Encoding.UTF8.GetBytes(msg_new), Encoding.UTF8.GetBytes(msg_new).Length, point);

        }
    }
    /// <summary>  
    /// 接收发送给本机ip对应端口号的数据报  
    /// </summary>  
    public void ReciveMsg(object socket)
    {
        while (true)
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 9999);//用来保存发送方的ip和端口号  
                                                                //byte[] buffer = new byte[4096*4];
                                                                //int length = (socket as Socket).ReceiveFrom(buffer, ref point);//接收数据报  
            byte[] buffer = (socket as UdpClient).Receive(ref point);//接收数据报

            string message = Encoding.UTF8.GetString(buffer);

            //= JsonConvert.DeserializeAnonymousType<int[,]>(message,new int[59,59]);

            //Treelet
            int[,] Info = JsonConvert.DeserializeAnonymousType(message, new int[59, 59]);
            //Treelet
            for (int i = 0; i < 59; i++)
            {
                for (int j = 0; j < 59; j++)
                {
                    if(Info[i,j] == this.qipanInfo[i,j])
                    {
                        continue;
                    }
                    else
                    {
                        this.qipanInfo[i, j] = Info[i, j];
                        this.change = true;
                    }
                }
            }


                    /* foreach (GameObject g in this.qizis)
                     {
                         Destroy(g);
                     }
                     qizi_num = 0;
                     this.drawQiPan();*/


            //Debug.Log("message is " + qipanInfo[1,1]);
        }


    }

    public void closeSocket()
    {
        this.recserver.Close();
        
    }
}
