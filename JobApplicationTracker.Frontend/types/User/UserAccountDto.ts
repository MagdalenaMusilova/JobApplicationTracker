import { UserResumeDto } from './UserResumeDto';

export interface UserAccountDto {
  id: string;
  email: string;
  username: string;
  firstName?: string;
  lastName?: string;
  resume?: UserResumeDto;
}
