import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React from "react";
import {i18n} from "../next-i18next.config";
import Style from "./index.module.scss";

export async function getStaticProps({locale}: GetStaticPropsContext) {
  return {
    props: {
      ...(await serverSideTranslations(locale ?? i18n.defaultLocale, ["common"]))
    }
  };
}

const IndexPage: NextPage = () => {
  return (
    <div className={Style.Component}>
      Home
    </div>
  );
};

export default IndexPage;
