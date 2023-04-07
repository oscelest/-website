import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React from "react";
import {GuildStateType} from "../../enums/GuildStateType";
import {i18n} from "../../next-i18next.config";
import Style from "./index.module.scss";

export async function getStaticProps({locale}: GetStaticPropsContext) {
  return {
    props: {
      state: GuildStateType.HOME,
      ...(await serverSideTranslations(locale ?? i18n.defaultLocale, ["common"]))
    }
  };
}

const BattleIndexPage: NextPage = () => {
  return (
    <div className={Style.Component}>
      Battle
    </div>
  );
};

export default BattleIndexPage;
