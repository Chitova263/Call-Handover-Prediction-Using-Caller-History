import React, { useState } from 'react';
import './App.css';
import { getResults } from './Actions';

const App = () => {
  const [results, setResults] = useState("")
  
  const load = () => {
    getResults().then(response => {
      console.log(response);
      setResults(response);
    });
  }
  return (
    <div className="App">
      Sample
      {JSON.stringify(results)}
      <button onClick={load}>Get</button>
    </div>
  );
}

export default App;
