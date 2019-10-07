import React, { useState} from 'react'
import { FormGroup,  InputGroup, Button, Divider } from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';
import Config from './Config';

const useStyles = makeStyles({
    root: {
        display: 'flex',
        flexDirection: 'column', 
        margin: '0 1rem 1rem 1rem',
        border: "2px solid gray",
    },
    btn:{
        margin:"auto",
        width: "95%"
    },
    form:{
        
    },
    input:{
        width:"24rem"
    },
    container:{
        display: 'flex',
        margin: "0.5rem 0.1rem auto 0.5rem",
    },
    label:{
        margin: "0.4rem 0.5rem 0 0.5rem",
        color:"blue"
    } 
  });

export default function Calls({run}) {
    const classes = useStyles();
    const initialState = "100,200,300,400,500,600,700"
    
    const [calls, setcalls] = useState(initialState)
    const initialCapacity = { c1:100, c2:100, c3:100, c4:100 }
    const [capacity, setcapacity] = useState(initialCapacity);

    const handleChangedCapacity = event => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        setcapacity(prevState=>({ ...prevState, [name]:value }));
    }

    return (
        <div className={classes.root}>
            <div className={classes.container}>
                <div className={classes.label}>CALLS</div>
                <FormGroup className={classes.form}
                    labelFor="num-of-calls"
                    inline
                >
                    <InputGroup id="num-of-calls" className={classes.input}
                        onChange={event => setcalls(event.target.value)}
                        placeholder="100,200,300,400,500,600"
                        value={calls} 
                    />   
                </FormGroup>
            </div>
            
            <Config handleChangedCapacity={handleChangedCapacity}/> 
            <Divider/>  
            <Button className={classes.btn} onClick={()=>run(calls, capacity)} text="Run"/>
            <Divider/>
        </div>
    )
}
