export interface JAStatusEntryDto {
  id: string;
  jobApplicationId: string;
  orderIndex: number;
  jaStatusType: number;
  createdAt: string;
  note?: string | null;
}