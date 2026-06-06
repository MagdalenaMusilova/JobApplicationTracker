export interface CreateJAStatusEntryDto {
  jobApplicationId: string;
  statusType: number;
  note?: string | null;
}