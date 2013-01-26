using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
	private string serverAddress = "Enter server's address here";
	private const int serverPort = 44271;
	private const string serverPassword = "!%RTHVT & ^&&  HRDSEG EW_ )@+";
	private enum ConnectState
	{
		IDLE,
		CONNECTING,
		STARTING,
		FAILED_CONN,
		FAILED_SERVE // THIS STATE NEVER HAPPENS?	
	}
	private ConnectState connectState = ConnectState.IDLE;
	// Use this for initialization
	void Start ()
	{
		Application.runInBackground = true;
	}
	
	void StartServer ()
	{
		Network.InitializeServer (32, serverPort, false);
		connectState = ConnectState.STARTING;
	}
	
	void StartConnect ()
	{
		Network.Connect (serverAddress, serverPort);
		connectState = ConnectState.CONNECTING;
	}
	
	void OnConnectedToServer ()
	{
		Application.LoadLevel ("Wait");	
	}
	
	void OnServerInitialized() {
		Networking.nPlayers = 0;
		Networking.players[Networking.nPlayers++] = Network.player;
		GameMode.nConnected = 0;
		Application.LoadLevel ("Wait");	
	}
	
	void OnFailedToConnect() {
		connectState = ConnectState.FAILED_CONN;	
	}
	
	void AlertWindow(int windowId) {
		GUILayout.BeginVertical();
			string msg = "";
			switch (connectState) {
			case ConnectState.CONNECTING:
				msg = "Connecting...";
				break;
			case ConnectState.STARTING:
				msg = "Starting server...";
				break;
			case ConnectState.FAILED_CONN:
				msg = "Failed to connect to server. Please try again.";
				break;
			case ConnectState.FAILED_SERVE:
				msg = "Failed to start the server. Please try again.";
				break;
			}
			GUILayout.Label (msg);
			if (connectState == ConnectState.FAILED_CONN || connectState == ConnectState.FAILED_SERVE) {
				GUILayout.Space (10);
				if (GUILayout.Button("OK")) {
					connectState = ConnectState.IDLE;	
				}
			}
			GUILayout.EndVertical ();
	}
	
	void OnGUI ()
	{
		var w = 1280; //Screen.width;
		var h = 800; //Screen.height;
		
		var btnW = 150;
		var btnH = 100;
		GUILayout.BeginArea (new Rect (10, 10, 200, 100));
		
		serverAddress = GUILayout.TextField (serverAddress);
		
		if (GUILayout.Button ("Connect and Play")) {
			print ("Playing");	
			StartConnect();
		}
		
		GUILayout.FlexibleSpace ();
		
		if (GUILayout.Button ("Start a Server")) {
			print ("Serving");
			StartServer();
		}
		
		
		GUILayout.EndArea ();
		
		// Dialogs!
		int dW = 300;
		int dH = 200;
		if (connectState != ConnectState.IDLE) {
			GUILayout.Window (0, new Rect (w / 2 - dW / 2, h / 2 - dH / 2, dW, dH),
				AlertWindow, "");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
