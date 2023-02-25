import {useRouter} from "next/router";
import {useEffect, useState} from "react";
import {VideoDownload} from "../components/VideoDownload";
import Style from "./index.module.scss";

function IndexPage() {
  
  const router = useRouter();
  const [url, setURL] = useState<string>(getQueryValue(router.query.url));
  
  useEffect(() => { if (router.isReady) setURL(getQueryValue(router.query.url)); }, [router.isReady]);
  
  return (
    <div className={Style.Component}>
      <VideoDownload value={url} onChange={onURLChange}/>
    </div>
  );
  
  async function onURLChange(url: string) {
    setURL(url);
    await router.replace("/", {query: {url}});
  }
  
}

function getQueryValue(parameter?: string | string[]) {
  return (Array.isArray(parameter) ? parameter.at(0) : parameter) ?? "";
}

export default IndexPage;
