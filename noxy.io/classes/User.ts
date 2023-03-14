import Superagent from "superagent";
import {v4} from "uuid";

export class User {
  
  public id: string;
  public email: string;
  public token: string;
  public timeCreated: Date;
  
  constructor(init?: UserJSON) {
    this.id = init?.id ?? v4();
    this.email = init?.email ?? "";
    this.token = init?.token ?? "";
    this.timeCreated = new Date(init?.timeCreated ?? 0);
  }
  
  public toString() {
    return this.id;
  }
  
  public toJSON(): UserJSON {
    return {
      id: this.id,
      email: this.email,
      token: this.token,
      timeCreated: this.timeCreated.toString()
    };
  }
  
  public static async login(email: string, password: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/LogIn`)
    .send({email, password});
    
    const user = new User(response.body);
    localStorage.setItem("auth", user.token);
    
    return user;
  }
  
  public static async signup(email: string, password: string) {
    const response = await Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/SignUp`)
    .send({email, password});
  
    const user = new User(response.body);
    localStorage.setItem("auth", user.token);
    
    return user;
  }
  
  public static logout() {
    localStorage.removeItem("auth");
  }
}

export interface UserJSON {
  id: string;
  email: string;
  token: string;
  timeCreated: string;
}
