using System;
using System.Collections;
using System.Collections.Generic;
using Esc;
using Esc.connection;
using Esc.events;
using UnityEngine;

public class PlayerController : Player {

	// Use this for initialization
	void Start () {
		
	}
	
	public PlayerController(Connection connection, string deviceId, string controllerUuid, string username, bool bot) : base(connection, deviceId, controllerUuid, username, bot) 
	{
		Debug.Log($"Creating a Player Controller deviceID {deviceId} controllerUuid{controllerUuid} username{username} bot{bot}\n");
		Connection?.RegisterEventHandler(typeof(CU_PlayerAction), OnPlayerAction);
		Connection?.RegisterEventHandler(typeof(CU_JoinGame), OnJoinGame);		
		
		
		
		
	}

	void OnPlayerAction(string eventName, int connectionId, EscEvent escEvent)
	{
		var action = (CU_PlayerAction) escEvent;
		Debug.Log($"Player joined onPlayerAction!");
		LevelManager.Instance.Points += action.points;
	}
	
	void OnJoinGame(string eventName, int connectionId, EscEvent escEvent)
	{						
		Debug.Log($"Player joined onJoinGame!");
		//Send some relevant player data from the Player Controller to the Host 
		Connection?.SendMessage(new UC_PhaseChange(new PlayerData(Phase.WAIT.ToString(), 0, 0)));			
	}
}

public enum Phase {WAIT, PLAY}

