import React from 'react';
import { Navbar, Alignment } from '@blueprintjs/core';

export default function Appbar() {
    return (
        <div>
            <Navbar>
                <Navbar.Group align={Alignment.LEFT}>
                    <Navbar.Heading>NET. SIMULATOR</Navbar.Heading>
                </Navbar.Group>
            </Navbar>
        </div>
    )
}