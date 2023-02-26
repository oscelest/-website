import ffmpegPath from "@ffmpeg-installer/ffmpeg";
import ffmpeg from "fluent-ffmpeg";

ffmpeg.setFfmpegPath(process.env.FFMPEG_PATH || ffmpegPath.path);

export default ffmpeg;
