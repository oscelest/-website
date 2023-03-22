import {HubConnectionBuilder} from "@microsoft/signalr";
import {useDialog} from "@noxy/react-dialog";
import {trackSubscription, useSubscription} from "@noxy/react-subscription-hook";
import {AppProps} from "next/app";
import React from "react";
import {subscriptionSocket, subscriptionUser} from "../Globals";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  const [dialog] = useDialog();
  const [socket, setSocket] = useSubscription(subscriptionSocket);
  
  trackSubscription(subscriptionUser, async user => {
    if (!user) {
      return socket?.stop();
    }
    const hub = new HubConnectionBuilder().withUrl("http://localhost:5161/ws/game", {accessTokenFactory: () => user.token}).build();
    await hub.start();
    setSocket(hub);
  });
  
  return (
    <>
      <Component {...pageProps}></Component>
      {dialog}
    </>
  );
}

export default Application;
