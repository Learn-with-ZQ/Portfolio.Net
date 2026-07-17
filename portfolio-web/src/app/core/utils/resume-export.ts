import { ResumeData } from '../models/portfolio.models';
import { dateRange, monthYear } from './format';

export interface ResumeContact {
  owner: string;
  role: string;
  email: string;
  location: string;
  summary: string;
  links: { label: string; url: string }[];
}

const sorted = <T extends { sortOrder: number }>(items: readonly T[]): T[] =>
  [...items].sort((a, b) => a.sortOrder - b.sortOrder);

/** ATS-optimised plain text (.txt) — the most universally parseable format. */
export function buildPlainText(data: ResumeData, c: ResumeContact): string {
  const L: string[] = [];
  const rule = () => L.push('='.repeat(64));
  const head = (t: string) => {
    L.push('', t.toUpperCase(), '-'.repeat(t.length));
  };

  L.push(c.owner, c.role);
  L.push([c.location, c.email].filter(Boolean).join(' | '));
  if (c.links.length) L.push(c.links.map((l) => `${l.label}: ${l.url}`).join(' | '));
  rule();

  if (c.summary) {
    head('Summary');
    L.push(c.summary);
  }

  if (data.skills.length) {
    head('Skills');
    for (const s of sorted(data.skills)) {
      const items = sorted(s.details).map((d) => d.skillDetailName);
      L.push(items.length ? `${s.skillName}: ${items.join(', ')}` : s.skillName);
    }
  }

  if (data.experience.length) {
    head('Experience');
    for (const e of sorted(data.experience)) {
      L.push(`${e.designation}${e.companyName ? ' — ' + e.companyName : ''}`);
      L.push(dateRange(e.startDate, e.endDate));
      for (const d of sorted(e.details)) L.push(`- ${d.experienceDetailName}`);
      L.push('');
    }
  }

  if (data.projects.length) {
    head('Projects');
    for (const p of sorted(data.projects)) {
      L.push(`${p.projectName}${p.contextName ? ' — ' + p.contextName : ''}`);
      if (p.technologies) L.push(`Technologies: ${p.technologies}`);
      L.push('');
    }
  }

  if (data.education.length) {
    head('Education');
    for (const ed of sorted(data.education)) {
      L.push(`${ed.degree}, ${ed.instituteName}`);
      const gpa =
        ed.cgpa != null ? `CGPA ${ed.cgpa}` : ed.gpa != null ? `GPA ${ed.gpa}` : '';
      L.push([dateRange(ed.startDate, ed.endDate), gpa].filter(Boolean).join(' | '));
    }
  }

  if (data.certifications.length) {
    head('Certifications');
    for (const cert of sorted(data.certifications)) {
      const exp = cert.doesNotExpire
        ? 'No expiry'
        : cert.expiryDate
          ? `Expires ${monthYear(cert.expiryDate)}`
          : '';
      L.push(
        `${cert.certificationName} — ${cert.issuerName} (Issued ${monthYear(cert.issueDate)}${
          exp ? ', ' + exp : ''
        })`
      );
    }
  }

  if (data.awards.length) {
    head('Awards');
    for (const a of sorted(data.awards)) {
      L.push(`${a.awardName} (${dateRange(a.startDate, a.endDate)})`);
    }
  }

  return L.join('\n').replace(/\n{3,}/g, '\n\n').trim() + '\n';
}

/** Markdown (.md) — readable and still fully text-based for ATS. */
export function buildMarkdown(data: ResumeData, c: ResumeContact): string {
  const L: string[] = [];
  L.push(`# ${c.owner}`, `**${c.role}**  `);
  L.push([c.location, c.email].filter(Boolean).join(' · '));
  if (c.links.length) L.push(c.links.map((l) => `[${l.label}](${l.url})`).join(' · '));

  if (c.summary) L.push('', '## Summary', c.summary);

  if (data.skills.length) {
    L.push('', '## Skills');
    for (const s of sorted(data.skills)) {
      const items = sorted(s.details).map((d) => d.skillDetailName);
      L.push(`- **${s.skillName}**${items.length ? ': ' + items.join(', ') : ''}`);
    }
  }

  if (data.experience.length) {
    L.push('', '## Experience');
    for (const e of sorted(data.experience)) {
      L.push(`### ${e.designation}${e.companyName ? ' — ' + e.companyName : ''}`);
      L.push(`*${dateRange(e.startDate, e.endDate)}*`);
      for (const d of sorted(e.details)) L.push(`- ${d.experienceDetailName}`);
    }
  }

  if (data.projects.length) {
    L.push('', '## Projects');
    for (const p of sorted(data.projects)) {
      L.push(`### ${p.projectName}${p.contextName ? ' — ' + p.contextName : ''}`);
      if (p.technologies) L.push(`*Technologies:* ${p.technologies}`);
    }
  }

  if (data.education.length) {
    L.push('', '## Education');
    for (const ed of sorted(data.education)) {
      const gpa = ed.cgpa != null ? `CGPA ${ed.cgpa}` : ed.gpa != null ? `GPA ${ed.gpa}` : '';
      L.push(`- **${ed.degree}**, ${ed.instituteName} — ${dateRange(ed.startDate, ed.endDate)}${gpa ? ' · ' + gpa : ''}`);
    }
  }

  if (data.certifications.length) {
    L.push('', '## Certifications');
    for (const cert of sorted(data.certifications)) {
      L.push(`- **${cert.certificationName}** — ${cert.issuerName} (Issued ${monthYear(cert.issueDate)})`);
    }
  }

  if (data.awards.length) {
    L.push('', '## Awards');
    for (const a of sorted(data.awards)) {
      L.push(`- ${a.awardName} (${dateRange(a.startDate, a.endDate)})`);
    }
  }

  return L.join('\n').replace(/\n{3,}/g, '\n\n').trim() + '\n';
}

/** Triggers a client-side file download (browser only). */
export function downloadText(filename: string, content: string, mime: string): void {
  const blob = new Blob([content], { type: `${mime};charset=utf-8` });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = filename;
  a.click();
  URL.revokeObjectURL(url);
}
