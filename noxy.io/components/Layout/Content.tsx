import {HTMLComponentProps} from "@noxy/react-utils";
import Style from "./Content.module.scss";
import React from "react";

export function Content(props: ContentProps) {
  const {children, className, ...component_props} = props;
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>{children}</div>
  );
}

export interface ContentProps extends HTMLComponentProps {

}

