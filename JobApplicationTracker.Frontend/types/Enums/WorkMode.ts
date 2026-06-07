export enum WorkMode {
  OnSite = 0,
  Hybrid = 1,
  Remote = 2,
}

export const workModeLabels: Record<WorkMode, string> = {
  [WorkMode.OnSite]: 'On-site',
  [WorkMode.Hybrid]: 'Hybrid',
  [WorkMode.Remote]: 'Remote',
};
