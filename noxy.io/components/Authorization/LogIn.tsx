import {Button} from "@noxy/react-button";
import {InputField, InputFieldChangeEvent, InputFieldType} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps} from "@noxy/react-utils";
import React, {useState} from "react";
import {ResponseError} from "superagent";
import {User} from "../../classes/User";
import {BadRequestResponse, subscriptionUser} from "../../Globals";
import Style from "./LogIn.module.scss";

export const LogIn = (props: LogInProps) => {
  const {className, children, ...component_props} = props;
  const [error, setError] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [email_error, setEmailError] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [password_error, setPasswordError] = useState<string>("");
  const [, setUser] = useSubscription(subscriptionUser);
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      {error && <span className={Style.Error}>{error}</span>}
      <InputField type={InputFieldType.EMAIL} label={"Email"} value={email} error={email_error} autoComplete={"email"} onChange={onEmailChange}/>
      <InputField type={InputFieldType.PASSWORD} label={"Password"} value={password} error={password_error} autoComplete={"password"} onChange={onPasswordChange}/>
      <Button onClick={onLogInClick}>Log In</Button>
    </div>
  );
  
  function onEmailChange(event: InputFieldChangeEvent) {
    setEmail(event.value);
  }
  
  function onPasswordChange(event: InputFieldChangeEvent) {
    setPassword(event.value);
  }
  
  async function onLogInClick() {
    setError("");
    setEmailError("");
    setPasswordError("");
    
    try {
      const user = await User.login(email, password);
      setUser(user);
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
        else if (status === 401) {
          setError("A user with the given email/password combination does not exist.");
        }
      }
      else {
        setError("Unknown error occurred");
      }
    }
  }
};

export interface LogInProps extends HTMLComponentProps {
  children?: never;
}
