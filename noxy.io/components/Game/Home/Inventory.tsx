import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import Style from "./Inventory.module.scss";

export const Inventory = (props: InventoryProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <div>Inventory</div>
    </div>
  );
};

export interface InventoryProps extends HTMLComponentProps {
  children?: never;
}
