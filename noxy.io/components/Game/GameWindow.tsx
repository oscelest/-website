import {Loader} from "@noxy/react-loader";
import {trackSubscription, useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {Guild} from "../../entity/Guild";
import {SceneType} from "../../enums/SceneType";
import {subscriptionAuth, subscriptionGuild, subscriptionScene} from "../../Globals";
import {Authorization} from "../Authorization/Authorization";
import Style from "./GameWindow.module.scss";
import {CraftingScene} from "./Scene/CraftingScene";
import {HomeScene} from "./Scene/HomeScene";
import {RecruitmentScene} from "./Scene/RecruitmentScene";
import {ShopScene} from "./Scene/ShopScene";
import {WelcomeScene} from "./Scene/WelcomeScene";

export const GameWindow = (props: ScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const [scene, setScene] = useSubscription(subscriptionScene);
  const [auth] = useSubscription(subscriptionAuth);
  const [, setGuild] = useSubscription(subscriptionGuild);
  
  trackSubscription(subscriptionAuth, async auth => {
    if (!auth.user) return;
    
    const guild = await Guild.self();
    if (guild === null) {
      setScene(SceneType.WELCOME);
    }
    else {
      setGuild(guild);
      setScene(SceneType.HOME);
    }
  });
  
  return (
    <div {...component_props} className={classes}>
      <Loader loading={auth.refreshing}>
        <Authorization>
          <Loader loading={scene === SceneType.NONE}>
            {renderScene(scene)}
          </Loader>
        </Authorization>
      </Loader>
    </div>
  );
  
  function renderScene(scene?: SceneType) {
    switch (scene) {
      case SceneType.HOME:
        return <HomeScene/>;
      case SceneType.WELCOME:
        return <WelcomeScene/>;
      case SceneType.CRAFTING:
        return <CraftingScene/>;
      case SceneType.SHOP:
        return <ShopScene/>;
      case SceneType.RECRUITMENT:
        return <RecruitmentScene/>;
      default:
        return null;
    }
  }
};

export interface ScreenProps extends HTMLComponentProps {
  children?: never;
}
