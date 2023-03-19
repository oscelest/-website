import {Button} from "@noxy/react-button";
import {DialogContext} from "@noxy/react-dialog";
import {InputField, InputFieldChangeEvent} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useContext, useEffect, useState} from "react";
import {subscriptionSocket} from "../../../Globals";
import Style from "./CreateGuildDialog.module.scss";

export const CreateGuildDialog = (props: CreateGuildDialogProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const context = useContext(DialogContext);
  const [socket] = useSubscription(subscriptionSocket);
  const [value, setValue] = useState<string>("");
  
  useEffect(
    () => {
      socket?.on("CreateGuild", onGuildCreate);
      return () => socket?.off("CreateGuild", onGuildCreate);
    },
    []
  );
  
  return (
    <div {...component_props} className={classes}>
      <div className={Style.Header}>Create your Guild</div>
      <div className={Style.Content}>
        <InputField label={"Name of the guild"} value={value} onChange={onInputChange}/>
        <Button onClick={onClick}>Create</Button>
      </div>
    </div>
  );
  
  function onClick() {
    socket?.send("CreateGuild", value);
  }
  
  function onInputChange(event: InputFieldChangeEvent) {
    setValue(event.value);
  }
  
  function onGuildCreate(guild: object) {
    console.log(guild);
    context.close();
  }
  
};

interface CreateGuildDialogProps extends HTMLComponentProps {

}
