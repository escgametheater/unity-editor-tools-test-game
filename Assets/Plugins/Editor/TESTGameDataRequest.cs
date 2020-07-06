using System;
using System.Collections;
using System.Collections.Generic;
using Esc;
using Esc.api;
using Esc.api.Requests;
using Esc.api.Responses;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace Test
{
    public class TESTGameDataRequest
    {


        private GetGameDataQA2 Req;
        private ApiV1Response<GameData> Resp;

        public bool HasResponse
        {
            get { return Resp != null; }
        }

        public ApiV1Response<GameData> data => Resp;

        public TESTGameDataRequest(string token,string slug, Action<GameData> callback, Action errorCallback)
        {
            
            
            new ApiInvoker<Empty, GameData>(Test.listURL, token).invoke(
                new Empty(), response =>
                {
                    Resp = response;
                    foreach (GameData gameData in Resp.payload.results)
                    {
                        if (gameData.slug.Equals(slug))
                        {
                            callback(gameData);
                            return;
                        }
                    }
                    
                    Vector2 scrollPos = Vector2.zero;
                }, errorCallback);

            
            /*

            //get game data by game ID integer - only games you have the right to host
            new ApiInvoker<GetGameData, GameData>(Test.dataURLNEW, token).invoke(
                new GetGameData(13), response =>
                {
                    
                    Resp = response;
                    Debug.Log("GameDataResponse: " + Resp.payload.results[0]);
                    callback(Resp.payload.results[0]);
                }, errorCallback);
*/

            /*
            string game = "shake-it-up";
            string key = "brand-definitions";
            Debug.Log($"TOKEN: {token}\n");
            
            Req = new GetGameDataQA2(game, key);
            new ApiInvoker<GetGameDataQA2, GameDataResponse>("https://api.esc.games/v1/games/get-data/", token).invoke(
                Req, response =>
                {
                    Resp = response;
                    Debug.Log("GameDataResponse: " + Resp.payload.results[0]);
                    callback(Resp.payload.results[0]);
                }, errorCallback);
*/
        }



    }

    [Serializable]
    public class GetGameData
    {
        
        public int id;        

        public GetGameData()
        {
        }
            
        public GetGameData(int id)
        {
            this.id = id;
        }
    }
    
    
    [Serializable]
    public class Empty
    {
        
     

        public Empty()
        {
        }
            
       
    }
    
    [Serializable]
    public class GetGameDataQA2
    {
        
        public string game_slug;
        public string key;
        public string update_channel = "dev";

        public GetGameDataQA2()
        {
        }
            
        public GetGameDataQA2(string slug, string key)
        {
            game_slug = slug;
            this.key = key;
        }
    }

    
    [Serializable]
    public class GameData
    {
        public string type;
        public string id;
        public string slug;
        public string display_name;
        public string description;        
        public string created_by;
        public string modified_by;
        public string deleted_by;

        public GameVersion[] latest_game_versions;

        public GameAsset[] game_assets;

    }

    [Serializable]
    public class GameVersion
    {
        public int game_build_id;
        public string update_channel;
        public string game_build_version;
        public string display_name;
    }

    [Serializable]
    public class GameAsset
    {
        public string type;
        public int id;
        public int game_id;
        public string mime_type;
        public string created_by;
        public string modified_by;
        public int custom_game_asset_id;
        public string filename;
        public string update_channel;
        public int user_id;
        public string extension;
        public string slug;
        public int is_active;
        public string create_time;
        public int is_public;
        public string current_timestamp;
        public string url;        
    }
}