import {InputField, InputFieldChangeEvent, InputFieldEventType} from "@noxy/react-input-field";
import {DetailedHTMLProps, HTMLAttributes, useEffect, useState} from "react";
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
    if (event.type === InputFieldEventType.CHANGE) {
      setError(undefined);
      onChange(event.value);
    }
  }
  async function onLoadURL(value: string) {
    if (!value) return setVideo(undefined);
    
    try {
      setError(undefined);
      setVideo(await getVideoData(new URL(value)));
    }
    catch (error) {
      if (error instanceof TypeError && error.message.slice(0, 15) === "URL constructor") {
        handleError(error.message.slice(17));
      }
      else if (error instanceof Error) {
        handleError(error.message);
      }
      else {
        console.error(error);
        handleError("Unexpected error occurred");
      }
    }
    setLoading(false);
  }
  
  async function getVideoData(url: URL): Promise<Video> {
    switch (url.hostname.toLowerCase()) {
      case "reddit.com":
      case "www.reddit.com":
        return RedditVideo.getVideoData(url);
    }
    throw new Error("URL is not valid.");
  }
  
  function handleError(message: string) {
    setError(message);
    setVideo(undefined);
  }
  
}


type HTMLComponentProps = DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>

export interface VideoDownloadProps extends Omit<HTMLComponentProps, "onChange"> {
  children?: never;
  value: string;
  onChange(url: string): void;
}
