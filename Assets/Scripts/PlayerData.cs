using System;

[Serializable]
public class PlayerData
{	
	public string phase;
	public int level;
	public int points;

	public PlayerData(string phase, int level, int points)
	{
		this.phase = phase;
		this.level = level;
		this.points = points;
	}

}