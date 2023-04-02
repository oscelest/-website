import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useEffect} from "react";
import {Guild} from "../../../entity/Guild";
import {Unit} from "../../../entity/Unit";
import {subscriptionGuild} from "../../../Globals";
import {ManagementScreen} from "../Screen/ManagementScreen";
import {UnitPill} from "../UnitPill";
import Style from "./RecruitmentScene.module.scss";

export const RecruitmentScene = (props: RecruitmentSceneProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  const [guild, setGuild] = useSubscription(subscriptionGuild);
  
  useEffect(() => {
    Unit.load().then(unitList => setGuild(new Guild({...guild!.toJSON(), unitList})));
  }, []);
  
  return (
    <div {...component_props} className={classes}>
      <ManagementScreen>
        <span>Recruitment page</span>
        <div>
          <Button onSubmit={onRecruitmentRefreshClick}>Refresh</Button>
        </div>
        <div className={Style.UnitList}>
          {guild?.unit_list.map(renderUnitList)}
        </div>
      </ManagementScreen>
    </div>
  );
  
  function renderUnitList(unit: Unit, index: number = 0) {
    return (
      <UnitPill key={index} unit={unit}/>
    );
  }
  
  async function onRecruitmentRefreshClick() {
    const unitList = await Unit.refreshUnitList();
    console.log(unitList);
    setGuild(new Guild({...guild!.toJSON(), unitList}));
  }
};

export interface RecruitmentSceneProps extends HTMLComponentProps {
  children?: never;
}
