import { Injectable, computed, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class PortfolioContextService {
  private readonly _profileId = signal(1);

  readonly profileId = this._profileId.asReadonly();
  readonly label = computed(() => `Profile #${this._profileId()}`);

  setProfileId(id: number): void {
    if (id > 0) {
      this._profileId.set(id);
    }
  }
}
