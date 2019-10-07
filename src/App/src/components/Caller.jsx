import React, { useState, useEffect } from 'react'
import { RadioGroup, 
    Radio,
    Button,
    HTMLSelect,
    Divider
} from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';
import ResultsPanel from './ResultsPanel';
import { ipcRenderer } from 'electron';

const useStyles = makeStyles({
    root: {
        margin: '0 1rem 1rem 0',
        border: "2px solid gray",
        display: 'flex',
        flexDirection: 'column'
    },
    radio:{
        display: 'flex',
        justifyContent: 'space-around',
        marginTop: '0.3rem'
    },
    controls: {
        display: 'flex',
        justifyContent: 'space-around'
    },
    btn: {
        display: 'block',
        width: '35%'
    },
    htmtselect:{
        width: '60%'
    },
    resultsPane:{
        display: 'grid',
        gridTemplateColumns: 'auto auto'     
    },
    logs:{
        display: 'flex',
       justifyContent: 'space-between',
       boxShadow: "inset 0 0 10px #000000", 
       margin: '0 0.5rem 0 0.5rem'
    },
    spn:{
        display:"block",
        margin: '0 0.5rem 0 0.5rem'
    }
  });

export default function Caller({users}) {
    const classes = useStyles();
    
    const services = ['Voice', 'Data', 'Video'];

    const [service, setservice] = useState(services[0]);
    const [mobileTerminalId, setmobileTerminal] = useState(null)
    const [results, setresults] = useState({})
    const handleCall = () => {
        const args = {
            mobileTerminalId,
            service
        }
        ipcRenderer.send('predict', args);
        ipcRenderer.on('prediction_results', (event, response) => {
            console.log(response)
            setresults({...response})
        })
    }

    useEffect(() => {
        return () => {
            ipcRenderer.removeAllListeners('prediction_results');
        };
    }, [results])

    return (
        <div className={classes.root}>
            <Divider/>
            <RadioGroup className={classes.radio}
                label="SELECT SERVICE"
                onChange={event => setservice(event.target.value)}
                selectedValue={service}    
            >
                {services.map((service, index) => (
                    <Radio label={service} value={service} key={index}/>
                ))}
            </RadioGroup>
            <Divider/>
            <div className={classes.controls}>
                <HTMLSelect className={classes.htmtselect}
                    onChange={event=>setmobileTerminal(event.target.value)}
                >
                    {users.map((user, index) => {
                    return <option value={user} key={index}>{user}</option>
                    })}
                </HTMLSelect>
                <Button text="Call" className={classes.btn} onClick={handleCall}/>
            </div>
            <Divider/>
            <div className={classes.resultsPane}>
                <ResultsPanel label="Service" value={service}/>
                <ResultsPanel label="UserId" value={mobileTerminalId}/> 
                <ResultsPanel label="Predicted State" value={results['nextState']}/>
                <ResultsPanel label="Frequency" value={results['frequency']}/> 
            </div>
            <Divider/>
            <States frequencyDictionary={results['frequencyDictionary']}/>
        </div>
    )
}

function States({frequencyDictionary}) {
    const classes = useStyles();
    let list = []
    if(frequencyDictionary !== undefined)
        list = Object.keys(frequencyDictionary);
    return <div  className={classes.logs}>
        {list.map((key,index) => (
            <div key={index}>
                <span className={classes.spn}>State: {key}</span>
                <span className={classes.spn}> Frequency: {frequencyDictionary[key]}</span>
            </div>
        ))}
    </div>
}

