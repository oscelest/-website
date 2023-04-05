import {trackSubscription, useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import {useRouter} from "next/router";
import React from "react";
import {Guild} from "../../entity/Guild";
import {subscriptionGuild, subscriptionUser} from "../../Globals";
import {Authorization} from "../Authorization/Authorization";
import Style from "./GameWindow.module.scss";
import {BattleScreen} from "./Screen/BattleScreen";
import {ManagementScreen} from "./Screen/ManagementScreen";

export const GameWindow = (props: ScreenProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
  const router = useRouter();
  const [guild, setGuild] = useSubscription(subscriptionGuild);
  
  trackSubscription(subscriptionUser, async auth => {
    if (!auth.value) return;
    setGuild({loading: true});
    setGuild({loading: false, value: await Guild.load()});
  });
  
  trackSubscription(subscriptionGuild, guild2 => {
    if (guild2.loading) return;
    if (guild2.value && guild.value?.state !== guild2.value.state) {
      if (guild2.value.state === 0) {
        return router.push("/");
      }
      if (guild2.value.state === 1) {
        return router.push("/battle");
      }
    }
    return router.push("/welcome");
  });
  
  return (
    <div {...component_props} className={classes}>
      <Authorization>
        {renderState(guild.value?.state)}
      </Authorization>
    </div>
  );
  
  function renderState(state?: number | null) {
    switch (state) {
      case 0:
        return <ManagementScreen>{children}</ManagementScreen>;
      case 1:
        return <BattleScreen>{children}</BattleScreen>;
      default:
        return children as JSX.Element;
    }
  }
};

export interface ScreenProps extends HTMLComponentProps {

}
