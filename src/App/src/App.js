import React, { useState, useEffect } from 'react';
import { ipcRenderer } from "electron";
import Appbar from './components/Appbar';
import Calls from './components/Calls';
import GraphsPanel from './components/GraphsPanel';
import Caller from './components/Caller';
import ResultLog from './components/ResultLog';

const App = () => {
  const [results, setResults] = useState({
    data:null,
    isLoading: true
  })
  
  const run = () => {
    setResults({ data:null, isLoading: true })
    console.log('fetching results')
    ipcRenderer.send('results');
    ipcRenderer.on('res', (event, response) => {
      console.log(response)
      setResults({data: response, isLoading: false})
    })
  }

  //component will unmount
  useEffect(() => {
    return () => {
      ipcRenderer.removeAllListeners('greeting')
    }
  }, [results]);

  return (
    <div className="App">
     <Appbar/>
     <Calls run={run}/>
     <Caller/>
     <GraphsPanel {...results}/>
     <ResultLog {...results}/>
    </div>
  );
}

export default App;
