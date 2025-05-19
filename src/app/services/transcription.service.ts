import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Transcription {
  id: number;
  text: string;
  fileName: string;
  audioUrl?: string; 
}

@Injectable({
  providedIn: 'root'
})
export class TranscriptionService {
  private apiUrl = 'http://localhost:5105/api/transcriptions';

  constructor(private http: HttpClient) {}

  getTranscriptions(): Observable<Transcription[]> {
    return this.http.get<Transcription[]>(this.apiUrl);
  }

  updateTranscription(transcription: Transcription) {
    return this.http.put(`${this.apiUrl}/${transcription.id}`, { text: transcription.text });
  }

  // ✅ Yeni transkripsiyon eklemek için eklendi
  addTranscription(formData: FormData): Observable<Transcription> {
    return this.http.post<Transcription>(this.apiUrl, formData);
  }
}
