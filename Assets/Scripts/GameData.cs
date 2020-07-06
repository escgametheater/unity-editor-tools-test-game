using System;

[Serializable]
public class GameData
{	
	public string gameState;
	public int gameLevel;
	public string name;

	public GameData(string gameState, int gameLevel, string name)
	{
		this.gameState = gameState;
		this.gameLevel = gameLevel;
		this.name = name;
	}

}