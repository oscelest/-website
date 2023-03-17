import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {useDialog} from "@noxy/react-dialog";
import {useSubscription} from "@noxy/react-subscription-hook";
import {AppProps} from "next/app";
import React, {useEffect, useState} from "react";
import {Content} from "../components/Layout/Content";
import {Header} from "../components/Layout/Header";
import {subscriptionSocket, subscriptionUser} from "../Globals";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  const [hub, setHub] = useState<HubConnection>();
  
  const [user] = useSubscription(subscriptionUser);
  const [, setSocket] = useSubscription(subscriptionSocket);
  const [dialog] = useDialog();
  
  useEffect(() => setHub(user ? new HubConnectionBuilder().withUrl("http://localhost:5161/ws/game", {accessTokenFactory: () => user.token}).build() : undefined), [user]);
  useEffect(
    () => {
      if (!hub) return;
      
      hub.start()
      .then(async () => setSocket(hub))
      .catch((error) => console.log(error));
    },
    [hub]
  );
  
  console.log(dialog)
  
  return (
    <>
      <Header/>
      <Content>
        <Component {...pageProps}></Component>
      </Content>
      {dialog}
    </>
  );
}

export default Application;
