import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React from "react";
import {Guild} from "../../../entity/Guild";
import {subscriptionGuild} from "../../../Globals";
import {ManagementScreen} from "../Screen/ManagementScreen";
import {UnitPill} from "../UnitPill";
import Style from "./RecruitmentScene.module.scss";

export const RecruitmentScene = (props: RecruitmentSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  const [guild, setGuild] = useSubscription(subscriptionGuild);
  
  return (
    <div {...component_props} className={classes}>
      <ManagementScreen>
        <span>Recruitment page</span>
        <div>
          <Button onSubmit={onRecruitmentRefreshClick}>Refresh</Button>
        </div>
        <div className={Style.UnitList}>
          {guild?.unit_list.map(unit => <UnitPill unit={unit}/>)}
        </div>
      </ManagementScreen>
    </div>
  );
  
  async function onRecruitmentRefreshClick() {
    const unitList = await Guild.refreshUnitList();
    setGuild(new Guild({...guild!.toJSON(), unitList}));
  }
};

export interface RecruitmentSceneProps extends HTMLComponentProps {
  children?: never;
}
