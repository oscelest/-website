import {Button} from "@noxy/react-button";
import {Loader} from "@noxy/react-loader";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import Image from "next/image";
import React, {useState} from "react";
import {Mission} from "../../../entity/Mission";
import Style from "./UnitPill.module.scss";

export const MissionItem = (props: MissionItemProps) => {
  const {className, mission, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  
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
          <Button className={Style.RecruitButton} value={unit} onSubmit={onRecruitSubmit}>
            <Loader className={Style.RecruitLoader} loading={loading}>
              Recruit
            </Loader>
          </Button>
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
    setLoading(true);
    await Unit.recruit(unit);
    setLoading(false);
    setUnitData({loading: true});
    const value = await Unit.load();
    setUnitData({loading: false, value});
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

export interface MissionItemProps extends HTMLComponentProps {
  children?: never;
  mission: Mission;
}
