import React from 'react';
import { Navbar, Alignment } from '@blueprintjs/core';
import { makeStyles } from '@material-ui/styles';
import logoapp from './logoapp.png';

const useStyles = makeStyles({
    root: {
        boxSizing: 'border-box',
        marginTop: '0',
        marginBottom: '1rem',
    },
    heading: {
        marginLeft: '10rem',
        fontSize: '1.5rem'
    },
})

export default function Appbar() {
    const classes = useStyles();
    return (
        <div className={classes.root}>
            <Navbar>
                <Navbar.Group align={Alignment.CENTER}>
                    <img src={logoapp} alt=""/>
                    <Navbar.Heading className={classes.heading}>
                        NET. SIMULATOR 
                    </Navbar.Heading>  
                </Navbar.Group>
            </Navbar>
        </div>
    )
}