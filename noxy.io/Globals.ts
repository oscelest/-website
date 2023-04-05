import {HubConnection} from "@microsoft/signalr";
import {createSubscription} from "@noxy/react-subscription-hook";
import {Guild} from "./entity/Guild";
import {Mission} from "./entity/Mission";
import {Unit} from "./entity/Unit";
import {User} from "./entity/User";

export const subscriptionUser = createSubscription<Data<User>>({});
export const subscriptionGuild = createSubscription<Data<Guild>>({});
export const subscriptionUnitList = createSubscription<Data<Unit[]>>({loading: false, value: []});
export const subscriptionMissionList = createSubscription<Data<Mission[]>>({loading: false, value: []});
export const subscriptionSocket = createSubscription<HubConnection | undefined>(undefined);

export interface Data<E> {
  value?: E
  loading?: boolean
}

export interface APIErrorResponse {
  status: number;
  title: string;
  traceId: string;
  type: string;
}


export interface BadRequestResponse extends APIErrorResponse {
  errors: {[key: string]: string[]};
}
