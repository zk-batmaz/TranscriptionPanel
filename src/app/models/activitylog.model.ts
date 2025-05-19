export interface ActivityLog {
  id: number;
  userId: number;
  action: string;
  fileName?: string;
  timestamp: string;
  userName?: string;
  transcriptionId?: number;
}
