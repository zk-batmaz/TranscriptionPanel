import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'transcriptions',
    loadComponent: () =>
      import('./pages/transcriptions/transcriptions.component').then(m => m.TranscriptionsComponent)
  },
  {
    path: 'activities',
    loadComponent: () =>
      import('./pages/activities/activities.component').then(m => m.ActivitiesComponent)
  },
  {
  path: 'users',
  loadComponent: () =>
    import('./pages/users/users.component').then(m => m.UsersComponent)
}
];
