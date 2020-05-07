import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Stepper from '@material-ui/core/Stepper';
import Container from '@material-ui/core/Container';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import StepContent from '@material-ui/core/StepContent';
import Button from '@material-ui/core/Button';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import { TextField } from '@material-ui/core';
import { debounce } from "debounce";
import { backendHost } from "../common"

export default class SecretConstructor extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            email: null,
            secret: null,
            activeStep: 0,
            otp: null
        }
    }

    setEmail(e) {
        this.setState({ email: e });
    }

    setSecret(s) {
        this.setState({ secret: s });
    }

    submit = async () => {
        let url = backendHost() + "secrets";
        let payload = {
            plainText: this.state.secret,
            notificationDestination: this.state.email
        };
        let response = await fetch(url, {
            method: "POST",
            body: JSON.stringify(payload),
            headers: { 'Access-Control-Allow-Origin': '*' }
        }).then(async (data) => {
            console.log(data);
            let respBody = await data.json();
            console.log(respBody);
            this.setState({ otp: respBody.oneTimePasscode });
        });
        this.setState({ activeStep: this.state.activeStep + 1 });
    };

    getSteps() {
        return ['Enter the email of the person you want to send a secret to', 'Enter the secret to send', 'Send the secret.'];
    }

    getStepContent(step) {
        switch (step) {
            case 0:
                return `The recipient will get an email with a link to unlock their secret.
                To ensure only they can see your secret they will have to enter
                an 8 digit code that will be generated once you submit the secret. You will have to send this code to them separately.`;
            case 1:
                return `While we only store the encrypted value of this secret our goal is get rid of it as soon as possible.
                We delete all trace of the secret as soon as the recipient gets it. We will also delete the secret if three incorrect attempts to unlock the secret are made.
                If the recipient does not unlock the secret in 24 hours then everything will be deleted regardless.`;
            case 2:
                return `Once submitted, an email will be sent to the address and an 8 digit code will appear that you should copy and send
                to the recipient over a DIFFERENT channel. Do not send the code to the same email address in order to ensure security.`;
            default:
                return 'Unknown step';
        }
    }

    getStepInput(step, email, secret) {
        switch (step) {
            case 0:
                return (<TextField id="outlined-basic-email" label="Recipient Email" variant="outlined" value={email} onChange={this.handleEmailInput} />);
            case 1:
                return (<TextField id="outlined-basic-secret" label="Secret" variant="outlined" type="password" value={secret} onChange={this.handleSecretInput} />);
            default:
                return;
        }
    }

    handleEmailInput = (e) => {
        this.setState({ email: e.target.value });
    };

    handleSecretInput = (e) => {
        this.setState({ secret: e.target.value });
    };

    handleNext = () => {
        this.setState({ activeStep: this.state.activeStep + 1 });
    };

    handleBack = () => {
        this.setState({ activeStep: this.state.activeStep - 1 });
    };

    handleReset = () => {
        this.setState({ activeStep: 0, secret: null, email: null });
    };

    renderStepper() {
        let steps = this.getSteps();
        return (
            <div >
                <Stepper activeStep={this.state.activeStep} orientation="vertical">
                    {steps.map((label, index) => (
                        <Step key={label}>
                            <StepLabel>{label}</StepLabel>
                            <StepContent>
                                <Typography>{this.getStepContent(index)}</Typography>
                                <div >
                                    <div>
                                        <br />
                                        {this.getStepInput(index, this.state.email, this.state.secret)}
                                        <br />
                                        <Button
                                            disabled={this.state.activeStep === 0}
                                            onClick={this.handleBack}
                                        >
                                            Back
                                    </Button>
                                        <Button
                                            variant="contained"
                                            color="primary"
                                            onClick={this.state.activeStep === steps.length - 1 ? this.submit : this.handleNext}
                                        >
                                            {this.state.activeStep === steps.length - 1 ? 'Submit & Send' : 'Next'}
                                        </Button>
                                    </div>
                                </div>
                            </StepContent>
                        </Step>
                    ))}
                </Stepper>
                {this.state.activeStep === steps.length && (
                    <Paper square elevation={0} >
                        <Typography>The secret is submitted. Send this code to the recipient.</Typography>
                        <Typography variant="h3" gutterBottom>{this.state.otp}</Typography>
                        <Button onClick={this.handleReset} >
                            Reset
                        </Button>
                    </Paper>
                )}
            </div>
        );
    }

    render() {
        return (
            <Container>
                {this.renderStepper()}
            </Container>
        );
    }
}

