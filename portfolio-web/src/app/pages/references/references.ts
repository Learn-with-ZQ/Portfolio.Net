import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';

@Component({
  selector: 'app-references',
  imports: [MatIconModule, MatProgressSpinnerModule],
  template: `
    <section class="page">
      <h1 class="page-title">References</h1>
      <p class="page-subtitle">Managers, team leads, professors, mentors and clients.</p>

      @let s = state();
      @if (s.status === 'loading') {
        <div class="state"><mat-spinner diameter="44" /><span>Loading…</span></div>
      } @else if (s.status === 'error') {
        <div class="state"><mat-icon>error_outline</mat-icon><span>{{ s.error }}</span></div>
      } @else if (s.data && s.data.length) {
        <div class="grid grid-cards">
          @for (r of s.data; track r.referenceId) {
            <article class="ref glass">
              <span class="avatar">{{ r.fullName.charAt(0) }}</span>
              <h3>{{ r.fullName }}</h3>
              <p class="role">
                {{ r.designation }}@if (r.designation && r.organization) {, }{{ r.organization }}
              </p>
              @if (r.relationship) { <span class="tag">{{ r.relationship }}</span> }

              @if (r.isContactVisible) {
                <div class="contact">
                  @if (r.email) { <a [href]="'mailto:' + r.email"><mat-icon>mail</mat-icon>{{ r.email }}</a> }
                  @if (r.phone) { <a [href]="'tel:' + r.phone"><mat-icon>call</mat-icon>{{ r.phone }}</a> }
                </div>
              } @else {
                <p class="hidden"><mat-icon>lock</mat-icon> Contact available on request</p>
              }
              @if (r.linkedInUrl) {
                <a class="linkedin" [href]="r.linkedInUrl" target="_blank" rel="noopener">
                  <mat-icon>business_center</mat-icon> LinkedIn
                </a>
              }
            </article>
          }
        </div>
      } @else {
        <div class="state"><mat-icon>groups</mat-icon><span>No references listed yet.</span></div>
      }
    </section>
  `,
  styles: `
    .ref {
      border-radius: var(--radius);
      padding: 24px;
      display: flex;
      flex-direction: column;
      gap: 6px;
    }
    .avatar {
      display: inline-grid; place-items: center; width: 52px; height: 52px;
      border-radius: 50%; background: var(--grad-brand); color: #fff; font-weight: 800; font-size: 1.3rem;
      margin-bottom: 6px;
    }
    .ref h3 { margin: 0; font-size: 1.1rem; }
    .role { margin: 0; color: var(--ink-soft); font-size: 0.9rem; }
    .tag {
      align-self: flex-start; margin-top: 4px;
      background: rgba(81,43,212,0.1); color: var(--c-primary);
      padding: 3px 10px; border-radius: 999px; font-size: 0.76rem; font-weight: 600;
    }
    .contact { display: flex; flex-direction: column; gap: 6px; margin-top: 10px; }
    .contact a, .linkedin { display: inline-flex; align-items: center; gap: 6px; color: var(--c-secondary); font-size: 0.88rem; font-weight: 500; }
    .contact mat-icon, .linkedin mat-icon, .hidden mat-icon { font-size: 18px; height: 18px; width: 18px; }
    .hidden { display: flex; align-items: center; gap: 6px; color: var(--ink-soft); font-size: 0.85rem; margin: 10px 0 0; }
    .linkedin { margin-top: 8px; }
  `
})
export class ReferencesPage {
  private readonly api = inject(PortfolioApiService);
  readonly state = toState(this.api.getReferences());
}
