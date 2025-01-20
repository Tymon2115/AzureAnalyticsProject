import { bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations'; // Import animations
import { DashboardComponent } from './app/components/dashboard/dashboard.component';
import { provideHttpClient } from '@angular/common/http';

bootstrapApplication(DashboardComponent, {
  providers: [provideAnimations(), provideHttpClient()],
}).catch((err) => console.error(err));
