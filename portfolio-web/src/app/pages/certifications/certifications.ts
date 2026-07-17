import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';
import { monthYear } from '../../core/utils/format';

@Component({
  selector: 'app-certifications',
  imports: [MatIconModule, MatProgressSpinnerModule],
  template: `
    <section class="page">
      <h1 class="page-title">Certifications</h1>
      <p class="page-subtitle">Credentials and professional certifications.</p>

      @let s = state();
      @if (s.status === 'loading') {
        <div class="state"><mat-spinner diameter="44" /><span>Loading…</span></div>
      } @else if (s.status === 'error') {
        <div class="state"><mat-icon>error_outline</mat-icon><span>{{ s.error }}</span></div>
      } @else if (!s.data?.length) {
        <div class="state"><mat-icon>verified</mat-icon><span>No certifications listed yet.</span></div>
      } @else {
        <div class="grid grid-cards">
          @for (cert of s.data; track cert.certificationId) {
            <article class="cert">
              <div class="cert-top"><mat-icon>verified</mat-icon></div>
              <h3>{{ cert.certificationName }}</h3>
              <p class="issuer">{{ cert.issuerName }}</p>
              <p class="dates">
                <span>Issued {{ monthYear(cert.issueDate) }}</span>
                @if (cert.doesNotExpire) {
                  <span class="pill ok">No expiry</span>
                } @else if (cert.expiryDate) {
                  <span class="pill">Expires {{ monthYear(cert.expiryDate) }}</span>
                }
              </p>
            </article>
          }
        </div>
      }
    </section>
  `,
  styles: `
    .cert {
      background: #fff; border: 1px solid rgba(0,0,0,0.06); border-radius: 16px; padding: 22px;
      display: flex; flex-direction: column; gap: 6px;
    }
    .cert-top { color: var(--brand); }
    .cert-top mat-icon { font-size: 28px; height: 28px; width: 28px; }
    .cert h3 { margin: 4px 0 0; font-size: 1.08rem; }
    .issuer { margin: 0; color: var(--mat-sys-on-surface-variant, #5f6368); font-weight: 500; }
    .dates { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; margin: 8px 0 0; font-size: 0.82rem; color: var(--mat-sys-on-surface-variant, #5f6368); }
    .pill { background: rgba(0,0,0,0.06); padding: 3px 10px; border-radius: 999px; font-weight: 500; }
    .pill.ok { background: rgba(46,125,50,0.12); color: #2e7d32; }
  `
})
export class CertificationsPage {
  private readonly api = inject(PortfolioApiService);
  readonly monthYear = monthYear;
  readonly state = toState(this.api.getCertifications());
}
