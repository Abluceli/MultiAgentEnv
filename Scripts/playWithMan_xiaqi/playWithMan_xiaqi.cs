//引入库
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Json;
//treelet
//using Newtonsoft.Json;
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

    public GameObject playa;

    // Start is called before the first frame update
    void Start()
    {
        this.playa.SetActive(false);
        //this.startServer();
        //drawQiPan();

    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void step(float x, float z)
    {
        bool isEnd = false;
        int win = 0;
        GameObject qizi = null;
        if(this.steps == 0 && this.heiORbai)
        {
            qizi = Instantiate(heiqi);
            qizi.transform.position = new Vector3(x, 0.1f, z);
            this.qizis.Add(qizi);
            this.steps++;
            this.heiORbai = false;
            this.qipanInfo[(int)x, (int)z] = -1;
            isEnd = this.determine((int)x, (int)z, -1);
            win = -1;
        }
       else if(this.steps != 0 && this.heiORbai)
        {
            qizi = Instantiate(heiqi);
            qizi.transform.position = new Vector3(x, 0.1f, z);
            this.qizis.Add(qizi);
            this.steps++;
            if(this.steps == 3)
            {
                this.heiORbai = false;
                this.steps = 1;
            }
            this.qipanInfo[(int)x, (int)z] = -1;
            isEnd = this.determine((int)x, (int)z, -1);
            win = -1;
        }
        else if (this.steps != 0 && !this.heiORbai)
        {
            qizi = Instantiate(baiqi);
            qizi.transform.position = new Vector3(x, 0.1f, z);
            this.qizis.Add(qizi);
            this.steps++;
            if (this.steps == 3)
            {
                this.heiORbai = true;
                this.steps = 1;
            }
            this.qipanInfo[(int)x, (int)z] = 1;
            isEnd = this.determine((int)x, (int)z, 1);
            win = 1;
        }
        if(isEnd)
        {
            //Text message = GameObject.Find("Canvas").GetComponentInChildren<Text>();
            Text messages = GameObject.Find("message").GetComponent<Text>();
            if(win == -1)
            {
                messages.text = "游戏结束！黑方胜出！";
                this.playa.SetActive(true);
            }
            else if(win == 1)
            {
                messages.text = "游戏结束！白方胜出！";
                this.playa.SetActive(true);
            }

            
        }
    }

    public void playagain()
    {
        Text messages = GameObject.Find("message").GetComponent<Text>();
        messages.text = "";
        this.playa.SetActive(false);
        this.heiORbai = true;
        this.steps = 0;
        this.qipanInfo = new int[this.xz_rows, this.xz_rows];
        this.xz_change = true;
        this.drawQiPan();
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
            int[,] Info = null;// JsonConvert.DeserializeObject<int[,]>(message);
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
        this.qipanInfo = new int[this.xz_rows, this.xz_rows];
        this.xz_change = true;
        this.drawQiPan();


    }



    //判断当前下的棋子是否结束游戏，x,y为下棋的位置,player为黑方或者白方（-1，1）
    public bool determine(int x, int y, int player)
    {
        if (player != 0)
        {
            int[] DIRECTIONS_x = new int[3] { 1, -1, 0 };
            int[] DIRECTIONS_y = new int[3] { 1, -1, 0 };
            int nx, ny;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 2 && j == 2)
                    {
                        continue;
                    }
                    nx = x + DIRECTIONS_x[i];
                    ny = y + DIRECTIONS_y[j];
                    if (is_outta_range(nx, ny))
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

                        bool is_end = _track(nx, ny, DIRECTIONS_x[i], DIRECTIONS_y[j]);

                        if (is_end)
                        {
                            return true;
                        }

                    }
                }

            }

            return false;
        }
        else
        {
            return false;
        }
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
            if (is_outta_range(x, y) || this.qipanInfo[x, y] != original_player)
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
    public bool is_outta_range(int nx, int ny)
    {
        return nx < 0 || nx >= this.qipanInfo.GetLength(0) || ny < 0 || ny >= this.qipanInfo.GetLength(1);
    }
}
