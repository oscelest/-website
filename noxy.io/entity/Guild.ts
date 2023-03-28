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
    this.unit_list = init?.unitList ?? [];
  }
  
  public override toJSON(): GuildJSON {
    return {
      ...super.toJSON(),
      user: this.user.toJSON(),
      unitList: this.unit_list
    };
  }
  
  public static async self() {
    const response = await Superagent.get(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Self`).auth(localStorage.auth, {type: "bearer"}).send();
    return response.status !== 204 ? new Guild(response.body) : null;
  }
  
  public static async register(name: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/Register`).auth(localStorage.auth, {type: "bearer"}).send({name});
    return new Guild(response.body);
  }
  
  public static async recruit(unit: Unit) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/RecruitUnit`).auth(localStorage.auth, {type: "bearer"}).send({unitID: unit.id});
    return response.body.map((x: UnitJSON) => new Unit(x));
  }
  
  public static async refreshUnitList() {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/Guild/RefreshUnitList`).auth(localStorage.auth, {type: "bearer"}).send();
    return response.body.map((x: UnitJSON) => new Unit(x));
  }
  
}

export interface GuildJSON extends SimpleEntityJSON {
  user: UserJSON;
  unitList: Unit[];
}
