import Superagent, {Response, SuperAgentRequest} from "superagent";
import {LocalStorageKeyType} from "../enums/LocalStorageKeyType";
import {SimpleEntity, SimpleEntityJSON} from "./SimpleEntity";

export class User extends SimpleEntity {
  
  public email: string;
  public token: string;
  
  private static authentication_request: SuperAgentRequest | undefined;
  
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
  
  
  public static async signup(email: string, password: string) {
    return this.handleResponse(await this.handleAuthenticationRequest(AuthenticationType.SignUp, email, password));
  }
  
  public static async login(email: string, password: string) {
    return this.handleResponse(await this.handleAuthenticationRequest(AuthenticationType.LogIn, email, password));
  }
  
  public static async refresh(auth: string) {
    return this.handleResponse(await this.handleAuthenticationRequest(AuthenticationType.Refresh, auth));
  }
  
  private static async handleAuthenticationRequest(type: AuthenticationType.SignUp | AuthenticationType.LogIn, email: string, password: string): Promise<SuperAgentRequest>
  private static async handleAuthenticationRequest(type: AuthenticationType.Refresh, auth: string): Promise<SuperAgentRequest>
  private static async handleAuthenticationRequest(type: AuthenticationType, arg1: string, arg2?: string): Promise<SuperAgentRequest> {
    if (this.authentication_request) return this.authentication_request;
    switch (type) {
      case AuthenticationType.LogIn:
        this.authentication_request = Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/LogIn`).send({email: arg1, password: arg2});
        break;
      case AuthenticationType.SignUp:
        this.authentication_request = Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/Signup`).send({email: arg1, password: arg2});
        break;
      case AuthenticationType.Refresh:
        this.authentication_request = Superagent.post(`${process.env.NEXT_PUBLIC_API_HOST}/User/Refresh`).auth(arg1, {type: "bearer"});
        break;
    }
    
    return this.authentication_request!.then(request => request).finally(() => this.authentication_request = undefined);
  }
  
  private static handleResponse({body}: Response): User {
    const {user, token} = body;
    localStorage.setItem(LocalStorageKeyType.JWT, token);
    return new User(user);
  }
  
  public static logout() {
    localStorage.removeItem(LocalStorageKeyType.JWT);
  }
}

enum AuthenticationType {
  SignUp  = 0,
  LogIn   = 1,
  Refresh = 2,
}

export interface UserJSON extends SimpleEntityJSON {
  email: string;
  token?: string;
}
