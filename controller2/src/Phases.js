import {Component} from "react";
import React from "react";
import {GameManager} from './MyController';


class Wait extends Component {

    render(){
        return(
        <div>
            WAITING...
        </div>);
    }
}

class Play extends Component {

    click(){
        console.log("DOING A THING");
        GameManager.commands.playerAction();
    }

    render(){
        return(
            <div>
                <button onClick={() => this.click()} className="button" style={{marginTop:"50px", height:"70px"}}>
                    DO A THING
                </button>
            </div>);
    }
}

export {Wait, Play}