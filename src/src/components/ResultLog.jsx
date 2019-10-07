import React from 'react'
import { makeStyles } from '@material-ui/styles';
import { Divider } from '@blueprintjs/core';

const useStyles = makeStyles({
    root: {
        border: "1px solid blue",
        margin: "1rem 1rem 0",
        fontSize: "11pt",
        fontFamily: "Inconsolata, monospace",
        color: "#b9a341",
        backgroundColor: "#012354",
        fontWeight: "bolder",
        opacity:"0.8"
    },
    rootLoading:{
        border: "1px solid blue",
        margin: "1rem 1rem 0",
        fontSize: "11pt",
        fontFamily: "Inconsolata, monospace",
        color: "#b9a341",
        backgroundColor: "#012354",
        fontWeight: "bolder",
        opacity:"0.5",
        overflow: "scroll",
        height: "10rem"
    },
    terminal:{
        
    }
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
            {data.map((value, index) => (
                <div key={index}>
                    <span>Number of calls:  {value['calls']}  | </span>
                    <span>Number of sessions:  {value['totalSessions']}  | </span>
                    <span>Number of predictive VHO:  {value['predictiveHandovers']}  | </span>
                    <span>Number of non predictive:  {value['nonPredictiveHandovers']} | </span>
                    <span>Number of predictive Blocked Calls:  {value['predictiveBlockedCalls']} | </span>
                    <span>Percentage Video VHOs Avoided:  {value['videoAvoided']}% | </span>
                    <span>Percentage Data VHOs Avoided:  {value['dataAvoided']}% | </span>
                    <span>Percentage Voice VHOs Avoided:  {value['voiceAvoided']}% | </span>
                    <span>Percentage Total VHOs Avoided:  {value['totalAvoided']}% | </span>
                    <Divider/>
                </div>
            ))}
        </div>
    )
}
