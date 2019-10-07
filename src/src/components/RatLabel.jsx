import React from 'react'
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
       
    },
    label:{
        color:"blue"
    }
})

export default function RatLabel({label}) {
    const classes = useStyles();  
    return (
        <div className={classes.root}>
            <span className={classes.label}>{label}</span>
        </div>
    )
}


