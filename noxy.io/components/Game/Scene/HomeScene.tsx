import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {subscriptionSocket} from "../../../Globals";
import Style from "./HomeScene.module.scss";

export const HomeScene = (props: HomeSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  const [socket] = useSubscription(subscriptionSocket);
  
  return (
    <div {...component_props} className={classes}>
    
    </div>
  );
  
};

export interface HomeSceneProps extends HTMLComponentProps {
  children?: never;
}
