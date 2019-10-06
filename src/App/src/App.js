import React, { useState, useEffect } from 'react';
import { ipcRenderer } from "electron";
import Appbar from './components/Appbar';
import Calls from './components/Calls';
import GraphsPanel from './components/GraphsPanel';
import Caller from './components/Caller';
import ResultLog from './components/ResultLog';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
  root: {
    boxSizing: 'border-box',
    marginTop: '0',
    marginBottom: '1rem'

  },
  simulations: {
    display: "grid",
    gridTemplateColumns: '1fr 1fr'
  }
})


const App = () => {
  const classes = useStyles();
  const [results, setResults] = useState({
    data: null,
    isLoading: true,
    init: true,
  })
  const [users, setusers] = useState([]);
  
  useEffect(() => {
    ipcRenderer.send('getusers');
    ipcRenderer.on('getusers', (event, response) => {
      setusers([...response]);
    })
    return () => {
      ipcRenderer.removeAllListeners('results');
      ipcRenderer.removeAllListeners('getusers');
    }
  },[]);

  const run = (calls, capacity) => {
    console.log(capacity)
    setResults({ data:null, isLoading: true , init: false })
    console.log('fetching results')
    ipcRenderer.send('results',{calls, capacity});
    ipcRenderer.on('res', (event, response) => {
      console.log(response)
      setResults({init: false, data: response, isLoading: false})
    })
  }

  return (
    <div className={classes.root}>
     <Appbar/>
     <div className={classes.simulations}>
      <Calls run={run}/>
      <Caller users={users}/>
     </div>
     <GraphsPanel {...results}/>
     <ResultLog {...results}/>
    </div>
  );
}

export default App;
