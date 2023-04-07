import {Button} from "@noxy/react-button";
import {Loader} from "@noxy/react-loader";
import {useSubscription} from "@noxy/react-subscription-hook";
import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React, {useEffect} from "react";
import {Mission} from "../../entity/Mission";
import {GuildStateType} from "../../enums/GuildStateType";
import {subscriptionMissionList} from "../../Globals";
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

const HomeMissionPage: NextPage = () => {
  const [mission_list, setMissionList] = useSubscription(subscriptionMissionList);
  const current_mission_list = mission_list.value?.filter(x => !x.time_started) ?? [];
  const available_mission_list = mission_list.value?.filter(x => x.time_started) ?? [];
  
  useEffect(
    () => {
      setMissionList({loading: true});
      
      Mission.load()
      .then(value => setMissionList({loading: false, value}))
      .catch(() => setMissionList({loading: false}));
    },
    []
  );
  
  return (
    <div className={Style.Component}>
      <div className={Style.MissionList}>
        <Button onSubmit={onRecruitmentRefreshClick}>Refresh</Button>
        <div className={Style.Current}>
          {current_mission_list.map(renderCurrentMission)}
        </div>
        <div className={Style.Separator}/>
        <div className={Style.Available}>
          {available_mission_list.map(renderCurrentMission)}
        </div>
      </div>
      <div className={Style.MissionSetup}>
        <div  ></div>
      </div>
    </div>
  );
  
  function renderCurrentMission() {
  
  }
  
  function renderMissionList() {
    return null;
  }
  
  async function onRecruitmentRefreshClick() {
    setMissionList({loading: true});
    setMissionList({loading: false, value: await Mission.refreshAvailable()});
  }
};

export default HomeMissionPage;
