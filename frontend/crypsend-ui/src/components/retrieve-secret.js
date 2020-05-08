import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import TextField from '@material-ui/core/TextField';
import CircularProgress from '@material-ui/core/CircularProgress';
import Visibility from '@material-ui/icons/Visibility';
import Button from '@material-ui/core/Button';
import VisibilityOff from '@material-ui/icons/VisibilityOff';
import { backendHost } from "../common"
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    useParams
} from "react-router-dom";

export default class SecretRetriever extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            otp: null,
            secret: null,
            wrongOtp: false,
            tooManyAttempts: false,
            isLoading: false
        };
    }

    handleOtpInput = (e) => {
        this.setState({ otp: e.target.value });
    };

    onSubmitOtp = async () => {
        this.setState({ isLoading: true });
        let url = backendHost() + "secrets/" + this.props.secretId + "?otp=" + this.state.otp;

        let response = await fetch(url, {
            method: "GET",
            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(async (data) => {
            let respBody = await data.json();
            this.setState({ secret: respBody.plainText, wrongOtp: respBody.requireVerification, tooManyAttempts: respBody.tooManyAttempts, isLoading: false });
        });
    };

    render() {
        let display = null;
        if (this.state.secret) {
            display = (
                <Container>
                    <ExpansionPanel style={{ marginTop: "2rem" }}>
                        <ExpansionPanelSummary
                            expandIcon={<ExpandMoreIcon />}
                            aria-controls="panel1a-content"
                            id="panel1a-header"
                        >
                            <Typography >Secret Unlocked: Expand to View</Typography>
                        </ExpansionPanelSummary>
                        <ExpansionPanelDetails>
                            <Container>
                                <Typography variant="h4">
                                    {this.state.secret}
                                </Typography>
                            </Container>
                        </ExpansionPanelDetails>
                    </ExpansionPanel>
                </Container>
            );
        }
        else if (this.state.tooManyAttempts) {
            return (<Typography variant="h2" color="error">Too many attempts. Secret is deleted. Contact the sender to try again.</Typography>);
        }
        else {
            display = (
                <Container>
                    <Typography style={{ "paddingTop": "50px", "paddingBottom": "30px" }}>Enter the 8 digit code your buddy gave you in order to unlock your secret.</Typography>
                    <TextField style={{ "textAlign": "center" }} id="outlined-basic-code" label="Code" variant="outlined" value={this.state.otp} onChange={this.handleOtpInput} />
                    <br />
                    <br />
                    {this.state.wrongOtp == true ? <Typography color="error">Incorrect code. Try again</Typography> : null}
                    {this.state.isLoading == true ? <CircularProgress /> : <Button color="primary" variant="contained" onClick={this.onSubmitOtp} >Submit</Button>}
                </Container>
            );
        }

        return (
            display
        );
    }
}