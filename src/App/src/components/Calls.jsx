import React from 'react'
import { FormGroup, 
    InputGroup,
    Button, 
    Divider} from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
        display: "flex",
        justifyContent: "start",
        border: "1px solid black",
        margin: "1rem 1rem 0"
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
        fontSize: "0.7rem"
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
                    icon="walk"
                    text="Run"
                    large={true}
                />
            </div>
            <Divider/>
            <div className={classes.results}>
              
            </div>
        </div>
    )
}
