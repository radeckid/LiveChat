export interface User {
  Id: number;
  Email: string;
  Friends?: Array<User>;
}
