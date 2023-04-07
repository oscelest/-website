import {Button} from "@noxy/react-button";
import {InputField, InputFieldChangeEvent} from "@noxy/react-input-field";
import {useSubscription} from "@noxy/react-subscription-hook";
import {GetStaticPropsContext, NextPage} from "next";
import {serverSideTranslations} from "next-i18next/serverSideTranslations";
import React, {useState} from "react";
import {Guild} from "../entity/Guild";
import {subscriptionGuild} from "../Globals";
import {i18n} from "../next-i18next.config";
import Style from "./index.module.scss";

export async function getStaticProps({locale}: GetStaticPropsContext) {
  return {
    props: {
      state: null,
      ...(await serverSideTranslations(locale ?? i18n.defaultLocale, ["common"]))
    }
  };
}

const IndexPage: NextPage = () => {
  const [value, setValue] = useState<string>("");
  const [, setGuild] = useSubscription(subscriptionGuild);
  
  return (
    <div className={Style.Component}>
      <div className={Style.Header}>Create your Guild</div>
      <div className={Style.Content}>
        <InputField label={"Name of the guild"} value={value} onChange={onInputChange}/>
        <Button onClick={onClick}>Create</Button>
      </div>
    </div>
  );
  
  async function onClick() {
    setGuild({value: await Guild.register(value)});
  }
  
  function onInputChange(event: InputFieldChangeEvent) {
    setValue(event.value);
  }
};

export default IndexPage;
