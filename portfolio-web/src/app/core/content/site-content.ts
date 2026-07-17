/**
 * Static profile content that has no backing API endpoint (the API does not
 * expose a bio/paragraphs or contact channel yet). Edit these values freely.
 */
export const SITE = {
  owner: 'Muhammad Zaid Qasim',
  role: 'Software Developer',
  company: 'E-Creatorz',
  assignment: 'Resident Engineer at IoBM',
  photo: 'profile.png',
  tagline:
    'Software Developer building reliable, well-architected enterprise platforms with ' +
    '.NET Core and Angular — currently a Resident Engineer at IoBM.',
  location: 'Karachi, Pakistan',
  email: 'hello@example.com', // TODO: replace with your public contact address
  phone: '', // optional, e.g. WhatsApp number in international format (no +)
  socials: [
    { label: 'GitHub', icon: 'code', url: 'https://github.com/' },
    { label: 'LinkedIn', icon: 'business_center', url: 'https://www.linkedin.com/' },
    { label: 'Email', icon: 'mail', url: 'mailto:hello@example.com' }
  ],
  education: {
    completed: 'BS Computer Science',
    inProgress: 'MS Data Science (In Progress)'
  },
  about: {
    intro:
      'Software Developer at E-Creatorz, currently serving as a Resident Engineer at IoBM. ' +
      'I design and deliver layered .NET Core systems backed by SQL Server, paired with ' +
      'modern Angular front-ends.',
    paragraphs: [
      'My work centres on maintainable enterprise systems — ERP, campus management and ' +
        'banking platforms — with clean separation of concerns, stored-procedure data access, ' +
        'robust authentication and thoughtful API design.',
      'On the front end I favour standalone Angular components, signals for state, SSR, and ' +
        'Angular Material for accessible, responsive interfaces.',
      'Alongside delivery I am pursuing an MS in Data Science, extending my backend depth into ' +
        'analytics, and I stay active in leadership and public-speaking through community work.'
    ],
    highlights: [
      '.NET Core 9 · Clean Architecture',
      'SQL Server · Dapper · Stored Procedures',
      'Angular 20 · Signals · SSR',
      'JWT Auth · Role-based Access'
    ]
  },
  /** Career-story blocks for the Home page (until a Paragraphs API is wired). */
  story: [
    {
      icon: 'work',
      title: 'Current Role',
      body:
        'Software Developer at E-Creatorz, deployed as Resident Engineer at IoBM — owning ' +
        'delivery and production support for enterprise systems.'
    },
    {
      icon: 'school',
      title: 'Current Studies',
      body: 'Pursuing an MS in Data Science, building on a BS in Computer Science.'
    },
    {
      icon: 'diversity_3',
      title: 'Leadership & Speaking',
      body:
        'Active in community leadership, organizing committees and public speaking, ' +
        'delivering events and mentoring peers.'
    },
    {
      icon: 'rocket_launch',
      title: 'Career Goals',
      body:
        'Growing toward solution architecture — combining backend engineering depth with ' +
        'data science to deliver measurable business value.'
    }
  ],
  /** Recruiter-focused positioning (curated highlights, not API-backed). */
  recruiter: {
    headline: 'Enterprise engineering that ships — and stays reliable.',
    summary:
      'Backend-focused software developer delivering ERP, banking and campus platforms end to ' +
      'end: database design, SQL optimization, resilient APIs, and hands-on production support ' +
      'that keeps critical systems running.',
    metrics: [
      { value: 'ERP · Banking · Campus', label: 'Domains delivered' },
      { value: 'Resident Engineer', label: 'On-site at IoBM' },
      { value: '.NET Core + SQL Server', label: 'Core stack' },
      { value: 'MS Data Science', label: 'In progress' }
    ],
    strengths: [
      {
        icon: 'inventory_2',
        title: 'Enterprise ERP Experience',
        body: 'Delivery and support across enterprise ERP modules used daily by real businesses.'
      },
      {
        icon: 'school',
        title: 'Campus Management Systems',
        body: 'Resident Engineer at IoBM, owning campus management workflows and integrations.'
      },
      {
        icon: 'account_balance',
        title: 'Banking Systems',
        body: 'Experience with banking-grade systems where correctness and auditability matter.'
      },
      {
        icon: 'speed',
        title: 'SQL Optimization',
        body: 'Query tuning, indexing and stored-procedure design for fast, reliable data access.'
      },
      {
        icon: 'support_agent',
        title: 'Production Support',
        body: 'On-call ownership: diagnosing, patching and stabilising live production systems.'
      },
      {
        icon: 'dns',
        title: 'Backend Expertise',
        body: 'ASP.NET Core, Web APIs, Entity Framework and clean, layered architecture.'
      },
      {
        icon: 'psychology',
        title: 'Problem Solving',
        body: 'Strong data structures & algorithms foundation applied to real delivery problems.'
      },
      {
        icon: 'insights',
        title: 'Data Science Journey',
        body: 'Extending backend depth into analytics through an in-progress MS in Data Science.'
      },
      {
        icon: 'groups',
        title: 'Leadership Experience',
        body: 'Organizing committees, mentoring and coordinating teams and events.'
      },
      {
        icon: 'trending_up',
        title: 'Business Value Delivered',
        body: 'Focused on outcomes — reliability, performance and features that move the business.'
      }
    ]
  }
} as const;
