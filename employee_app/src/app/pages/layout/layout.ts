import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  selector: 'app-layout',
  styleUrl: './layout.css',
  templateUrl: './layout.html',
})
export class Layout {
  private readonly router = inject(Router);
  readonly loggedUserName = this.getLoggedUserName();
  readonly userInitials = this.getInitials(this.loggedUserName);

  logout(): void {
    localStorage.removeItem('empUser');
    void this.router.navigateByUrl('/login');
  }

  private getLoggedUserName(): string {
    const savedUser = localStorage.getItem('empUser');
    if (!savedUser) {
      return 'Employee';
    }

    try {
      const user: unknown = JSON.parse(savedUser);
      if (typeof user === 'object' && user !== null) {
        const data = user as Record<string, unknown>;
        const name = data['employeeName'] ?? data['name'] ?? data['userName'] ?? data['username'] ?? data['emailId'] ?? data['email'];
        if (typeof name === 'string' && name.trim()) {
          return name.trim();
        }
      }
    } catch (error) {
      console.error('Unable to read the saved employee profile.', error);
    }

    return 'Employee';
  }

  private getInitials(name: string): string {
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length > 1) {
      return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    }
    return name.slice(0, 2).toUpperCase();
  }
}
