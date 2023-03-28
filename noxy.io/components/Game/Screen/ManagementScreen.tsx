import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {SceneType} from "../../../enums/SceneType";
import {subscriptionScene} from "../../../Globals";
import {Inventory} from "../Home/Inventory";
import {Roster} from "../Home/Roster";
import Style from "./ManagementScreen.module.scss";

export const ManagementScreen = (props: ManagementScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [scene, setScene] = useSubscription(subscriptionScene);
  
  return (
    <div {...component_props} className={classes}>
      <Roster/>
      <div className={Style.Content}>
        <div className={Style.Navigation}>
          <Button onSubmit={setScene} value={SceneType.HOME} disabled={scene === SceneType.HOME}>Home</Button>
          <Button onSubmit={setScene} value={SceneType.RECRUITMENT} disabled={scene === SceneType.RECRUITMENT}>Recruitment</Button>
          <Button onSubmit={setScene} value={SceneType.CRAFTING} disabled={scene === SceneType.CRAFTING}>Crafting</Button>
          <Button onSubmit={setScene} value={SceneType.SHOP} disabled={scene === SceneType.SHOP}>Shop</Button>
        </div>
        {children}
      </div>
      <Inventory/>
    </div>
  );
};

interface ManagementScreenProps extends HTMLComponentProps {

}
