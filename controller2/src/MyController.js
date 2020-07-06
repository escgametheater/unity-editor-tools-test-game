import React, {Component} from 'react';

import {ESCManager, EventManager, ReducerManager }from "@esc_games/esc-controller-sdk";
import {NetworkDisconnectController, EVENT_NETWORK_ERROR} from '@esc_games/esc-controller-sdk/networking/ESCNetworking';
import {Wait, Play} from "./Phases";

const GAME_ID = "SampleProject";
const GAME_NAME = "Game:"+GAME_ID;

ESCManager.networking.gameId = GAME_ID ;
const ACTION_JOINED_GAME = GAME_NAME + ":JOIN_GAME";
const ACTION_PHASE_CHANGED = GAME_NAME + ":PHASE_CHANGED";

const REQUEST_JOIN_GAME = GAME_NAME + ":JOIN_GAME";
const REQUEST_PLAYER_ACTION = GAME_NAME + ":PLAYER_ACTION";

const CU_JOIN_GAME = "CU_JoinGame";
const CU_PLAYER_ACTION = "CU_PlayerAction";
const UC_PHASE_CHANGE = "UC_PhaseChange";

const PHASE_WAIT = 'WAIT';
const PHASE_PLAY = 'PLAY';



const defaultState = {
    player: {
        phase: PHASE_WAIT,
        level:0,
        points:0
    },
    joined: false
}

const reducerManager = new ReducerManager({
        [ACTION_JOINED_GAME]: (state) => {
            return {
                ...state,
                joined: true
            }
        },
        [ACTION_PHASE_CHANGED]: (state, action) => {
            return {
                ...state,
                player : action.value.playerData
            }
        }
    }, defaultState
);


ESCManager.networking.registerEventHandler(REQUEST_JOIN_GAME, GAME_NAME, (message) => {
    console.log("CU_JOIN_GAME");
    ESCManager.networking.sendCommand(CU_JOIN_GAME, message);
});

ESCManager.networking.registerEventHandler(REQUEST_PLAYER_ACTION, GAME_NAME, (message) => {
    console.log("CU_PLAYER_ACTION");
    ESCManager.networking.sendCommand(CU_PLAYER_ACTION, message);
});


ESCManager.networking.registerEventHandler(UC_PHASE_CHANGE, GAME_NAME, (message) => {
    console.log("UC_PHASE_CHANGE", message);

    console.dir(message);
    GameManager.dispatchUI(ACTION_PHASE_CHANGED, message);
});


const GameManager = new EventManager(GAME_ID, reducerManager);
GameManager.commands = {

    joinGame : () => {
        console.log("JOIN GAME");
        ESCManager.networking.dispatchEvent(REQUEST_JOIN_GAME);
    },

    playerAction : () => {
        console.log("PLAYER ACTION");
        //
        ESCManager.networking.dispatchEvent(REQUEST_PLAYER_ACTION, {points: 1});
    }

};

class SampleGameComponent extends Component{



    render(){
        console.log("HEY!");
        console.dir(this.props);

        if (this.props.joined === false) {
            GameManager.commands.joinGame();
            GameManager.dispatchUI(ACTION_JOINED_GAME, true);
        }



        let html = null;

        switch (this.props.player.phase) {
            case PHASE_WAIT:
                html = (<Wait/>);
                break;
            case PHASE_PLAY:
                html = (<Play/>);
                break;
            default:
                html = (<Wait/>);
                break;
        }
        return (
            <div>
                {html}
                <NetworkDisconnectController/>
            </div>
        )

    }
}



const MyController = GameManager.connect(SampleGameComponent, [
    ACTION_PHASE_CHANGED,
    ACTION_JOINED_GAME
]);


export {GameManager, MyController, ACTION_PHASE_CHANGED,
    ACTION_JOINED_GAME}