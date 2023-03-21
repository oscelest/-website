import {DialogEvent, useDialog} from "@noxy/react-dialog";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useEffect} from "react";
import {Guild} from "../../../entity/Guild";
import {subscriptionSocket, subscriptionUser} from "../../../Globals";
import {CreateGuildDialog} from "../Dialog/CreateGuildDialog";
import Style from "./HomeScene.module.scss";

export const HomeScene = (props: HomeSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [socket] = useSubscription(subscriptionSocket);
  const [user] = useSubscription(subscriptionUser);
  const [, createDialog] = useDialog();
  
  useEffect(
    () => {
      if (!socket) return;
    },
    [socket]
  );
  
  useEffect(() => {
    if (!user) return;
    
    Guild.self()
    .then((guild) => {
      console.log(guild);
      if (guild === null) {
        createDialog({
          dismissible: false,
          closeable: false,
          onClose: (a: DialogEvent<Guild>) => {
            if (!a.value) return;
            console.log(a.value)
          },
          children: (
            <CreateGuildDialog/>
          )
        });
      }
    });
    
  }, [user]);
  
  return (
    <div {...component_props} className={classes}>
    
    </div>
  );
};

export interface HomeSceneProps extends HTMLComponentProps {
  children?: never;
}
