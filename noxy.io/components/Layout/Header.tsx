import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps} from "@noxy/react-utils";
import React from "react";
import {User} from "../../classes/User";
import {subscriptionUser} from "../../Globals";
import Style from "./Header.module.scss";

export function Header(props: HeaderProps) {
  const {className, ...component_props} = props;
  const [user] = useSubscription(subscriptionUser);
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      {renderUserButton()}
    </div>
  );
  
  function renderUserButton() {
    if (user) {
      return (
        <Button onClick={onLogOutClick}>Log Out</Button>
      );
    }
    
    return (
      <Button>Sign Up / Log In</Button>
    );
  }
  
  function onLogOutClick() {
    User.logout();
  }
}

export interface HeaderProps extends HTMLComponentProps {
  children?: never;
}
