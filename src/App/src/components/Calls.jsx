import React from 'react'
import { FormGroup, 
    InputGroup,
    Button } from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
        display: "flex",
        justifyContent: "start",
        border: "2px solid blue",
        margin: "1rem 1rem 0",
        boxShadow: "inset 0 0 10px #000000",
    },
    form:{
        margin: "0.5rem 0.5rem"
    },
    button:{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-around",
        margin: "0.5rem 0.5rem"
    },
    results:{
        fontSize: "0.7rem",
        marginRight: "0.5rem"
    },
    rightPane:{
        display: "flex",
        justifyContent: "space-between",
        margin: "auto 0.5rem auto auto",
        fontFamily: "Inconsolata, monospace",
        fontWeight: "bolder",
    }
  });

export default function Calls({run}) {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <FormGroup className={classes.form}
                helperText="enter comma separated number of calls"
                label="Calls"
                labelFor="num-of-calls"
                inline
            >
                <InputGroup id="num-of-calls" 
                    placeholder="100,200,300,400,500" 
                />   
            </FormGroup>
            <div className={classes.button}>
                <Button 
                    onClick={run}
                    text="Run"
                    large={true}
                />
            </div>
            <div className={classes.rightPane}>
                <div className={classes.results}>
                    <div>RAT-1 - Capacity: 100; Services:[voice] </div>
                    <div>RAT-2 - Capacity: 100; Services:[data]</div>
                    <div>RAT-3 - Capacity: 100; Services:[voice, data] </div>
                    <div>RAT-4 - Capacity: 100; Services:[voice, data, video]</div>
                </div>
                <div className={classes.results}>
                    <div>Voice - 1 bbu</div>
                    <div>Data - 2 bbu</div>
                    <div>Video - 2 bbu</div>
                </div>
            </div>
            
        </div>
    )
}
