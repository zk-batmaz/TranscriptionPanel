import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { NgIf, CommonModule } from '@angular/common'; 
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, MatToolbarModule, MatButtonModule, NgIf, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'transcription-panel';

  constructor(public authService: AuthService) {}
  
  ngOnInit() {
    console.log('AppComponent initialized');
    console.log('isLoggedIn:', this.authService.isLoggedIn());
    console.log('isAdmin:', this.authService.isAdmin());
    console.log('Role:', this.authService.getRole());
    
    // Token debug
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        console.log('Token payload from app component:', payload);
      } catch (e) {
        console.error('Error parsing token:', e);
      }
    } else {
      console.log('No token found');
    }
  }

  logout() {
    this.authService.logout();
    window.location.href = '/login';
  }
}