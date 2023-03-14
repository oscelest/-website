import {useSubscription} from "@noxy/react-subscription-hook";
import {AppProps} from "next/app";
import React, {useEffect} from "react";
import {Content} from "../components/Layout/Content";
import {Header} from "../components/Layout/Header";
import {subscriptionSocket, subscriptionUser} from "../Globals";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  
  const [user] = useSubscription(subscriptionUser);
  const [socket] = useSubscription(subscriptionSocket);
  
  useEffect(
    () => {
      if (!user) return;
      socket.start()
      .then(async () => {
        socket.on("ReceiveMessage", (...args: any[]) => {
          console.log(...args);
        })
        await socket.invoke("SendMessage", user.email, "Hello World");
      })
      .catch(error => console.log(error));
    },
    [user]
  );
  
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
