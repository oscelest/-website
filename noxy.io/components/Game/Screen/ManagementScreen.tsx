import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {Inventory} from "../Home/Inventory";
import {Roster} from "../Home/Roster";
import Style from "../Scene/HomeScene.module.scss";

export const ManagementScreen = (props: ManagementScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <Roster/>
      {children}
      <Inventory/>
    </div>
  );
};

interface ManagementScreenProps extends HTMLComponentProps {

}
