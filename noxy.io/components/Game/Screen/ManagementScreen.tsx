import {Button} from "@noxy/react-button";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import Link from "next/link";
import React from "react";
import {Inventory} from "../Home/Inventory";
import {Roster} from "../Home/Roster";
import Style from "./ManagementScreen.module.scss";

export const ManagementScreen = (props: ManagementScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  return (
    <div {...component_props} className={classes}>
      <Roster/>
      <div className={Style.Content}>
        <div className={Style.Navigation}>
          <Link href={"/"}>
            <Button>Home</Button>
          </Link>
          <Link href={"/recruitment"}>
            <Button>Recruitment</Button>
          </Link>
          {/*<Button onSubmit={setScene} value={SceneType.RECRUITMENT} disabled={scene === SceneType.RECRUITMENT}>Recruitment</Button>*/}
          {/*<Button onSubmit={setScene} value={SceneType.CRAFTING} disabled={scene === SceneType.CRAFTING}>Crafting</Button>*/}
          {/*<Button onSubmit={setScene} value={SceneType.SHOP} disabled={scene === SceneType.SHOP}>Shop</Button>*/}
        </div>
        <div className={Style.Page}>
          {children}
        </div>
      </div>
      <Inventory/>
    </div>
  );
};

interface ManagementScreenProps extends HTMLComponentProps {

}
