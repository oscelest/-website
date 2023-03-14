import {AppProps} from "next/app";
import React from "react";
import {Content} from "../components/Layout/Content";
import {Header} from "../components/Layout/Header";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  
  return (
    <>
      <Header/>
      <Content>
        <Component {...pageProps}></Component>
      </Content>
    </>
  );
}

export default Application;
