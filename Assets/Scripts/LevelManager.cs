using System.Collections;
using System.Collections.Generic;
using Esc;
using Esc.connection;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

	public int level;
	public int points;

	public Text pointsText;
	
	private static LevelManager instance;

	public static LevelManager Instance { get; private set; }

	public int Points
	{
		get { return points; }
		set
		{
			points = value;
			
			if (pointsText == null) return;
			pointsText.text = points.ToString();
		}
	}

	void Awake()
	{
		if (Instance != null && Instance != this) Destroy(gameObject);

		Instance = this;	
	}

	void Start () {
		
		Game.Instance.CreatePlayer += CreatePlayer;			
		Game.Instance.OnPlayerReconnected += ReconnectPlayer;			
		Game.Instance.OnPlayerLeft += PlayerLeft;
		
	}

	public void StartRound()
	{
		Game.HostStartRound();
		Game.Instance.ConnectionManager.BroadcastMessage(new UC_PhaseChange(new PlayerData(Phase.PLAY.ToString(),1,10)));
	    //initiate the game
	}


	void PlayerLeft(Player player)
	{
		//
	}
	
	void OnApplicationQuit()
	{
		
		Game.HostStopRound();
		Game.HostStopGame();
	}

	void ReconnectPlayer(Player player)
	{
		//
	}
	
	Player CreatePlayer(Connection connection, string deviceId, string controllerUuid, string username, bool bot)
	{
		PlayerController newPlayer = new PlayerController(connection, deviceId, controllerUuid, username, bot);
		
		return newPlayer;
	}
}
