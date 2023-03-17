import {useDialog} from "@noxy/react-dialog";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useEffect} from "react";
import {subscriptionSocket} from "../../../Globals";
import {CreateGuildDialog} from "../Dialog/CreateGuildDialog";
import Style from "./HomeScene.module.scss";

export const HomeScene = (props: HomeSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [socket] = useSubscription(subscriptionSocket);
  console.log(useDialog);
  console.log(useDialog());
  const [, createDialog] = useDialog();
  
  useEffect(
    () => {
      if (!socket) return;
      socket.on("Load", onLoad);
      
      socket.send("Load").catch(err => console.error(err));
    },
    [socket]
  );
  
  return (
    <div {...component_props} className={classes}>
    
    </div>
  );
  
  function onLoad(guild: object | null) {
    if (guild === null) {
      createDialog({children: (<CreateGuildDialog/>)})
    }
    else {
      console.log(guild);
    }
  }
  
};

export interface HomeSceneProps extends HTMLComponentProps {
  children?: never;
}
