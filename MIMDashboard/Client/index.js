/* 
    ./client/index.js
*/
import React from 'react';
import ReactDOM from 'react-dom';
import Dashboard from './components/Dashboard.tsx';
import configureStore from './Components/PlayerStats/store/configureStore';
import './Sass/style.scss';

const store = configureStore(); 

ReactDOM.render(
    <Provider store={store}>
         <Dashboard />, 
    </Provider>,
document.getElementById('root')
);
