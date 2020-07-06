using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Esc;
using UnityEngine;
using UnityEditor;

namespace Test
{
	[Serializable]
	public class BuildInfo
	{
		public string zipPath;
		public string buildPath;
		public string version;
	}
	
	public class Test : EditorWindow
	{
		public const string tokenURL = "https://api.esc.games/v1/auth/request-token/";
		public const string dataURLNEW = "https://api.esc.games/v1/games-data/get/";
		public const string dataURL = "https://api.esc.games/v1/games/get-data/";
		public const string uploadURL = "https://api.esc.games/v1/games-builds/upload-build";
		public const string listURL = "https://api.esc.games/v1/games/list/";

		private const string tokenKey = "<<<REDACTED>>>";

		private static string _token;
		public static string token
		{
			get
			{
				if (!String.IsNullOrEmpty(_token)) return _token;
				
				string savedToken = EditorPrefs.GetString(tokenKey);
				if (String.IsNullOrEmpty(savedToken))
					Debug.LogError("Please login to esc.games in the ESCConnect window to access game data.");

				_token = savedToken;
				return _token;
			}
		}
		
		private const string gameIDKey = "esc_game_id";
		
		private static string _gameID;
		public static string gameID
		{
			get
			{
				if (!String.IsNullOrEmpty(_gameID)) return _gameID;
				string savedGameID = EditorPrefs.GetString(gameIDKey);
				_gameID = savedGameID;
				return _gameID;
			}
		}

		private string username;
		private string password;


		
		private Esc.UserData user;
		private BuildInfo gameBuild = new BuildInfo();
		private BuildInfo adminControllerBuild = new BuildInfo();
		private BuildInfo playerControllerBuild = new BuildInfo();
		private bool inheritControllers;
		private bool inheritCustomData;
		private bool inheritCustomAssets;

		private bool isRequestingAuth;
		private bool isLoggedIn;
		private bool authFailed;
		private LoginResponse resp;

		private GameData gameData;
		private Texture avatarTexture = null;
		private WWWEditor www;
		Vector2 scrollPos = Vector2.zero;

		[MenuItem("TEST/ESCConnect")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(Test));
		}
		bool[] selectedAssets;
		private bool overwriteWhenDowloadingAssets;

	
		void OnGUI()
		{
			if (!isLoggedIn)
			{
				GUILayout.Label("TEST ESC Games Login", EditorStyles.boldLabel);

				if (!isRequestingAuth)
				{
					username = EditorGUILayout.TextField("Username", username);
					password = EditorGUILayout.PasswordField("Password", password);
					if (GUILayout.Button("Submit"))
					{
						isRequestingAuth = true;
						authFailed = false;
						AuthRequest authRequest = new AuthRequest(username, password, OnLoggedIn, OnLoginFailed);
					}
				}
				else
				{
					GUILayout.Label("Logging into... " + username, EditorStyles.boldLabel);
					if (GUILayout.Button("Reset"))
					{
						isRequestingAuth = false;
					}
				}
				
				if (authFailed)
				{
					GUILayout.Label("Login failed!", EditorStyles.whiteLargeLabel);
				}
			}
			else
			{
				GUILayout.Label("TEST ESC Games", EditorStyles.boldLabel);
				
				if (resp != null)
					GUILayout.Label("Welcome, " + resp.user.display_name, EditorStyles.label);

				if (GUILayout.Button("Logout", GUILayout.Width(60)) || resp == null)
				{
					Logout();
					return;
				}

				DisplayLine();
				
				GUILayout.BeginHorizontal();
				// Set Game ID
				string gid = EditorGUILayout.TextField("Game ID", gameID, GUILayout.Width(300));
				if (_gameID != gid)
				{
					_gameID = gid;
					EditorPrefs.SetString(gameIDKey, gameID);
				}

				if (Game.Instance != null && GUILayout.Button("Get ID From Scene", GUILayout.Width(120)))
				{
					_gameID = Game.Instance.GameSlug;
					EditorPrefs.SetString(gameIDKey, gameID);
				}
				
				GUILayout.EndHorizontal();
				
				DisplaySpacer();

				DisplayBuildInfo();
				//DisplayBuildInfo(ref adminControllerBuild, "Game Admin Controller");
				//DisplayBuildInfo(ref playerControllerBuild, "Player Controller");
				
				GUILayout.Label("Sync Custom Assets:", EditorStyles.label);

				DisplayCustomAssets();
						
				if (avatarTexture != null)
				{
					float padding = 10;
					float size = 50;
					float x = position.width - size - padding;
					float y = padding;
					EditorGUI.DrawPreviewTexture(new Rect(x, y, size, size), avatarTexture);
				}
			}

		}

#region Callback Functions
		private void OnLoggedIn(LoginResponse resp)
		{
			isLoggedIn = true;
			authFailed = false;
			this.resp = resp;
			
			// Load avatar
			user = resp.user;
			string url = resp.user.avatar_tiny_url;
			www = new WWWEditor(url, OnAvatarLoaded);
			
			EditorPrefs.SetString(tokenKey, resp.token);
			
			
		}

		private void OnGameDataSuccess(GameData resp)
		{
			Debug.Log("OnGameDataSuccess YES\n");
			gameData = resp;
			selectedAssets = new bool[gameData.game_assets.Length];
		}

		private void OnGameDataFailed()
		{
			Debug.Log("OnGameDataFailed FAILED\n");
		}

		private void OnLoginFailed()
		{
			Debug.LogError("ESC Login failed!");
			Logout();
			authFailed = true;
		}
		
