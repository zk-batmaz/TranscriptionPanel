import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-add-transcription-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './transcription-dialog.component.html',
  styleUrls: ['./transcription-dialog.component.css']
})
export class AddTranscriptionDialogComponent {
  newText: string = '';
  selectedFile: File | null = null;
  audioUrl: string | null = null;

  constructor(private dialogRef: MatDialogRef<AddTranscriptionDialogComponent>) {}

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.audioUrl = URL.createObjectURL(this.selectedFile);
    }
  }

  addTranscription() {
    if (!this.newText.trim() || !this.selectedFile) {
      alert('Please enter both transcription text and audio file.');
      return;
    }

    this.dialogRef.close({
      text: this.newText,
      file: this.selectedFile
    });
  }
}

