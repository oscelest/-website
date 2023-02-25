import {DetailedHTMLProps, HTMLAttributes} from "react";
import Style from "./Loader.module.scss";

export function Loader(props: LoaderProps) {
  const {active, className, children, ...component_props} = props;
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  if (active === false) {
    return <>{children}</>;
  }
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      Loading
    </div>
  );
}


type HTMLComponentProps = DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>

export interface LoaderProps extends Omit<HTMLComponentProps, "onChange"> {
  active?: boolean;
}
