import {HTMLAttributes} from "react";
import Style from "./Content.module.scss";

export function Content(props: ContentProps) {
  const {children, className, ...component_props} = props;
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>{children}</div>
  );
}

export interface ContentProps extends HTMLAttributes<HTMLDivElement> {

}

