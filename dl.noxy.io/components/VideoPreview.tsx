import {Button} from "@noxy/react-button";
import React, {DetailedHTMLProps, HTMLAttributes, useState} from "react";
import {Video} from "../classes/Video";
import {Loader} from "./Loader";
import Style from "./VideoPreview.module.scss";

export function VideoPreview(props: VideoPreviewProps) {
  const {value, loading, className, ...component_method_props} = props;
  const {onChange, ...component_props} = component_method_props;
  
  const [error, setError] = useState<string>();
  
  const classes = [Style.Component];
  if (className) classes.push(className);
  
  return (
    <div {...component_props} className={classes.join(" ")}>
      <Loader active={loading}>
        {renderContent(value)}
      </Loader>
    </div>
  );
  
  function renderContent(video?: Video) {
    if (!video) return null;
    
    return (
      <>
        <div className={Style.Thumbnail}>
          <img src={video.url_thumbnail} alt={"Thumbnail"}/>
        </div>
        <div className={Style.Info}>
          <div className={Style.Title}>{video.title}</div>
          <div className={Style.Error}>{error}</div>
          <div className={Style.OptionList}>
            {Object.entries(video.quality_collection).map(renderOption)}
          </div>
        </div>
      </>
    );
  }
  
  function renderOption([quality, key]: [string, string]) {
    return (
      <Button key={key} value={quality} onSubmit={onDownload}>{`Download in ${quality}p`}</Button>
    );
  }
  
  function onDownload(quality?: string) {
    try {
      value?.download(quality);
    }
    catch (error) {
      if (!(error instanceof Error)) return console.error(error);
      setError(error.message);
    }
  }
}


type HTMLComponentProps = DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>

export interface VideoPreviewProps extends HTMLComponentProps {
  children?: never;
  value?: Video;
  loading?: boolean;
}
