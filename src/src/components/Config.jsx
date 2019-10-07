import React from 'react';
import { makeStyles } from '@material-ui/styles';
import RatConfig from './RatConfig';

const useStyles = makeStyles({
    root: {
        display: 'grid',
        gridTemplateRows:'auto auto',
        gridTemplateColumns: '1fr 1fr',   
    },
})

export default function Config({handleChangedCapacity}) {
    const classes = useStyles();
    
    const values = [1,2,3,4];
    return (
        <div className={classes.root}>
          {values.map((value, index) => {
              return <RatConfig key={index}
                name={`c${index+1}`}
                handleChangedCapacity={handleChangedCapacity} 
                label={`RAT-${value}`}
                />
          })}
        </div>
    )
}

