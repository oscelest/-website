import {HubConnection} from "@microsoft/signalr";
import {createSubscription} from "@noxy/react-subscription-hook";
import {Guild} from "./entity/Guild";
import {User} from "./entity/User";
import {SceneType} from "./enums/SceneType";

export const subscriptionUser = createSubscription<User | undefined>(undefined);
export const subscriptionGuild = createSubscription<Guild | undefined>(undefined);
export const subscriptionScene = createSubscription<SceneType>(SceneType.NONE);
export const subscriptionSocket = createSubscription<HubConnection | undefined>(undefined);

export interface APIErrorResponse {
  status: number;
  title: string;
  traceId: string;
  type: string;
}


export interface BadRequestResponse extends APIErrorResponse {
  errors: {[key: string]: string[]};
}
