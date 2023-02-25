import {VideoType} from "../enums/VideoType";

export abstract class Video {
  
  public readonly type: VideoType;
  
  public abstract title: string;
  public abstract duration: number;
  public abstract url_thumbnail: string;
  public abstract quality_collection: {[quality: string]: any};
  
  protected constructor(type: VideoType) {
    this.type = type;
  }
  
  public abstract download(quality?: string): void
  
  protected validateQuality(quality?: string): string {
    if (!quality || !this.quality_collection[quality]) throw new Error(`Video with quality '${quality}' does not exist.`);
    return quality;
  }
}
