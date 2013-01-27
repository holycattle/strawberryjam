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
	private bool modalOn = false;
	private ConnectState connectState = ConnectState.IDLE;
	private bool displayConnectDialog = false;
	private bool displayServeDialog = false;
	
	public Texture2D texBackground;
	public GUIStyle connectButton;
	public GUIStyle serverButton;
	
	// Use this for initialization
	void Start ()
	{
		Application.runInBackground = true;
		modalOn = false;
	}
	
	void StartServer ()
	{
		Network.InitializeServer (3, serverPort, false);
		connectState = ConnectState.STARTING;
	}
	
	void StartConnect ()
	{
		Network.Connect (serverAddress, serverPort);
		Networking.serverAddress = serverAddress;
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
	
	void ConnectWindow(int windowId) {
		GUILayout.BeginVertical ();
		
		serverAddress = GUILayout.TextField (serverAddress);
		
		if (GUILayout.Button ("Connect")) {
			StartConnect();	
		}
		GUILayout.EndVertical ();
	}
	
	void OnGUI () {
		var w = 1280; //Screen.width;
		var h = 800; //Screen.height;
		
		// Background
		GUI.DrawTexture(new Rect(0, 0, w, h), texBackground);
		
		var btnW = 445;
		var btnH = 58;
		// Dialogs!
		int dW = 300;
		int dH = 200;
		if (connectState != ConnectState.IDLE) {
			modalOn  = true;
			GUILayout.Window (0, new Rect (w / 2 - dW / 2, h / 2 - dH / 2, dW, dH),
				AlertWindow, "");
		} else if (displayConnectDialog) {
			modalOn = true;
			GUILayout.Window (0, new Rect (w / 2 - dW / 2, h / 2 - dH / 2, dW, dH),
				ConnectWindow, "");
		}
		if (modalOn) {
			GUI.enabled = false;	
		}
		
		if (GUI.Button(new Rect(w / 2 - btnW / 2, 580, btnW, btnH), "", connectButton)) {
			print ("Playing");	
			displayConnectDialog = true;
		}
		if (GUI.Button(new Rect(w / 2 - btnW / 2, 640, btnW, btnH), "", serverButton)) {
			print ("Serving");
			StartServer();
		}
		GUI.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
