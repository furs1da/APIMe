import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ErrorHandlerService } from './shared/services/error-handler.service';
import { NgModule } from '@angular/core';
import { JwtModule } from "@auth0/angular-jwt";
import { AuthGuard } from './shared/guards/auth.guard';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminGuard } from './shared/guards/admin.guard';
import { ForgotPasswordComponent } from './authentication/forgot-password/forgot-password.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'; 
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';
import { MatOptionModule } from '@angular/material/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

export function tokenGetter() {
  return localStorage.getItem("token");
}


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    NotFoundComponent,
    ForbiddenComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule,
    MatOptionModule,
    MatInputModule,
    MatFormFieldModule,
    MatDividerModule,
    MatCardModule,
    MatTableModule,
    MatSlideToggleModule,
    CollapseModule.forRoot(),
    NgbModule,
    FontAwesomeModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'privacy', component: PrivacyComponent, canActivate: [AuthGuard, AdminGuard] },
      { path: 'authentication', loadChildren: () => import('./authentication/authentication.module').then(m => m.AuthenticationModule) },
      { path: 'section', loadChildren: () => import('./section/section.module').then(m => m.SectionModule), canActivate: [AuthGuard] },
      { path: 'student', loadChildren: () => import('./student/student.module').then(m => m.StudentModule), canActivate: [AuthGuard] },
      { path: 'profile', loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule), canActivate: [AuthGuard] },
      { path: 'route', loadChildren: () => import('./routes/route.module').then(m => m.RouteModule) },
      { path: '404', component: NotFoundComponent },
      { path: 'forbidden', component: ForbiddenComponent },
      
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: '**', redirectTo: '/404', pathMatch: 'full' }

    
    ]),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlerService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor() {
    library.add(fas);
  }
}
