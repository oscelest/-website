import {Button} from "@noxy/react-button";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import Image from "next/image";
import React from "react";
import {RoleLevel} from "../../entity/RoleLevel";
import {Unit} from "../../entity/Unit";
import {subscriptionUnitList} from "../../Globals";
import Style from "./UnitPill.module.scss";

export const UnitPill = (props: UnitPillProps) => {
  const {className, unit, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  const [, setUnitData] = useSubscription(subscriptionUnitList);
  const [affinity_list = [], profession_list = []] = unit.getRoleLevelListSet();
  
  return (
    <div {...component_props} className={classes}>
      <div className={Style.Header}>
        <div className={Style.Title}>
          <span className={Style.Name}>Name: {unit.name}</span>
          <span className={Style.Experience}>Level {unit.getLevel()}</span>
        </div>
        <div className={Style.Recruit}>
          <div className={Style.RecruitCost}>
            {unit.getCost()}
            <Image className={Style.RoleIcon} src={`/img/currency.png`} alt="" width={24} height={24}/>
          </div>
          <Button className={Style.RecruitButton} value={unit} onSubmit={onRecruitSubmit}>Recruit</Button>
        </div>
      </div>
      <div className={Style.RoleList}>
        <div className={Style.RoleAffinityList}>
          {affinity_list.map(renderRoleLevel)}
        </div>
        <div className={Style.RoleProfessionList}>
          {profession_list.map(renderRoleLevel)}
        </div>
      </div>
    </div>
  );
  
  async function onRecruitSubmit(unit: Unit) {
    setUnitData({loading: true});
    await Unit.recruit(unit);
    setUnitData({loading: false, value: await Unit.load()});
  }
  
  function renderRoleLevel(level: RoleLevel, index: number = 0) {
    return (
      <div className={Style.Role} key={index}>
        <Image className={Style.RoleIcon} src={`/img/${level.role.getIcon()}`} alt="" width={48} height={48} title={level.role.name}/>
        <div className={Style.RoleExperience}>{level.getLevel()}</div>
      </div>
    );
  }
};

export interface UnitPillProps extends HTMLComponentProps {
  children?: never;
  unit: Unit;
}
