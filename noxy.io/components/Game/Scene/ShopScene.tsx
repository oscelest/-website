import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {Inventory} from "../Home/Inventory";
import {Roster} from "../Home/Roster";
import Style from "./HomeScene.module.scss";

export const ShopScene = (props: ShopSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <Roster/>
      <div className={Style.Home}>
      
      </div>
      <Inventory/>
    </div>
  );
};

export interface ShopSceneProps extends HTMLComponentProps {
  children?: never;
}
