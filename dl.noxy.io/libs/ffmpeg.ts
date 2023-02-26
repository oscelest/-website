import ffmpeg from "fluent-ffmpeg";

if (!process.env.FFMPEG_PATH) throw new Error("FFMPEG_PATH is not set.");
ffmpeg.setFfmpegPath(process.env.FFMPEG_PATH);

export default ffmpeg;
