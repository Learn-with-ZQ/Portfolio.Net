import { Component, computed, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SkillFull } from '../../core/models/portfolio.models';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { toState } from '../../core/utils/async-state';

interface RadarAxis {
  label: string;
  ax: number;
  ay: number;
  px: number;
  py: number;
  lx: number;
  ly: number;
  anchor: string;
}
interface Radar {
  size: number;
  cx: number;
  cy: number;
  axes: RadarAxis[];
  polygon: string;
  rings: number[];
}

@Component({
  selector: 'app-skills',
  imports: [MatIconModule, MatProgressSpinnerModule],
  templateUrl: './skills.html',
  styleUrl: './skills.scss'
})
export class SkillsPage {
  private readonly api = inject(PortfolioApiService);
  readonly state = toState(this.api.getSkillsFull());

  readonly skills = computed<SkillFull[]>(() => {
    const st = this.state();
    return st.status === 'success' && st.data ? st.data : [];
  });

  readonly maxCount = computed(() => Math.max(1, ...this.skills().map((s) => this.weight(s))));

  readonly radar = computed<Radar | null>(() => {
    const s = this.skills();
    return s.length >= 3 ? this.buildRadar(s) : null;
  });

  readonly cloud = computed(() =>
    this.skills().flatMap((sk) =>
      sk.details.map((d) => ({ name: d.skillDetailName, weight: this.weight(sk) }))
    )
  );

  weight(s: SkillFull): number {
    return s.detailCount || s.details.length || 0;
  }

  barPct(s: SkillFull): number {
    return Math.max(8, Math.round((this.weight(s) / this.maxCount()) * 100));
  }

  sizeClass(w: number): string {
    const m = this.maxCount();
    if (w >= m * 0.66) return 'lg';
    if (w >= m * 0.33) return 'md';
    return 'sm';
  }

  private buildRadar(skills: SkillFull[]): Radar {
    const size = 340;
    const cx = 170;
    const cy = 170;
    const R = 118;
    const max = this.maxCount();
    const n = skills.length;

    const axes: RadarAxis[] = skills.map((s, i) => {
      const ang = (-90 + (i * 360) / n) * (Math.PI / 180);
      const cos = Math.cos(ang);
      const sin = Math.sin(ang);
      const r = (this.weight(s) / max) * R;
      return {
        label: s.skillName,
        ax: +(cx + R * cos).toFixed(1),
        ay: +(cy + R * sin).toFixed(1),
        px: +(cx + r * cos).toFixed(1),
        py: +(cy + r * sin).toFixed(1),
        lx: +(cx + (R + 16) * cos).toFixed(1),
        ly: +(cy + (R + 16) * sin).toFixed(1),
        anchor: cos > 0.3 ? 'start' : cos < -0.3 ? 'end' : 'middle'
      };
    });

    return {
      size,
      cx,
      cy,
      axes,
      polygon: axes.map((a) => `${a.px},${a.py}`).join(' '),
      rings: [0.33, 0.66, 1].map((f) => +(f * R).toFixed(1))
    };
  }
}
