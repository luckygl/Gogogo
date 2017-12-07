using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using System.Security.Cryptography;   //加密数据类，算法
//this is client

public class TCPClient : MonoBehaviour {

	Socket connectServerSocket;
	IPEndPoint m_serverPort;
	Thread T1;
	public Text Ta;
	string m_msgRespond;

	void Start () {
		try{	
			print ("TCPClient.Start()");
			IPAddress m_serverIp = IPAddress.Parse("192.168.1.113");
			m_serverPort = new IPEndPoint (m_serverIp,9990);
			connectServerSocket = new Socket (AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);		
			T1 = new Thread (new ThreadStart(ThreadtoConnectServer));
			T1.Start ();		
		}
		catch(System.Exception ex){
			
		}
	}

	void ThreadtoConnectServer(){
		Debug.Log ("子线程开启,准备连接服务器");
		connectServerSocket.Connect (m_serverPort);
		print ("已经连接上服务器");
		string message = "hello,I'm Unity TCP Client";
		byte[] sendbyteArr = System.Text.Encoding.UTF8.GetBytes (message);
		int successSendBytes = connectServerSocket.Send (sendbyteArr,sendbyteArr.Length,SocketFlags.None);
		print ("发送数据成功");

		while (true) {
			byte[] receivebytes = new byte[1024];
			int successReceiveBytes = connectServerSocket.Receive (receivebytes);
			string str=System.Text.Encoding.UTF8.GetString(receivebytes);
			m_msgRespond = str;
			print ("接收到服务器发来的信息："+str);
		}

	}

	void Update(){
//		print ("Update");
		Ta.text=m_msgRespond;
	}


	public void EventSendButton(){
		string message = "hello,客户端发送按钮被按下，并发送";
		byte[] sendbyteArr = System.Text.Encoding.UTF8.GetBytes (message);
		int successSendBytes = connectServerSocket.Send (sendbyteArr,sendbyteArr.Length,SocketFlags.None);
	}

	void OnDestory(){
		print ("Destory被执行");
		connectServerSocket.Close ();
	}

	void OnDisable(){
		print ("Disable");
	}
}
