import React,{ useState } from 'react';
import { HTMLSelect } from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';

const useStyles = makeStyles({
    root: {
       margin: "auto"
    },
    htmselect:{
        marginLeft: "0.8rem"
    }
})

export default function Config() {
    const classes = useStyles();
    const initialCapacity = { c1:100, c2:100, c3:100, c4:100 }
    const [capacity, setcapacity] = useState(initialCapacity);

    const handleChangedCapacity = event => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        setcapacity(prevState=>({ ...prevState, [name]:value }));
    }

    const items = [1,2,3,4];

    return (
        <div className={classes.root}>
            {items.map( value => {
                let name = `RAT${value}`
                return <div key={value}>
                    <HTMLSelect onChange={handleChangedCapacity} 
                        name={name} 
                        defaultValue={100}
                        className={classes.htmselect} >
                        <option value={25}>25</option>
                        <option value={50}>50</option>
                        <option value={75}>75</option>
                        <option value={100}>100</option>
                    </HTMLSelect>
                </div>
            })}
        </div>
    )
}
