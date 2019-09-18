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
using System.Collections.Generic;
using UnityEngine.UI;
using System.Globalization;

public class playWithMan_xiaqi : MonoBehaviour
{
    //-1代表黑棋，1代表白棋，0代表没有落子
    private int[,] qipanInfo;
    private bool change = false;
    //private ArrayList[,] qipanInfo = new ArrayList[59, 59];
    //黑棋和白棋的对象，用于生成棋子
    public GameObject heiqi;
    public GameObject baiqi;
    public GameObject x;
    public GameObject z;
    public GameObject clickobj;
    
    public Transform main_camera;
    public Transform plane;


    private string self_ip = "58.199.160.185";
    private int myPort = 8888;
    private UdpClient recserver;

    //记录棋盘中棋子实体
    private List<GameObject> qizis = new List<GameObject>();

    private List<GameObject> xzs = new List<GameObject>();
    private List<GameObject> clickobjs = new List<GameObject>();

    private int xz_rows = 19;
    private bool xz_change = false;

    private bool heiORbai = true;
    private int steps = 0;
    // Start is called before the first frame update
    void Start()
    {

        //this.startServer();
        //drawQiPan();

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void step()
    {
        GameObject qizi = null;
        if (this.heiORbai)
        {
            
            qizi = Instantiate(heiqi);
            qizi.transform.position = new Vector3(this.transform.position.x, 0.1f, this.transform.position.z);
        }
        else
        {
            qizi = Instantiate(baiqi);
            qizi.transform.position = new Vector3(this.transform.position.x, 0.1f, this.transform.position.z);
        }
        this.qizis.Add(qizi);
        this.steps++;

        if(this.steps == 1)
        {
            this.heiORbai = false;
        }
    }

    public void drawQiPan()
    {
        if(xz_change)
        {
            main_camera.position = new Vector3(xz_rows / 2, xz_rows, xz_rows / 2);
            plane.position = new Vector3(xz_rows / 2, 0, xz_rows / 2);
            plane.localScale = new Vector3(xz_rows / 10f, 1, xz_rows / 10f);
            foreach (GameObject g in this.xzs)
            {
                DestroyImmediate(g);
            }
            foreach (GameObject g in this.clickobjs)
            {
                DestroyImmediate(g);
            }
            foreach (GameObject g in this.qizis)
            {
                DestroyImmediate(g);
            }

            GameObject xz = null;
            for (int i = 0; i < xz_rows; i++)
            {
                xz = Instantiate(x);
                if (xz_rows % 2 == 1)
                {
                    xz.transform.position = new Vector3((xz_rows - 1) / 2, 0, i);
                    xz.transform.localScale = new Vector3(xz_rows - 1, 0.2f, 0.1f);
                    xzs.Add(xz);
                }
                else
                {
                    xz.transform.position = new Vector3((xz_rows - 1) / 2f, 0, i);
                    xz.transform.localScale = new Vector3(xz_rows - 1, 0.2f, 0.1f);
                    xzs.Add(xz);
                }
            }
            for (int i = 0; i < xz_rows; i++)
            {
                xz = Instantiate(z);
                if (xz_rows % 2 == 1)
                {
                    xz.transform.position = new Vector3(i, 0, (xz_rows - 1) / 2);
                    xz.transform.localScale = new Vector3(0.1f, 0.2f, xz_rows - 1);
                    xzs.Add(xz);
                }
                else
                {
                    xz.transform.position = new Vector3(i, 0, (xz_rows - 1) / 2f);
                    xz.transform.localScale = new Vector3(0.1f, 0.2f, xz_rows - 1);
                    xzs.Add(xz);
                }
            }
            for(int i = 0; i < xz_rows; i++)
            {
                for(int j = 0; j < xz_rows; j++)
                {
                    xz = Instantiate(clickobj);
                    xz.transform.position = new Vector3(i, 0.5f, j);
                    clickobjs.Add(xz);
                }
            }

            xz_change = false;

        }

        if (change)
        {
            foreach (GameObject g in this.qizis)
            {
                DestroyImmediate(g);
            }
            for (int i = 0; i < xz_rows; i++)
            {
                for (int j = 0; j < xz_rows; j++)
                {
                    GameObject qizi = null;
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == 0)
                    {
                        qizi = Instantiate(heiqi);
                        qizis.Add(qizi);

                    }
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == 1)
                    {
                        qizi = Instantiate(baiqi);
                        qizis.Add(qizi);
                    }
                    if (System.Convert.ToInt32(qipanInfo.GetValue(i, j)) == 2)
                    {
                        continue;
                    }
                    qizi.transform.position = new Vector3(i, 0.1f, j);
                }
            }
            change = false;
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


            //Treelet
            int[,] Info = JsonConvert.DeserializeObject<int[,]>(message);
            //Treelet

            if (Info.GetLength(0) != this.xz_rows)
            {
                this.xz_rows = Info.GetLength(0);
                this.xz_change = true;
                this.change = true;
                qipanInfo = Info;
            }
            else
            {
                //this.xz_change = false;
                this.change = true;
                qipanInfo = Info;
            }


            Debug.Log("message is " + Info.GetLength(0));
        }


    }

    public void closeSocket()
    {
        this.recserver.Close();

    }

    public void drawQipanWithHuman()
    {
        //Dropdown dropDown = GameObject.Find("InputField ").GetComponent<Dropdown>();
        
        InputField inputField = GameObject.Find("Canvas").GetComponentInChildren<InputField>();
        //this.xz_rows = System.Convert.ToInt32(inputField.text);
        //Debug.Log(this.GetComponent<InputField>().text);
        this.xz_rows = System.Convert.ToInt32(inputField.text);
        this.xz_change = true;
        this.drawQiPan();


    }

    
}
