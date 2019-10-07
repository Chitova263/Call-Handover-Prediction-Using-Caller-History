import React from 'react'
import { makeStyles } from '@material-ui/styles';
import RatLabel from './RatLabel';
import { HTMLSelect } from '@blueprintjs/core';

const useStyles = makeStyles({
    root: {
       margin: 'auto',
       width: '90%'
    },
    htmlselect:{
        width: '100%'
    },
    label:{
       
        
    }
})

export default function RatConfig({label, handleChangedCapacity, name}) {
    const classes = useStyles();
    return (
        <div className={classes.root}>
           
            <RatLabel label={label}/>
            <HTMLSelect onChange={handleChangedCapacity} 
                name={name}
                defaultValue={100}
                className={classes.htmlselect}
            >
                <option value={25}>25</option>
                <option value={50}>50</option>
                <option value={75}>75</option>
                <option value={100}>100</option>
            </HTMLSelect>
        </div>
    )
}
