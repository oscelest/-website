import superagent from "superagent";
import {VideoType} from "../enums/VideoType";
import {Video} from "./Video";

export class RedditVideo extends Video {
  
  public duration: number;
  public title: string;
  public quality_collection: {[key: string]: string};
  public url_thumbnail: string;
  
  public url_dash: string;
  
  constructor(data: RedditThread, mpd: string) {
    super(VideoType.REDDIT);
    
    this.title = data.title;
    this.duration = data.media.reddit_video.duration;
    this.url_thumbnail = data.thumbnail;
    this.url_dash = data.media.reddit_video.dash_url.replace(/(?<=DASH).*/i, "");
  
    this.quality_collection = {};
    const parser = new DOMParser();
    const xmlDoc = parser.parseFromString(mpd,"text/xml");
    const video_list = xmlDoc.querySelectorAll(`AdaptationSet[contentType="video"] Representation`);
    for (let video of video_list) {
      const key = video.getAttribute("height");
      const value = video.querySelector("BaseURL")?.textContent;
      if (!key || !value) continue;
      this.quality_collection[key] = value;
    }
  }
  
  public async download(quality?: string) {
    quality = this.validateQuality(quality);
    
    const response = await superagent
    .post(`${location.origin}/api/download/reddit`)
    .set("Content-Type", "application/json")
    .responseType("blob")
    .send(JSON.stringify({quality, url: this.url_dash}));
    
    const file = new File([response.body], `${this.title}.mp4`, {type: "video/mp4"});
    const url = URL.createObjectURL(file);
    const a = document.createElement("a");
    a.href = url;
    a.download = file.name;
    a.click();
    URL.revokeObjectURL(url);
  }
  
}

interface RedditThread {
  title: string;
  thumbnail: string;
  media: RedditThreadMedia;
}

interface RedditThreadMedia {
  reddit_video: RedditThreadMediaVideo;
}

interface RedditThreadMediaVideo {
  bitrate_kbps: number;
  dash_url: string;
  duration: number;
  fallback_url: string;
  height: number;
  hls_url: string;
  is_gif: boolean;
  scrubber_media_url: string;
  transcoding_status: string;
  width: number;
}
