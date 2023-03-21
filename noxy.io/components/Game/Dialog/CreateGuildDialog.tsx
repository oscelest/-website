import {Button} from "@noxy/react-button";
import {DialogContext} from "@noxy/react-dialog";
import {InputField, InputFieldChangeEvent} from "@noxy/react-input-field";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useContext, useState} from "react";
import {Guild} from "../../../entity/Guild";
import Style from "./CreateGuildDialog.module.scss";

export const CreateGuildDialog = (props: CreateGuildDialogProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const context = useContext(DialogContext);
  const [value, setValue] = useState<string>("");
  
  return (
    <div {...component_props} className={classes}>
      <div className={Style.Header}>Create your Guild</div>
      <div className={Style.Content}>
        <InputField label={"Name of the guild"} value={value} onChange={onInputChange}/>
        <Button onClick={onClick}>Create</Button>
      </div>
    </div>
  );
  
  async function onClick() {
    context.close(await Guild.register(value));
  }
  
  function onInputChange(event: InputFieldChangeEvent) {
    setValue(event.value);
  }
};

interface CreateGuildDialogProps extends HTMLComponentProps {

}
