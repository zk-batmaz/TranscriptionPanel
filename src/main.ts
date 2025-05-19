import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app/app.routes';
import { authInterceptorFn } from './app/interceptors/auth.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(withInterceptors([authInterceptorFn])),
    provideRouter(routes),
    provideAnimationsAsync()
  ]
}).then(() => {
  console.log('🚀 Angular uygulaması başarıyla başlatıldı');
}).catch(err => {
  console.error('❌ Bootstrap hatası:', err);
});