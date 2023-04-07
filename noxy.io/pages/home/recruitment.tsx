import {Button} from "@noxy/react-button";
import {Loader} from "@noxy/react-loader";
import {useSubscription} from "@noxy/react-subscription-hook";
import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React, {useEffect} from "react";
import {UnitPill} from "../../components/Game/UnitPill";
import {Unit} from "../../entity/Unit";
import {GuildStateType} from "../../enums/GuildStateType";
import {subscriptionUnitList} from "../../Globals";
import {i18n} from "../../next-i18next.config";
import Style from "./recruitment.module.scss";

export async function getStaticProps({locale}: GetStaticPropsContext) {
  return {
    props: {
      state: GuildStateType.HOME,
      ...(await serverSideTranslations(locale ?? i18n.defaultLocale, ["common"]))
    }
  };
}

const HomeRecruitmentPage: NextPage = () => {
  const [unit_data, setUnitData] = useSubscription(subscriptionUnitList);
  const unit_list = unit_data.value?.filter(x => !x.recruited) ?? [];
  
  useEffect(
    () => {
      setUnitData({loading: true});
      
      Unit.load()
      .then(value => setUnitData({loading: false, value}))
      .catch(() => setUnitData({loading: false}));
    },
    []
  );
  
  return (
    <div className={Style.Component}>
      <span>Recruitment page</span>
      <div>
        <Button onSubmit={onRecruitmentRefreshClick}>Refresh</Button>
      </div>
      <div className={Style.UnitList}>
        <Loader loading={unit_data.loading}>
          {unit_list.map(renderUnitList)}
        </Loader>
      </div>
    </div>
  );
  
  async function onRecruitmentRefreshClick() {
    setUnitData({loading: true});
    setUnitData({loading: false, value: await Unit.refreshAvailable()});
  }
};


function renderUnitList(unit: Unit, index: number = 0) {
  return (
    <UnitPill key={index} unit={unit}/>
  );
}

export default HomeRecruitmentPage;


