import { SITE } from '../content/site-content';
import { ResumeData } from '../models/portfolio.models';
import { dateRange } from '../utils/format';

export interface AssistantItem {
  title: string;
  subtitle?: string;
  link?: string;
}

export interface AssistantReply {
  text: string;
  items?: AssistantItem[];
  linkLabel?: string;
  linkPath?: string;
}

const STOPWORDS = new Set([
  'show', 'me', 'his', 'her', 'the', 'a', 'an', 'of', 'in', 'on', 'at', 'to', 'and', 'or',
  'tell', 'about', 'what', 'is', 'are', 'zaid', 'zaids', 'does', 'do', 'have', 'has', 'with',
  'experience', 'work', 'worked', 'projects', 'project', 'please', 'can', 'you', 'list', 'any'
]);

/**
 * Deterministic, data-grounded portfolio assistant. Every answer is derived
 * from the live API data (`ResumeData`) or the static profile — it cannot
 * invent facts. Intent matching is keyword-based on purpose: predictable,
 * SSR-safe and dependency-free.
 */
export function answer(question: string, data: ResumeData | null): AssistantReply {
  const q = question.toLowerCase().trim();

  if (!q) return suggest();

  if (/(who|about|introduce|summary|profile|tell)/.test(q) && !/(project|experience|education|skill|certific|award)/.test(q)) {
    return aboutReply(data);
  }

  if (/(experience|career|job|work|role|company|companies)/.test(q)) {
    return experienceReply(q, data);
  }

  if (/(project|built|build|portfolio piece)/.test(q)) {
    return projectsReply(q, data);
  }

  if (/(education|degree|study|studied|university|bs |ms |master|bachelor)/.test(q)) {
    return educationReply(data);
  }

  if (/(skill|stack|technolog|tools|framework)/.test(q)) {
    return skillsReply(data);
  }

  if (/(certific|credential|badge)/.test(q)) {
    return certificationsReply(data);
  }

  if (/(award|achievement|recognition|competition|medal)/.test(q)) {
    return awardsReply(data);
  }

  if (/(contact|hire|email|reach|available|meeting)/.test(q)) {
    return {
      text: `You can reach ${SITE.owner} at ${SITE.email} (${SITE.location}). He is open to opportunities.`,
      linkLabel: 'Go to contact page',
      linkPath: '/contact'
    };
  }

  if (/(resume|cv|download)/.test(q)) {
    return {
      text: 'The ATS-friendly resume is generated live from this portfolio and can be viewed, printed or exported.',
      linkLabel: 'Open resume',
      linkPath: '/resume'
    };
  }

  // Free-text keyword search across experience + projects (e.g. "ERP", "banking").
  const keywordReply = keywordSearch(q, data);
  return keywordReply ?? suggest();
}

function suggest(): AssistantReply {
  return {
    text:
      "I answer from the live portfolio data. Try one of these: “Tell me about Zaid”, " +
      "“Show his .NET experience”, “Show ERP experience”, “Show banking experience”, " +
      "“Show projects”, “Show education”."
  };
}

function aboutReply(data: ResumeData | null): AssistantReply {
  const counts = data
    ? ` Across this portfolio: ${data.experience.length} roles, ${data.projects.length} projects, ` +
      `${data.skills.length} skill areas and ${data.certifications.length} certifications.`
    : '';
  return {
    text:
      `${SITE.owner} is a ${SITE.role} at ${SITE.company}, currently ${SITE.assignment}. ` +
      `${SITE.about.intro}${counts}`,
    linkLabel: 'Read the full story',
    linkPath: '/about'
  };
}

function experienceReply(q: string, data: ResumeData | null): AssistantReply {
  if (!data) return loadingReply();
  const terms = extractTerms(q);
  const matches = data.experience.filter((e) => {
    if (!terms.length) return true;
    const hay = `${e.designation} ${e.companyName ?? ''} ${e.deployedTo ?? ''} ${e.details
      .map((d) => d.experienceDetailName)
      .join(' ')}`.toLowerCase();
    return terms.some((t) => hay.includes(t));
  });

  if (!matches.length) {
    return {
      text: terms.length
        ? `No roles explicitly mention “${terms.join(', ')}”, but here is the full experience timeline.`
        : 'Here is the experience timeline.',
      linkLabel: 'View experience',
      linkPath: '/experience'
    };
  }

  return {
    text: terms.length
      ? `Found ${matches.length} role${matches.length > 1 ? 's' : ''} matching “${terms.join(', ')}”:`
      : `${SITE.owner} has ${matches.length} role${matches.length > 1 ? 's' : ''} on record:`,
    items: matches.map((e) => ({
      title: `${e.designation}${e.companyName ? ' — ' + e.companyName : ''}`,
      subtitle: dateRange(e.startDate, e.endDate)
    })),
    linkLabel: 'Full timeline',
    linkPath: '/experience'
  };
}

