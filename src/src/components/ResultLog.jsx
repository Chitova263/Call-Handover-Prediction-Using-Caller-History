import React from 'react'
import { makeStyles } from '@material-ui/styles';
import { HTMLTable } from '@blueprintjs/core';

const useStyles = makeStyles({
    root: {
        border: "1px solid gray",
        margin: "1rem 1rem 0",
        fontSize: "11pt",
        fontFamily: "Inconsolata, monospace",
        fontWeight: "bolder",
        overflowX: 'auto',
        whiteSpace: 'nowrap'
    },
    rootLoading:{
        border: "1px solid gray",
        margin: "1rem 1rem 0",
        fontSize: "11pt",
        fontFamily: "Inconsolata, monospace",
        fontWeight: "bolder",
        height: "10rem"
    },
  });

export default function ResultLog({data, isLoading}) {
    const classes = useStyles();
    if(isLoading){
        return  <div className={classes.rootLoading}>
             >>>
        </div>
    }
    return (
        <div className={classes.root}>
           <HTMLTable bordered={true} condensed={true} interactive={true} striped={true}>
           <thead>
                <tr>
                    <th>#</th>
                    <th>Total # Calls</th>
                    <th>Total # Sessions</th>
                    <th># Voice Calls</th>
                    <th># Data Calls</th>
                    <th># Video Calls</th>
                    <th># Handovers Predictive Algorithm</th>
                    <th># Handovers Non Predictive Algorithm</th>
                    <th># Voice Handovers Predictive Algorithm</th>
                    <th># Data Predictive Algorithm Handovers</th>
                    <th># Voice Handovers Non Predictive Algorithm</th>
                    <th># Data Predictive Non Algorithm Handovers</th>
                    <th>% Handovers Avoided</th>
                    <th>% Voice Handovers Avoided</th>
                    <th>% Data Handovers Avoided</th>
                </tr>
            </thead>
            <tbody>
                {data.map((value, index) =>(
                    <tr key={index}>
                     <td>{index + 1}</td>
                     <td>{value['calls']}</td>
                     <td>{value['totalSessions']}</td>
                     <td>{value['voiceCalls']}</td>
                     <td>{value['dataCalls']}</td>
                     <td>{value['videoCalls']}</td>
                     <td>{value['predictiveHandovers']}</td>
                     <td>{value['nonPredictiveHandovers']}</td>
                     <td>{value['predictiveVoiceHandovers']}</td>
                     <td>{value['predictiveDataHandovers']}</td>
                     <td>{value['nonPredictiveVoiceHandovers']}</td>
                     <td>{value['nonPredictiveDataHandovers']}</td>
                     <td>{value['totalAvoided']}</td>
                     <td>{value['voiceAvoided']}</td>
                     <td>{value['dataAvoided']}</td>
                   </tr>
                ))}
            </tbody>
           </HTMLTable>
        </div>
    )
}
