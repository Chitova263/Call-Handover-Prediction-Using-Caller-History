import React from 'react'
import { makeStyles } from '@material-ui/styles';

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
              $>> NET. SIMULATOR.LOGGER
        </div>
    }
    return (
        <div className={classes.root}>
            {JSON.stringify(data, null, 2)}
        </div>
    )
}