		private void OnAvatarLoaded(WWW www)
		{
			avatarTexture = www.texture;
		}
#endregion

		private void Logout()
		{
			isRequestingAuth = false;
			isLoggedIn = false;
			authFailed = false;
			username = "";
			password = "";
			avatarTexture = null;
			www = null;
			resp = null;
			EditorPrefs.DeleteKey(tokenKey);
		}

		private void DisplayCustomAssets()
		{
			EditorGUILayout.BeginVertical("Textfield");

			GUILayout.Label($"Manage Custom Assets for slug {gameID}", EditorStyles.label);

			if (GUILayout.Button("Get Game Data", GUILayout.Width(200)))
			{				
				TESTGameDataRequest testGameDataRequest = new TESTGameDataRequest(token, gameID,
					OnGameDataSuccess, OnGameDataFailed);
			}


			if (gameData == null || gameData.game_assets.Length == 0) return;

			
			//----------------------------------  DOWNLOAD ASSETS
			GUILayout.BeginHorizontal();
			
			overwriteWhenDowloadingAssets = EditorGUILayout.Toggle("Overwrite Local Custom Assets", overwriteWhenDowloadingAssets);

			if (GUILayout.Button("Download Selected Assets", GUILayout.Width(200)))
			{
				
			}

			GUILayout.EndHorizontal();

			//----------------------------------  DISPLAY ASSETS			
			GUILayout.Label($"Custom Assets: {gameData.game_assets.Length}", EditorStyles.label);

			int[] colWidths = {250, 50, 150, 50};
			GUILayout.BeginHorizontal();
			GUILayout.Label("Slug", GUILayout.Width(colWidths[0]));
			GUILayout.Label("FileType", GUILayout.Width(colWidths[1]));
			GUILayout.Label("Create Time", GUILayout.Width(colWidths[2]));
			GUILayout.Label("Select", GUILayout.Width(colWidths[3]));
			GUILayout.EndHorizontal();


			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Width(500),
				GUILayout.Height(500));

			for (int i = 0; i < gameData.game_assets.Length; i++)
			{
				GameAsset gameAsset = gameData.game_assets[i];

				GUILayout.BeginHorizontal();

				GUILayout.Label(gameAsset.slug, GUILayout.Width(colWidths[0]));
				GUILayout.Label(gameAsset.extension, GUILayout.Width(colWidths[1]));
				GUILayout.Label(gameAsset.create_time, GUILayout.Width(colWidths[2]));

				selectedAssets[i] = EditorGUILayout.Toggle("", selectedAssets[i], GUILayout.Width(colWidths[3]));
				GUILayout.EndHorizontal();


			}

			EditorGUILayout.EndScrollView();


			EditorGUILayout.EndVertical();
			DisplaySpacer();
		}

		private void DisplayBuildInfo()
		{
			EditorGUILayout.BeginVertical("Textfield");

			GUILayout.Label($"Upload Settings", EditorStyles.boldLabel);
			
			inheritControllers = EditorGUILayout.Toggle("Inherit controllers", inheritControllers);
			inheritCustomData = EditorGUILayout.Toggle("Inherit custom data", inheritCustomData);
			inheritCustomAssets = EditorGUILayout.Toggle("Inherit custom assets", inheritCustomAssets);
			GUILayout.Label($"Upload file: {(gameBuild.zipPath == null? "nope" : gameBuild.zipPath)}", GUILayout.Width(600));
			gameBuild.version = EditorGUILayout.TextField("Build version: ", gameBuild.version, GUILayout.Width(400));
			
			if (!String.IsNullOrEmpty(gameBuild.zipPath) && !String.IsNullOrEmpty(gameBuild.version))
			{
							
				if (GUILayout.Button("Upload " + gameBuild + " Build", GUILayout.Width(300)))
				{
				
					TESTUploadRequest uploadRequest =
						new TESTUploadRequest(token, gameID, gameBuild.version, gameID, gameBuild.zipPath, null, null);
					
				}
			}
			
			
			GUILayout.Label($"Build Game", EditorStyles.boldLabel);
			
		
			//------------------------- BUILD INFO
			
			
			GUILayout.BeginHorizontal();
			gameBuild.buildPath = EditorGUILayout.TextField("Build path: ", BuildManager.FULL_BUILD_DIR, GUILayout.Width(400));
			if (GUILayout.Button("Change Dir", GUILayout.Width(80)))
				gameBuild.buildPath = EditorUtility.OpenFilePanel("Overwrite with zip",Application.dataPath,"");
			GUILayout.EndHorizontal();
			
			
			if (GUILayout.Button("Build Game", GUILayout.Width(300)))
			{
				gameBuild.zipPath = BuildManager.generateBuild();
				Debug.Log($"Built thing {gameBuild.zipPath}\n");
			}
						

			GUILayout.Label($"   --- or ---", EditorStyles.boldLabel);
			

			if (GUILayout.Button("Select zip file", GUILayout.Width(300)))
			{
				gameBuild.zipPath = EditorUtility.OpenFilePanel("Overwrite with zip", "", "zip");
			}
			
			
			
			
			EditorGUILayout.EndVertical();
			DisplaySpacer();
		}

		private void DisplaySpacer()
		{
			EditorGUILayout.Space();
		}
		
		private void DisplayLine()
		{
			EditorGUILayout.Space();

			int height = 1;
			Rect rect = EditorGUILayout.GetControlRect(false, height);
			rect.height = height;
			EditorGUI.DrawRect(rect, new Color (0.5f,0.5f,0.5f,1));
			
			EditorGUILayout.Space();
		}
		
	}
}