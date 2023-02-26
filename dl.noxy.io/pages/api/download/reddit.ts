import {randomUUID} from "crypto";
import fs from "fs-extra";
import {NextApiRequest, NextApiResponse} from "next";
import * as os from "os";
import path from "path";
import superagent from "superagent";
import ffmpeg from "../../../libs/ffmpeg";

export const config = {
  api: {
    responseLimit: false
  }
};

export default async function DownloadRedditEndpoint(request: NextApiRequest, response: NextApiResponse) {
  if (request.method?.toLowerCase() !== "post") return response.status(404).json(getResponse(false, "Endpoint does not exist."));
  if (request.headers["content-type"]?.toLowerCase() !== "application/json") return response.status(400).json(getResponse(false, "Endpoint only accepts JSON body."));
  if (!request.body.url) return response.status(400).json(getResponse(false, "URL is missing from request."));
  if (!request.body.quality) return response.status(400).json(getResponse(false, "URL is missing from request."));
  
  const directory = path.resolve(os.tmpdir(), "download");
  fs.ensureDirSync(directory);
  const video_file = path.resolve(directory, `reddit-video-${randomUUID()}.mp4`);
  const audio_file = path.resolve(directory, `reddit-audio-${randomUUID()}.mp4`);
  const merged_file = path.resolve(directory, `reddit-merge-${randomUUID()}.mp4`);
  
  const video_url = `${request.body.url}_${request.body.quality}.mp4`;
  const audio_url = `${request.body.url}_audio.mp4`;
  
  if (await writeFile(video_file, video_url) && await writeFile(audio_file, audio_url)) {
    if (await mergeFile(merged_file, video_file, audio_file)) {
      try {
        const buffer = fs.readFileSync(merged_file);
        response.send(buffer);
      }
      catch (error) {
        console.error(error);
        response.status(500).json(getResponse(false, "Unexpected server exception occurred"));
      }
      finally {
        fs.unlink(video_file, error => error && console.error(error));
        fs.unlink(audio_file, error => error && console.error(error));
        fs.unlink(merged_file, error => error && console.error(error));
      }
      return;
    }
  }
  response.status(500).json(getResponse(false, "Unexpected server exception occurred"));
}

async function writeFile(file: string, url: string): Promise<boolean> {
  try {
    fs.writeFileSync(file, (await superagent.get(url)).body);
    return true;
  }
  catch (error) {
    console.error(error);
    fs.unlink(file, console.error);
    return false;
  }
}

async function mergeFile(file: string, video: string, audio: string): Promise<boolean> {
  return new Promise((resolve, reject) => {
    ffmpeg()
    .addInput(video)
    .addInput(audio)
    .addOptions(["-map 0:v", "-map 1:a", "-c:v copy"])
    .format("mp4")
    .saveToFile(file)
    .on("end", () => resolve(true))
    .on("error", (error: Error) => {
      console.error(error);
      resolve(false);
      fs.unlink(file, console.error);
    });
  });
}

function getResponse(success: boolean, message: string) {
  return JSON.stringify({success, message});
}
