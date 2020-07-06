using System;
using System.Collections;
using System.Collections.Generic;
using Esc;
using Esc.api;
using Esc.api.Responses;
using UnityEngine;

public class TESTUploadRequest
{
	private TESTUploadPayload Req;
	private ApiV1Response<UploadResponse> Resp;

	public bool HasResponse
	{
		get
		{
			return this.Resp != null;
		}
	}

	public ApiV1Response<UploadResponse> data
	{
		get
		{
			return this.Resp;
		}
	}

	public TESTUploadRequest(
		string token,
		string slug,
		string version,
		string fileID,
		string filePath,
		Action<UploadResponse> callback,
		Action errorCallback)
	{
		TESTUploadRequest uploadRequest = this;
		this.Req = new TESTUploadPayload(slug, version, fileID, filePath);
		new ApiInvoker<TESTUploadPayload, UploadResponse>("https://api.esc.games/v1/games-builds/upload-build", token).invoke(this.Req, (Action<ApiV1Response<UploadResponse>>) (response =>
		{
			uploadRequest.Resp = response;
			Debug.Log((object) ("UploadResponse: " + (object) uploadRequest.Resp.payload.results[0]));
			callback(uploadRequest.Resp.payload.results[0]);
		}), errorCallback);
	}
}

[Serializable]
public class TESTUploadPayload
{
	public string slug;
	public string game_build_version;
	public string upload_id_file;
	public string file;
	public string update_channel = "dev";

	public TESTUploadPayload()
	{
	}

	public TESTUploadPayload(string slug, string version, string uploadID, string file)
	{
		this.slug = slug;
		this.game_build_version = version;
		this.upload_id_file = uploadID;
		this.file = file;
	}
}