import Head from "next/head";
import React from "react";
import Style from "./index.module.scss";

function IndexPage() {
  return (
    <div className={Style.Component}>
      <Head>
        <title>Download video | Noxy.io</title>
      </Head>
      No one lives here right now.
    </div>
  );
}

export default IndexPage;
