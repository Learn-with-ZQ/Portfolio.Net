import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { ResumeData } from '../../core/models/portfolio.models';
import { AssistantReply, answer } from '../../core/services/assistant-engine';
import { PortfolioApiService } from '../../core/services/portfolio-api.service';
import { SITE } from '../../core/content/site-content';

interface ChatMessage {
  from: 'user' | 'assistant';
  text: string;
  reply?: AssistantReply;
}

@Component({
  selector: 'app-assistant',
  imports: [FormsModule, RouterLink, MatButtonModule, MatIconModule],
  templateUrl: './assistant.html',
  styleUrl: './assistant.scss'
})
export class AssistantPage {
  private readonly api = inject(PortfolioApiService);
  readonly site = SITE;

  readonly messages = signal<ChatMessage[]>([
    {
      from: 'assistant',
      text:
        `Hi — I'm the portfolio assistant. I answer questions about ${SITE.owner} ` +
        `using the live portfolio data. Ask me anything below, or tap a suggestion.`
    }
  ]);
  readonly draft = signal('');

  readonly suggestions = [
    'Tell me about Zaid',
    'Show his .NET experience',
    'Show ERP experience',
    'Show banking experience',
    'Show projects',
    'Show education'
  ];

  private knowledge: ResumeData | null = null;

  constructor() {
    // Load the knowledge base once; answers degrade gracefully until it arrives.
    this.api.getResume().subscribe({
      next: (data) => (this.knowledge = data),
      error: () => (this.knowledge = null)
    });
  }

  ask(question?: string): void {
    const text = (question ?? this.draft()).trim();
    if (!text) return;
    this.draft.set('');

    const reply = answer(text, this.knowledge);
    this.messages.update((msgs) => [
      ...msgs,
      { from: 'user', text },
      { from: 'assistant', text: reply.text, reply }
    ]);
  }
}
