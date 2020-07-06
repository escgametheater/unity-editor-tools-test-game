import React from 'react';
import ReactDOM from 'react-dom';
import { ESCGameController } from '@esc_games/esc-controller-sdk/';
import { MyController } from './MyController';

ReactDOM.render(
    <ESCGameController game={"sample-game"}
                       includeESCLogo={false}
                       includeTattooDisplay={false}
                       includeTattooSelector={false}
                       desiredOrientation={"portrait"}
                       autoSizeEnabled={true}
                       appModeEnabled={true}
                       nosleep={true}>
        <MyController/>
    </ESCGameController>,
    document.getElementById('root')
);
