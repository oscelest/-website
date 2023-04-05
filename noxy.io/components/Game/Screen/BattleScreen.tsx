import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import Style from "./ManagementScreen.module.scss";

export const BattleScreen = (props: BattleScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      {children}
    </div>
  );
};

interface BattleScreenProps extends HTMLComponentProps {

}
