import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps} from "@noxy/react-utils";
import React from "react";
import {subscriptionUser} from "../../Globals";
import Style from "./Authorization.module.scss";
import {LogIn} from "./LogIn";
import {SignUp} from "./SignUp";

export const Authorization = (props: AuthorizationProps) => {
  const {className, children, ...component_props} = props;
  const [user] = useSubscription(subscriptionUser);
  if (user) return children as JSX.Element;
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      <LogIn/>
      <SignUp/>
    </div>
  );
};

export interface AuthorizationProps extends HTMLComponentProps {

}
