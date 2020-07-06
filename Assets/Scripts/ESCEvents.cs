using System;
using Esc.events;
using UnityEngine.SceneManagement;


//Game --> Controller
[Serializable]
public class UC_PhaseChange: EscEvent
{
	public PlayerData playerData;
	
	public UC_PhaseChange(PlayerData playerData)
	{
		this.playerData = playerData;
	}
}


//Controller --> Game
[Serializable]
public class CU_PlayerAction : EscEvent
{
	public int points;

	public CU_PlayerAction(int points)
	{
		this.points = points;
	}

	
}


[Serializable]
public class CU_JoinGame : EscEvent
{                
	public CU_JoinGame() {}
}