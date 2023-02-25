import {InputField, InputFieldChangeEvent} from "@noxy/react-input-field";
import {DetailedHTMLProps, HTMLAttributes, useEffect, useState} from "react";
import superagent from "superagent";
import {RedditVideo} from "../classes/RedditVideo";
import {Video} from "../classes/Video";
import {useDefer} from "../hooks/useDefer";
import Style from "./VideoDownload.module.scss";
import {VideoPreview} from "./VideoPreview";

export function VideoDownload(props: VideoDownloadProps) {
  const {value, className, children, ...component_method_props} = props;
  const {onChange, ...component_props} = component_method_props;
  
  const [loading, setLoading] = useState<boolean>(false);
  const [video, setVideo] = useState<Video>();
  const [error, setError] = useState<string>();
  const defer = useDefer();
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  useEffect(
    () => {
      if (value) setLoading(true);
      defer(onLoadURL, 500, value);
    },
    [value]
  );
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      <InputField className={Style.Input} label={"Paste video url here"} error={error} value={value} onChange={onInputChange}/>
      <VideoPreview value={video} loading={loading}/>
    </div>
  );
  function onInputChange(event: InputFieldChangeEvent) {
    setError(undefined);
    onChange(event.value);
  }
  async function onLoadURL(value: string) {
    if (!value) return setVideo(undefined);
    try {
      const url = new URL(value);
      switch (url.hostname.toLowerCase()) {
        case "reddit.com":
        case "www.reddit.com":
          return await getRedditVideoData(url);
      }
    }
    catch {
      handleError("URL is not valid.");
    }
  }
  
  async function getRedditVideoData(url: URL) {
    setError(undefined);
    const thread_response = await superagent.get(`${url.href.replace(/\/+$/, "")}.json`);
    const mpd_response = await superagent.get(thread_response.body[0].data.children[0].data.media.reddit_video.dash_url);
    
    try {
      setVideo(new RedditVideo(thread_response.body[0].data.children[0].data, mpd_response.text));
      console.log("setting false");
      setLoading(false);
    }
    catch (error) {
      if (!(error instanceof Error)) return;
      handleError(error.message);
    }
  }
  
  function handleError(message: string) {
    console.log("hello?");
    setError(message);
    setVideo(undefined);
    setLoading(false);
  }
  
}


type HTMLComponentProps = DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>

export interface VideoDownloadProps extends Omit<HTMLComponentProps, "onChange"> {
  children?: never;
  value: string;
  onChange(url: string): void;
}
