using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO.Compression;
using UnityEditor.SceneManagement;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public static class BuildManager {
	
	public static string FULL_BUILD_DIR = Application.dataPath.Replace("Assets","") + "BUILD";

	public static string generateBuild()
	{

		// Build player.
		//Directory.CreateDirectory()



		Debug.Log($"Generating a build in: {FULL_BUILD_DIR}\n");
		if (Directory.Exists(FULL_BUILD_DIR)) Directory.Delete(FULL_BUILD_DIR, true);
		Directory.CreateDirectory(FULL_BUILD_DIR);

		int sceneCount = SceneManager.sceneCount;
		string[] scenes = new string[sceneCount];


		for (int i = 0; i < sceneCount; i++)
		{
			scenes[i] = SceneManager.GetSceneByBuildIndex(i).path;
		}


		float startTime = Time.realtimeSinceStartup;
		BuildPlayerOptions options = new BuildPlayerOptions();
		options.target = BuildTarget.WebGL;
		options.options = BuildOptions.None;
		options.locationPathName = FULL_BUILD_DIR;
		options.scenes = scenes;

		BuildPipeline.BuildPlayer(options);

		string timeString = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-fff",
			CultureInfo.InvariantCulture);
		string zipFile = "Build." + timeString + ".zip";
		string zipDir = Application.dataPath.Replace("Assets", "");

		if (File.Exists(zipDir + zipFile)) File.Delete(zipDir + zipFile);
		ZipFile.CreateFromDirectory(FULL_BUILD_DIR, zipFile);
		
		
		float totalTime = Time.realtimeSinceStartup - startTime;
		Debug.Log($"Made a build {zipDir + zipFile} in {totalTime} seconds!\n");

		return zipFile;

	}
}
