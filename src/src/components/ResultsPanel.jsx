import React from 'react';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root:{
        margin:"0.2rem"
    },
    label:{

    },
    value:{
        color: 'blue'
    }
})


export default function ResultsPanel({label, value}) {
    const classes = useStyles();
    return(
        <div className={classes.root}>
            <span className={classes.label}>{label}:  </span> 
            <span className={classes.value}>{value}</span>
        </div>
    )
}