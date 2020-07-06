using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CustomData", menuName = "Custom Asset Data", order = 1)]
public class CustomData : ScriptableObject, ISerializable
{
	public string name;
	public List<CustomDataSheetDef> sheets;
	
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
/*
		switch (type)
		{
			case CustomDataType.number:
				float[] newValues = new float[values.Length];
				for (var i = 0; i < newValues.Length; i++)
					newValues[i] = float.Parse(values[i]);
				info.AddValue("data", newValues);
				break;
			case CustomDataType.boolean:
				bool[] boolValues = new bool[values.Length];
				for (var i = 0; i < boolValues.Length; i++)
					boolValues[i] = bool.Parse(values[i]);
				info.AddValue("data", boolValues);
				break;
			default:
				info.AddValue("data", values);
				break;
		}
*/		
	}
}


[Serializable]
public enum CustomDataType
{
	text,
	number,
	color,
	boolean,
	customAssetImage,
	customAssetVTT,
	customAssetTextFile,
	customAssetSound
}

public abstract class CustomDataContent
{
	public abstract override string ToString();

}


public abstract class CustomDataFieldDef¡
{
	public CustomDataType type;
	public bool required;
	public string name;
	public int size;
	public string minimumResolution;
	public List<T> data;
}

[Serializable]
public class CustomDataColumnDef 
{
	public CustomDataType type;
	public bool required;
	public string name;
	public int size;
	public string minimumResolution;
	private string[] values;
	
	public CustomDataColumnDef(CustomDataType type, bool required, string name)
	{
		this.type = type;
		this.required = required;
		this.name = name;
	}

	public void AddValue(string value)
	{
		
	}

	
}

[Serializable]
public enum CustomDataSheetEditMode
{
	Rewrite, Append
}

[Serializable]
public class CustomDataSheetDef
{
	public string name;
	public CustomDataSheetEditMode editMode;
	public List<CustomDataColumnDef> columns;
}





[Serializable]
public class BrandPair
{
	public int brandA;
	public int brandB;	
	public string colorDark;
	public string colorMid;
	public string colorLight;
	public string bgImage;
	public string bgImageTiling;
	public string controllerSelectTeamText;
	public string gameSupportTeamText;

	public BrandPair()
	{
		
	}

	
	
	public BrandPair(int brandA, int brandB, string colorDark, string colorMid, string colorLight)
	{
		this.brandA = brandA;
		this.brandB = brandB;
		this.colorDark = colorDark;
		this.colorMid = colorMid;
		this.colorLight = colorLight;		
		bgImage = "";
		bgImageTiling = "";
		controllerSelectTeamText = "TAP TO SELECT YOUR TEAM";
		gameSupportTeamText = "TO SUPPORT YOUR TEAM!";

	}

	public BrandPair(int brandA, int brandB, string colorDark, string colorMid, string colorLight, string bgImage, string bgImageTiling, string controllerSelectTeamText, string gameSupportTeamText)
	{
		this.brandA = brandA;
		this.brandB = brandB;
		this.colorDark = colorDark;
		this.colorMid = colorMid;
		this.colorLight = colorLight;
		this.bgImage = bgImage;
		this.bgImageTiling = bgImageTiling;
		this.controllerSelectTeamText = controllerSelectTeamText;
		this.gameSupportTeamText = gameSupportTeamText;
	}
}


[Serializable]
public class CategoryData
{
	public int categoryID;	
	public string categoryName;
	public string spinnerObject;
	// Deprecated name, synonymous with spinnerObject
	public string categoryObject;

	public string SpinnerObject
	{
		get { return (string.IsNullOrEmpty(spinnerObject)) ? categoryObject : spinnerObject; }
	}

	public CategoryData(int id, string name, string obj)
	{
		categoryID = id;
		categoryName = name;		
		categoryObject = obj;
		spinnerObject = obj;
	}
}


[Serializable]
public class SequenceDefinition
{
	public int sequenceID;	
	public string gameplayVTTSlug;
	public string sequenceName;

	public SequenceDefinition(int sequenceID, string gameplayVTTSlug, string sequenceName)
	{
		this.sequenceID = sequenceID;
		this.gameplayVTTSlug = gameplayVTTSlug;
		this.sequenceName = sequenceName;
	}
}


[Serializable]
public class SponsorData
{
	public int sponsorID;
	public string sponsorName;
	public string sponsorLogoSlug;
	public string colorDark;
	public string colorMid;
	public string colorLight;	
	public string bgImage;
	public string bgImageTiling;

	public SponsorData(){}
	public SponsorData(int sponsorID, string sponsorName, string colorDark, string colorMid, string colorLight, string sponsorLogoSlug, string bgImage, string bgImageTiling)
	{
		this.sponsorID = sponsorID;
		this.sponsorName = sponsorName;
		this.sponsorLogoSlug = sponsorLogoSlug;
		this.colorDark = colorDark;
		this.colorMid = colorMid;
		this.colorLight = colorLight;
		this.bgImage = bgImage;
		this.bgImageTiling = bgImageTiling;	
	}
	

}


[Serializable]
public class LoginType
{
	public int loginTypeID;
	public string loginTypeName;

	public LoginType(int loginTypeId, string name)
	{
		loginTypeID = loginTypeId;
		loginTypeName = name;
	}
}


[Serializable]
public class BrandData
{
	public int brandID;
	public string brandName;	
	public string brandObject;	
	public string colorLight;
	public string colorMid;
	public string colorDark;
	public int category;
	public string winningSFX;
	

	public BrandData(){}

	public BrandData(int brandId, string brandName, string brandObject, string colorLight, string colorMid, string colorDark, int category,string winningSfx)
	{
		brandID = brandId;
		this.brandName = brandName;		
		this.brandObject = brandObject;
		winningSFX = winningSfx;
		
		this.colorLight = colorLight;
		this.colorMid = colorMid;
		this.colorDark = colorDark;
	
		this.category = category;
	}
	
	
}

[Serializable]
public class SponsorCta
{
	public int id;
	public int sponsorID;
	public string url;
	public string image;


	public SponsorCta(int id, int sponsorId, string url, string image)
	{
		this.id = id;
		sponsorID = sponsorId;
		this.url = url;
		this.image = image;
	}

}


[Serializable]
public class GetGameData
{
	public string slug;
	public string key;
	public string update_channel = "dev";

	public GetGameData()
	{
                
	}
            
	public GetGameData(string game_slug, string data_key)
	{	
		slug = game_slug;
		key = data_key;
	}
}