function projectsReply(q: string, data: ResumeData | null): AssistantReply {
  if (!data) return loadingReply();
  const terms = extractTerms(q);
  const matches = data.projects.filter((p) => {
    if (!terms.length) return true;
    const hay = `${p.projectName} ${p.contextName ?? ''} ${p.technologies}`.toLowerCase();
    return terms.some((t) => hay.includes(t));
  });

  if (!matches.length) {
    return { text: 'No projects match that — browse them all instead.', linkLabel: 'View projects', linkPath: '/projects' };
  }

  return {
    text: `${matches.length} project${matches.length > 1 ? 's' : ''}${terms.length ? ` matching “${terms.join(', ')}”` : ''}:`,
    items: matches.slice(0, 6).map((p) => ({
      title: p.projectName,
      subtitle: p.technologies || p.contextName,
      link: `/projects/${p.projectId}`
    })),
    linkLabel: 'All projects',
    linkPath: '/projects'
  };
}

function educationReply(data: ResumeData | null): AssistantReply {
  const base = `${SITE.owner} holds a ${SITE.education.completed} and is pursuing an ${SITE.education.inProgress}.`;
  if (!data?.education.length) {
    return { text: base, linkLabel: 'Education details', linkPath: '/education' };
  }
  return {
    text: base,
    items: data.education.map((e) => ({
      title: e.degree,
      subtitle: `${e.instituteName} · ${dateRange(e.startDate, e.endDate)}${e.cgpa != null ? ` · CGPA ${e.cgpa}` : ''}`
    })),
    linkLabel: 'Education details',
    linkPath: '/education'
  };
}

function skillsReply(data: ResumeData | null): AssistantReply {
  if (!data?.skills.length) return loadingReply();
  return {
    text: `Core skill areas (${data.skills.length}):`,
    items: data.skills.map((s) => ({
      title: s.skillName,
      subtitle: s.details.map((d) => d.skillDetailName).join(', ') || undefined
    })),
    linkLabel: 'Skill visualizations',
    linkPath: '/skills'
  };
}

function certificationsReply(data: ResumeData | null): AssistantReply {
  if (!data?.certifications.length) {
    return { text: 'Certifications are listed on the certifications page.', linkLabel: 'View certifications', linkPath: '/certifications' };
  }
  return {
    text: `${data.certifications.length} certification${data.certifications.length > 1 ? 's' : ''} on record:`,
    items: data.certifications.map((c) => ({ title: c.certificationName, subtitle: c.issuerName })),
    linkLabel: 'All certifications',
    linkPath: '/certifications'
  };
}

function awardsReply(data: ResumeData | null): AssistantReply {
  if (!data?.awards.length) {
    return { text: 'Awards and recognition are on the awards page.', linkLabel: 'View awards', linkPath: '/awards' };
  }
  return {
    text: `${data.awards.length} award${data.awards.length > 1 ? 's' : ''}:`,
    items: data.awards.map((a) => ({ title: a.awardName, subtitle: dateRange(a.startDate, a.endDate) })),
    linkLabel: 'All awards',
    linkPath: '/awards'
  };
}

function keywordSearch(q: string, data: ResumeData | null): AssistantReply | null {
  if (!data) return null;
  const terms = extractTerms(q);
  if (!terms.length) return null;

  const items: AssistantItem[] = [];
  for (const e of data.experience) {
    const hay = `${e.designation} ${e.companyName ?? ''} ${e.details.map((d) => d.experienceDetailName).join(' ')}`.toLowerCase();
    if (terms.some((t) => hay.includes(t))) {
      items.push({ title: `${e.designation}${e.companyName ? ' — ' + e.companyName : ''}`, subtitle: 'Experience' });
    }
  }
  for (const p of data.projects) {
    const hay = `${p.projectName} ${p.technologies}`.toLowerCase();
    if (terms.some((t) => hay.includes(t))) {
      items.push({ title: p.projectName, subtitle: 'Project', link: `/projects/${p.projectId}` });
    }
  }
  if (!items.length) return null;
  return { text: `Here is what mentions “${terms.join(', ')}”:`, items: items.slice(0, 8) };
}

function extractTerms(q: string): string[] {
  return q
    .replace(/[?.,!]/g, ' ')
    .split(/\s+/)
    .map((w) => w.trim().toLowerCase())
    .filter((w) => w.length > 1 && !STOPWORDS.has(w));
}

function loadingReply(): AssistantReply {
  return { text: 'Portfolio data is still loading (or the API is offline) — try again in a moment.' };
}
