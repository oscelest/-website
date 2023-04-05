import {Loader} from "@noxy/react-loader";
import {useSubscription} from "@noxy/react-subscription-hook";
import {HTMLComponentProps, sanitizeClassName} from "@noxy/react-utils";
import React, {useEffect} from "react";
import {Unit} from "../../../entity/Unit";
import {subscriptionUnitList} from "../../../Globals";
import {UnitPill} from "../UnitPill";
import Style from "./Roster.module.scss";

export const Roster = (props: RosterProps) => {
  const {className, children, ...component_props} = props;
  const classes = sanitizeClassName(Style.Component, className);
  const [unit_data, setUnitData] = useSubscription(subscriptionUnitList);
  const unit_list = unit_data.value?.filter(x => x.recruited) ?? [];
  
  useEffect(
    () => {
      setUnitData({loading: true});
      Unit.load()
      .then(value => {
        setUnitData({loading: false, value});
      })
      .catch(() => {
        setUnitData({loading: false});
      });
    },
    []
  );
  
  return (
    <div {...component_props} className={classes}>
      <div>Roster</div>
      <Loader loading={unit_data.loading}>
        {unit_list.map(renderUnitList)}
      </Loader>
    </div>
  );
};

export interface RosterProps extends HTMLComponentProps {
  children?: never;
}

function renderUnitList(unit: Unit, index: number = 0) {
  return (
    <UnitPill key={index} unit={unit}/>
  );
}
