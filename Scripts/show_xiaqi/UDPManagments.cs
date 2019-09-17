using System.Collections.Generic;
//引入库
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpManagment
{

    //服务器端
    //public Dictionary<int, UdpClient> port_sockets_server;
    //static public Dictionary<int, Control> port_control = new Dictionary<int, Control>();
    public string self_ip;
    //public string ServerIP;
    public int myPort;
    //public Scenes sence;
    //public List<Control> controls;
   // public bool setSence = false;
    public UdpClient recserver;

    //"111.186.116.169"), 8001
    public UdpManagment(string ip, int port)//,int senport
    {
        //this.controls = new List<Control>();
        //this.port_sockets_server = new Dictionary<int, UdpClient>();
        this.self_ip = ip;
        //this.ServerIP = ServerIP;
        this.myPort = port;
    }

    public void startServer()
    {
        ParameterizedThreadStart s = new ParameterizedThreadStart(this.ReciveMsg);
        Thread t = new Thread(s);
        //recserver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //recserver.Bind(new IPEndPoint(IPAddress.Parse(self_ip), myPort));//绑定端口号和IP
        recserver = new UdpClient(new IPEndPoint(IPAddress.Parse(self_ip), myPort));
        t.IsBackground = true;
        t.Start(recserver);
    }

    /*public void start()
    {
        ParameterizedThreadStart s = new ParameterizedThreadStart(this.ReciveMsg);
        foreach (int key in this.port_sockets_server.Keys)
        {
            Thread t = new Thread(s);
            t.IsBackground = true;
            t.Start(this.port_sockets_server[key]);
        }
    }*/

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
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号  
                                                                //byte[] buffer = new byte[4096*4];
                                                                //int length = (socket as Socket).ReceiveFrom(buffer, ref point);//接收数据报  
            byte[] buffer = (socket as UdpClient).Receive(ref point);//接收数据报
            string message = Encoding.UTF8.GetString(buffer);
            Debug.Log("message is "+ message);
           /* if (message != "")
            {
                string[] msg = message.Split('#');
                string type = msg[0];

                *//*if (type == "control")
                {
                    //Control con = (Control)JsonConvert.DeserializeObject(msg[1], typeof(Control));
                    // port_control.Add(int.Parse(point.ToString().Split(':')[1]), con);
                   // port_control[int.Parse(point.ToString().Split(':')[1])] = con;
                    // Debug.Log("xxxxxxxxx is"+int.Parse(point.ToString().Split(':')[1]));
                }*//*
                //int.Parse(point.ToString().Split(':')[1])
                //this.sendMsg(con, new IPEndPoint(IPAddress.Parse(this.ServerIP), int.Parse(point.ToString().Split(':')[1])), type);
                //this.tasks.Add(obj);


            }*/

        }


        }

    public void closeSocket()
    {
        this.recserver.Close();
        /*foreach (int Key in this.port_sockets_server.Keys)
        {
            if (port_sockets_server[Key] != null)
            {
                port_sockets_server[Key].Close();
            }

        }*/

    }
}


