import React from 'react'
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
        border: "1px solid blue",
        margin: "1rem 1rem 0",
        height: "9rem",
        fontSize: "11pt",
        fontFamily: "Inconsolata, monospace",
        color: "white",
        backgroundColor: "#151515",
        opacity:"0.8"
    },
  });

export default function ResultLog({data}) {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            {JSON.stringify(data, null, 2)}
        </div>
    )
}
