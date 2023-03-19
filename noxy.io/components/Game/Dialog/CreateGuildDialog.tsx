import {Button} from "@noxy/react-button";
import {InputField} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {subscriptionSocket} from "../../../Globals";
import Style from "../../Authorization/LogIn.module.scss";

export const CreateGuildDialog = (props: CreateGuildDialogProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [socket] = useSubscription(subscriptionSocket);
  
  return (
    <div {...component_props} className={classes}>
      <div className={Style.Header}>Create your Guild</div>
      <div className={Style.Content}>
        <InputField label={"Name of the guild"}></InputField>
        <Button onClick={onClick}>Create</Button>
      </div>
    
    </div>
  );
  
  function onClick() {
    
  }
  
};

interface CreateGuildDialogProps extends HTMLComponentProps {

}
