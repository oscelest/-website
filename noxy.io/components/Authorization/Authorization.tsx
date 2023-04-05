import {Loader} from "@noxy/react-loader";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps} from "@noxy/react-utils";
import React, {useEffect} from "react";
import {User} from "../../entity/User";
import {LocalStorageKeyType} from "../../enums/LocalStorageKeyType";
import {subscriptionUser} from "../../Globals";
import Style from "./Authorization.module.scss";
import {LogIn} from "./LogIn";
import {SignUp} from "./SignUp";

export const Authorization = (props: AuthorizationProps) => {
  const {className, children, ...component_props} = props;
  const [user, setUser] = useSubscription(subscriptionUser);
  
  useEffect(
    () => {
      const token = localStorage.getItem(LocalStorageKeyType.JWT);
      if (token && user.loading !== false && !user.value) {
        setUser({loading: true});
        User.refresh(token)
        .then((value) => setUser({loading: false, value}))
        .catch(() => setUser({loading: false}));
      }
      else {
        setUser({loading: false});
      }
    },
    []
  );
  
  if (user.value) return children as JSX.Element;
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      <Loader loading={user.loading !== false}>
        <LogIn/>
        <SignUp/>
      </Loader>
    </div>
  );
  
};

export interface AuthorizationProps extends HTMLComponentProps {

}
