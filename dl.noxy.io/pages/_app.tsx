import {AppProps} from "next/app";
import React from "react";
import {Content} from "../components/Layout/Content";
import "../public/style/font.scss";
import "../public/style/globals.scss";
import {Header} from "../components/Layout/Header";

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
