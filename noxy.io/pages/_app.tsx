import {HubConnectionBuilder} from "@microsoft/signalr";
import {useDialog} from "@noxy/react-dialog";
import {trackSubscription, useSubscription} from "@noxy/react-subscription-hook";
import {AppProps} from "next/app";
import React, {useEffect} from "react";
import {User} from "../entity/User";
import {subscriptionAuth, subscriptionSocket} from "../Globals";
import "../public/style/font.scss";
import "../public/style/globals.scss";

function Application({Component, pageProps}: AppProps) {
  const [dialog] = useDialog();
  const [socket, setSocket] = useSubscription(subscriptionSocket);
  const [, setAuth] = useSubscription(subscriptionAuth);
  
  useEffect(() => {
    const {auth} = localStorage;
    if (auth) {
      return setAuth({refreshing: false});
    }
    
    User.refresh(auth)
    .then(([user, jwt]) => setAuth({jwt, user, refreshing: false}))
    .catch(() => {
      setAuth({refreshing: false});
      localStorage.removeItem("auth");
    });
  }, []);
  
  trackSubscription(subscriptionAuth, async ({jwt}) => {
    if (!jwt) return socket?.stop();
    
    const hub = new HubConnectionBuilder().withUrl("http://localhost:5161/ws/game", {accessTokenFactory: () => jwt}).build();
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
