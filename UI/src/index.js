import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import * as serviceWorker from './serviceWorker';
import { BrowserRouter, HashRouter } from 'react-router-dom';
import ConfigLoader from 'Components/ConfigLoader';
import ScrollToTop from 'Components/ScrollToTop';

ReactDOM.render(
  <HashRouter>
    <ScrollToTop />
    <ConfigLoader ready={(config) => <App config={config} />} />
  </HashRouter>, 
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
