import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {createSubscription} from "@noxy/react-subscription-hook";
import {User} from "./classes/User";

export const subscriptionUser = createSubscription<User | undefined>(undefined);
export const subscriptionSocket = createSubscription<HubConnection>(new HubConnectionBuilder().withUrl("http://localhost:5161/ws/game").build());

export interface APIErrorResponse {
  status: number;
  title: string;
  traceId: string;
  type: string;
}


export interface BadRequestResponse extends APIErrorResponse {
  errors: {[key: string]: string[]};
}
