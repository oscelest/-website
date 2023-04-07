import {useSubscription} from "@noxy/react-subscription-hook";
import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React, {useEffect} from "react";
import {UnitPill} from "../components/Game/UnitPill";
import {Unit} from "../entity/Unit";
import {subscriptionUnitList} from "../Globals";
import {i18n} from "../next-i18next.config";
import Style from "./recruitment.module.scss";

export async function getStaticProps({locale}: GetStaticPropsContext) {
  return {
    props: {
      ...(await serverSideTranslations(locale ?? i18n.defaultLocale, ["common"]))
    }
  };
}

const RecruitmentPage: NextPage = () => {
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
      <span>Mission page</span>
    </div>
  );
};

export default RecruitmentPage;


