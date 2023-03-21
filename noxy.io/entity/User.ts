import Superagent from "superagent";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class User extends SimpleEntity {
  
  public email: string;
  public token: string;
  
  constructor(init?: UserJSON) {
    super(init);
    this.email = init?.email ?? "";
    this.token = init?.token ?? "";
  }
  
  public override toJSON(): UserJSON {
    return {
      ...super.toJSON(),
      email: this.email,
      token: this.token
    };
  }
  
  public static async login(email: string, password: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/LogIn`).send({email, password});
    
    const user = new User(response.body);
    localStorage.setItem("auth", user.token);
    
    return user;
  }
  
  public static async signup(email: string, password: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/SignUp`).send({email, password});
    
    const user = new User(response.body);
    localStorage.setItem("auth", user.token);
    
    return user;
  }
  
  public static logout() {
    localStorage.removeItem("auth");
  }
}

export interface UserJSON extends SimpleEntityJSON {
  email: string;
  token: string;
}
