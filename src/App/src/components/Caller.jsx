import React, { useState } from 'react'
import { RadioGroup, 
    Radio,
    Button,
    Divider,
    HTMLSelect
} from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
        display: "flex",
        justifyContent: "even",
        border: "2px solid black",
        margin: "1rem 1rem 0"
    },
    form:{
        margin: "0.5rem 0.5rem"
    },
    button:{
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-around",
        margin: "0.5rem 0.8rem 0 2rem"
    },
    results:{
        display: "flex",
        flexDirection: "column",
        justifyContent:"space-around",
        margin: "0.5rem 0.5rem"
    },
    result:{
        marginRight: "0.5rem"
    }
  });

export default function Caller() {
    const classes = useStyles();
    
    const radioValues = {Voice:'Voice', Data:'Data', Video:'Video'};
    const mobileTerminals = { 
        mobileTerminal1:'mobileTerminal1',
        mobileTerminal2:'mobileTerminal2',
        mobileTerminal3:'mobileTerminal3',
    }

    const [radioValue, setradioValue] = useState(radioValues.Voice);
    const [mobileTerminal, setmobileTerminal] = useState(mobileTerminals.mobileTerminal1)
    
    return (
        <div className={classes.root}>
            <div className={classes.form}>
                <RadioGroup 
                    label="Select Service"
                    onChange={event => setradioValue(event.target.value)}
                    selectedValue={radioValue}
                    
                >
                    <Radio label="Voice" value={radioValues.Voice} />
                    <Radio label="Data" value={radioValues.Data} />
                    <Radio label="Video" value={radioValues.Video} />
                </RadioGroup>
                <HTMLSelect 
                    onChange={event=>setmobileTerminal(event.target.value)}
                >
                    <option value={mobileTerminals.mobileTerminal1}>{mobileTerminals.mobileTerminal1}</option>
                    <option value={mobileTerminals.mobileTerminal2}>{mobileTerminals.mobileTerminal2}</option>
                    <option value={mobileTerminals.mobileTerminal3}>{mobileTerminals.mobileTerminal3}</option>
                </HTMLSelect>
            </div>
            <div className={classes.button}>
                <Button text="Call"
                    large={true}
                />
            </div>
            <Divider/>
            <div className={classes.results}>
                <div className={classes.result}>MobileTerminalId: {mobileTerminal} </div>
                <div className={classes.result}>Service:  {radioValue}</div>
                <div className={classes.result}>Predicted State:  </div>
                <div className={classes.result}>Rat Selected:  </div>
                <div className={classes.result}>Successfull Prediction:  </div>
            </div>
        </div>
    )
}
