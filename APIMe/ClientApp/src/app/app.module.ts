import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
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
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: 'admin/sections', loadChildren: () => import('./section/section.module').then(m => m.SectionModule), canActivate: [AuthGuard] },
     

      { path: 'privacy', component: PrivacyComponent, canActivate: [AuthGuard, AdminGuard] },
      { path: 'authentication', loadChildren: () => import('./authentication/authentication.module').then(m => m.AuthenticationModule) },
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
export class AppModule { }
