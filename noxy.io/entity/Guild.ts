import Superagent from "superagent";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";
import {Unit, UnitJSON} from "./Unit";
import {User, UserJSON} from "./User";

export class Guild extends SimpleEntity {
  
  public user: User;
  public unit_list: Unit[];
  
  constructor(init?: GuildJSON) {
    super(init);
    this.user = new User(init?.user);
    this.unit_list = init?.unitList?.map(x => new Unit(x)) ?? [];
  }
  
  public override toJSON(): GuildJSON {
    return {
      ...super.toJSON(),
      user: this.user.toJSON(),
      unitList: this.unit_list.map(x => x.toJSON())
    };
  }
  
  public static async load() {
    const response = await Superagent.get(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Load`).auth(localStorage.auth, {type: "bearer"}).send();
    return response.status !== 204 ? new Guild(response.body) : null;
  }
  
  public static async register(name: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Register`).auth(localStorage.auth, {type: "bearer"}).send({name});
    return new Guild(response.body);
  }
  
}

export interface GuildJSON extends SimpleEntityJSON {
  user?: UserJSON;
  unitList?: UnitJSON[];
}
