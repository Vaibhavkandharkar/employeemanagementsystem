import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { Constant } from '../../core/constant/Constant';

@Component({
  imports: [FormsModule],
  selector: 'app-login',
  styleUrl: './login.css',
  templateUrl: './login.html',
})
export class Login {
  loginObj: { username: string; password: string } = {
    username: '',
    password: '',
  };

  http = inject(HttpClient);
  router = inject(Router);

  onLogin() {
    this.http.post(environment.API_URL + Constant.APP_METHODS.LOGIN, this.loginObj).subscribe({
      next: (res: any) => {
        if (res.result) {
          alert("Login Successful");
          localStorage.setItem('empUser', JSON.stringify(res.data));
          this.router.navigateByUrl("/dashboard");
        } else {
          alert(res.message || "Invalid credentials");
        }
      },
      error: (err: any) => {
        console.error('Login error:', err);
        alert(err.error?.message || "Login failed");
      }
    });
  }
}
