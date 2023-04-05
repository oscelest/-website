import {useDialog} from "@noxy/react-dialog";
import {AppProps} from "next/app";
import React from "react";
import {GameWindow} from "../components/Game/GameWindow";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  const [dialog] = useDialog();
  
  return (
    <>
      <GameWindow>
        <Component {...pageProps}/>
      </GameWindow>
      {dialog}
    </>
  );
}

export default Application;
