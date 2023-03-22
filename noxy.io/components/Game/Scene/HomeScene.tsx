import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {SceneType} from "../../../enums/SceneType";
import {subscriptionScene} from "../../../Globals";
import {ManagementScreen} from "../Screen/ManagementScreen";
import Style from "./HomeScene.module.scss";

export const HomeScene = (props: HomeSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [, setScene] = useSubscription(subscriptionScene);
  
  return (
    <div {...component_props} className={classes}>
      <div className={Style.Header}></div>
      <ManagementScreen>
        <div>
          <Button onSubmit={setScene} value={SceneType.RECRUITMENT}>Recruitment</Button>
          <Button onSubmit={setScene} value={SceneType.CRAFTING}>Crafting</Button>
          <Button onSubmit={setScene} value={SceneType.SHOP}>Shop</Button>
        </div>
      </ManagementScreen>
    </div>
  );
  
};

export interface HomeSceneProps extends HTMLComponentProps {
  children?: never;
}
