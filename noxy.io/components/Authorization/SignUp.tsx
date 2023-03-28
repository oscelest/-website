import {Button} from "@noxy/react-button";
import {InputField, InputFieldChangeEvent, InputFieldEventType, InputFieldType} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps} from "@noxy/react-utils";
import React, {useState} from "react";
import {ResponseError} from "superagent";
import {User} from "../../entity/User";
import {BadRequestResponse, subscriptionAuth} from "../../Globals";
import Style from "./SignUp.module.scss";

export const SignUp = (props: SignUpProps) => {
  const {className, children, ...component_props} = props;
  const [error, setError] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [email_error, setEmailError] = useState<string>();
  const [password, setPassword] = useState<string>("");
  const [password_error, setPasswordError] = useState<string>("");
  const [confirm, setConfirm] = useState<string>("");
  const [confirm_error, setConfirmError] = useState<string>("");
  const [, setUser] = useSubscription(subscriptionAuth);
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      {error && <span className={Style.Error}>{error}</span>}
      <form className={Style.Form}>
        <InputField type={InputFieldType.EMAIL} label={"Email"} value={email} error={email_error} autoComplete={"email"} onChange={onEmailChange}/>
        <InputField type={InputFieldType.PASSWORD} label={"Password"} value={password} error={password_error} autoComplete={"new-password"} onChange={onPasswordChange}/>
        <InputField type={InputFieldType.PASSWORD} label={"Confirm password"} value={confirm} error={confirm_error} autoComplete={"new-password"} onChange={onConfirmChange}/>
      </form>
      <Button onClick={onSignUpClick}>Sign Up</Button>
    </div>
  );
  
  function onEmailChange(event: InputFieldChangeEvent) {
    setEmail(event.value);
  }
  
  function onPasswordChange(event: InputFieldChangeEvent) {
    setPassword(event.value);
  }
  
  function onConfirmChange(event: InputFieldChangeEvent) {
    setConfirmError("");
    setConfirm(event.value);
    if (event.type === InputFieldEventType.COMMIT && password !== event.value) {
      setConfirmError("Passwords do not match.");
    }
  }
  
  async function onSignUpClick() {
    try {
      const [user, jwt] = await User.signup(email, password);
      setUser({user, jwt});
    }
    catch (error) {
      if (error instanceof Error) {
        const {status, response} = error as ResponseError;
        if (!response) {
          setError("Unknown error occurred");
        }
        else if (status === 400) {
          const {errors: {Email, Password}} = response.body as BadRequestResponse;
          if (Email) setEmailError("Email address must be valid.");
          if (Password) setPasswordError("Password must be between 12 and 256 characters.");
        }
        else if (status === 409) {
          setError("A user with this email already exists.");
        }
      }
      else {
        setError("Unknown error occurred");
      }
    }
  }
};

export interface SignUpProps extends HTMLComponentProps {
  children?: never;
}
