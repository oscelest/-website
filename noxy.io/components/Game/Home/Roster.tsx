import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import Style from "./Roster.module.scss";

export const Roster = (props: RosterProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <div>Roster</div>
    </div>
  );
};

export interface RosterProps extends HTMLComponentProps {
  children?: never;
}
