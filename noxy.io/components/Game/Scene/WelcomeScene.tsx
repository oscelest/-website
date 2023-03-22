import {Button} from "@noxy/react-button";
import {InputField, InputFieldChangeEvent} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useState} from "react";
import {Guild} from "../../../entity/Guild";
import {SceneType} from "../../../enums/SceneType";
import {subscriptionGuild, subscriptionScene} from "../../../Globals";
import Style from "./WelcomeScene.module.scss";

export const WelcomeScene = (props: HomeSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [value, setValue] = useState<string>("");

  const [, setScene] = useSubscription(subscriptionScene);
  const [guild, setGuild] = useSubscription(subscriptionGuild);
  if (guild) {
    setScene(SceneType.HOME);
    return null;
  }
  
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
    setGuild(await Guild.register(value))
    setScene(SceneType.HOME);
  }
  
  function onInputChange(event: InputFieldChangeEvent) {
    setValue(event.value);
  }
};

export interface HomeSceneProps extends HTMLComponentProps {
  children?: never;
}
