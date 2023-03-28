import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {ManagementScreen} from "../Screen/ManagementScreen";
import Style from "./HomeScene.module.scss";

export const CraftingScene = (props: CraftingSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <ManagementScreen>
        <span>Crafting page</span>
      </ManagementScreen>
    </div>
  );
};

export interface CraftingSceneProps extends HTMLComponentProps {
  children?: never;
}
