import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';


export default class Layout extends React.Component {
    render() {
        console.log(this.props);
        return (
            <div>
                <AppBar position="static">
                    <Toolbar>
                        <Typography variant="h6" >
                            Send-A-Secret
                        </Typography>
                    </Toolbar>
                </AppBar>
                {React.cloneElement(this.props.children, this.props)}
            </div>
        );
    }
}