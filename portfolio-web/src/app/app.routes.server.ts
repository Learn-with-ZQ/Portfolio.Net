import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    // Render on demand (SSR) rather than prerendering at build time, because
    // every page depends on the live Portfolio API and a runtime auth token.
    path: '**',
    renderMode: RenderMode.Server
  }
];
