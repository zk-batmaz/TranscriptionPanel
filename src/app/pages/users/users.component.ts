import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User, CreateUserRequest } from '../../services/user.service';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule, 
    MatButtonModule, 
    MatInputModule, 
    MatSelectModule,
    MatFormFieldModule,
    MatCardModule,
    MatSnackBarModule,
    FormsModule
  ],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  newUser: CreateUserRequest = { username: '', password: '', role: 'editor' };
  displayedColumns: string[] = ['username', 'role', 'actions'];
  loading = false;

  constructor(
    private userService: UserService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.loading = true;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error while getting users:', error);
        this.showSnackBar(error.message || 'Error while getting users');
        this.loading = false;
      }
    });
  }

  addUser(): void {
    if (!this.isFormValid()) {
      this.showSnackBar('Please fill in all fields!');
      return;
    }

    this.loading = true;
    this.userService.addUser(this.newUser).subscribe({
      next: () => {
        this.getUsers();
        this.resetForm();
        this.showSnackBar('User added successfully');
        this.loading = false;
      },
      error: (error) => {
        console.error('Error while adding user:', error);
        this.showSnackBar(error.message || 'Error while adding user');
        this.loading = false;
      }
    });
  }

  deleteUser(id: number): void {
    if (confirm('Are you sure you want to delete this user?')) {
      this.loading = true;
      this.userService.deleteUser(id).subscribe({
        next: () => {
          this.getUsers();
          this.showSnackBar('User deleted successfully');
          this.loading = false;
        },
        error: (error) => {
          console.error('Error while deleting user:', error);
          this.showSnackBar(error.message || 'Error while deleting user');
          this.loading = false;
        }
      });
    }
  }

  private isFormValid(): boolean {
    return this.newUser.username.trim() !== '' && 
           this.newUser.password.trim() !== '' && 
           this.newUser.role.trim() !== '';
  }

  private resetForm(): void {
    this.newUser = { username: '', password: '', role: 'editor' };
  }

  private showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }
}