import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service'; // yol doğruysa
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  hide = true;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

login() {
  if (this.username && this.password) {
    this.authService.login(this.username, this.password).subscribe({
      next: (response) => {
        console.log('✅ Token başarıyla alındı:', response.token);
        localStorage.setItem('token', response.token);
        this.router.navigate(['/transcriptions']);
      },
      error: (error) => {
        console.error('Login failed:', error);
        alert('Login failed. Please enter the information correctly.');
      }
    });
  } else {
    alert('Enter your username and password.');
  }
}


  onSubmit() {
    this.login();
  }
}
