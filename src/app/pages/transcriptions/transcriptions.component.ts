import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranscriptionService, Transcription } from '../../services/transcription.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ActivityService } from '../../services/activity.service';
import { ActivityLog } from '../../models/activitylog.model';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { AddTranscriptionDialogComponent } from './add-transcription-dialog.component';


@Component({
  selector: 'app-transcriptions',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDialogModule,
    AddTranscriptionDialogComponent
  ],
  templateUrl: './transcriptions.component.html',
  styleUrls: ['./transcriptions.component.css']
})
export class TranscriptionsComponent implements OnInit {
  transcriptions: (Transcription & { audioUrl: string; updatedText: string })[] = [];
  filterText: string = '';

  newText: string = '';
  selectedFile: File | null = null;

  constructor(
    private transcriptionService: TranscriptionService,
    private activityService: ActivityService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadTranscriptions();
  }

  loadTranscriptions() {
    this.transcriptionService.getTranscriptions().subscribe({
      next: (data) => {
        this.transcriptions = data.map(item => ({
          ...item,
          audioUrl: `http://localhost:5105/uploads/${encodeURIComponent(item.fileName)}`,
          updatedText: item.text
        }));
      },
      error: (err) => console.error('Veri alınamadı:', err)
    });
  }

  get filteredTranscriptions() {
    return this.transcriptions.filter(t =>
      t.updatedText?.toLowerCase().includes(this.filterText.toLowerCase())
    );
  }

  applyFilter() {
    this.filterText = this.filterText.trim();
  }

  saveTranscription(item: Transcription & { updatedText: string }) {
    const updated: Transcription = {
      id: item.id,
      fileName: item.fileName,
      text: item.updatedText
    };

    this.transcriptionService.updateTranscription(updated).subscribe({
      next: () => {
        alert('Transcription saved.');
        const activity: ActivityLog = {
          id: 0,
          userId: 1,
          userName: 'Admin',
          action: `Transcription #${item.id} updated`,
          timestamp: new Date().toISOString(),
          transcriptionId: undefined
        };
        this.activityService.logActivity(activity).subscribe();
      },
      error: (err) => {
        console.error('Update Error:', err);
        alert('Could not save!');
      }
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  addTranscription() {
    if (!this.selectedFile || !this.newText.trim()) {
      alert('Please enter both transcription text and audio file.');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('text', this.newText.trim());

    this.transcriptionService.addTranscription(formData).subscribe({
      next: (newItem) => {
        alert('New transcription added.');
        this.newText = '';
        this.selectedFile = null;
        this.loadTranscriptions();

        const activity: ActivityLog = {
          id: 0,
          userId: 1,
          userName: 'Admin',
          action: `New transcription added (#${newItem.id})`,
          timestamp: new Date().toISOString(),
          transcriptionId: newItem.id
        };
        this.activityService.logActivity(activity).subscribe();
      },
      error: (err) => {
        console.error('Error:', err);
        alert('Could not save!');
      }
    });
  }
  openAddDialog() {
    const dialogRef = this.dialog.open(AddTranscriptionDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result?.text && result?.file) {
        const formData = new FormData();
        formData.append('file', result.file);
        formData.append('text', result.text);

        this.transcriptionService.addTranscription(formData).subscribe({
          next: (newItem) => {
            alert('New transcription added.');
            this.loadTranscriptions();

            const activity: ActivityLog = {
              id: 0,
              userId: 1,
              userName: 'Admin',
              action: `New transcription added (#${newItem.id})`,
              timestamp: new Date().toISOString(),
              transcriptionId: newItem.id
            };
            this.activityService.logActivity(activity).subscribe();
          },
          error: (err) => {
            console.error('Error:', err);
            alert('Could Not Save!');
          }
        });
      }
    });
  }
}
