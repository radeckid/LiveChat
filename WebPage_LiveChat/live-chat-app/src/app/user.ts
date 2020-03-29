export interface User {
  id: number;
  email: string;
  friends?: Array<User>;
}
