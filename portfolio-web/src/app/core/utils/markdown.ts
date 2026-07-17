/**
 * Tiny, dependency-free Markdown → HTML converter for admin-authored blog
 * content. Input is HTML-escaped first, so raw HTML in the source is inert;
 * only the Markdown subset below is rendered. Runs fine during SSR.
 */
export function markdownToHtml(md: string): string {
  if (!md) return '';

  let s = md.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

  // Fenced code blocks
  s = s.replace(/```([\s\S]*?)```/g, (_m, code: string) => `<pre><code>${code.trim()}</code></pre>`);

  // Headings
  s = s
    .replace(/^###### (.*)$/gm, '<h6>$1</h6>')
    .replace(/^##### (.*)$/gm, '<h5>$1</h5>')
    .replace(/^#### (.*)$/gm, '<h4>$1</h4>')
    .replace(/^### (.*)$/gm, '<h3>$1</h3>')
    .replace(/^## (.*)$/gm, '<h2>$1</h2>')
    .replace(/^# (.*)$/gm, '<h1>$1</h1>');

  // Blockquotes
  s = s.replace(/^> (.*)$/gm, '<blockquote>$1</blockquote>');

  // Inline: bold, italic, code, links
  s = s
    .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
    .replace(/(^|[^*])\*([^*\n]+?)\*/g, '$1<em>$2</em>')
    .replace(/`([^`]+?)`/g, '<code>$1</code>')
    .replace(/\[([^\]]+)\]\((https?:\/\/[^\s)]+)\)/g, '<a href="$2" target="_blank" rel="noopener">$1</a>');

  // Unordered lists
  s = s.replace(/(?:^|\n)((?:- .*(?:\n|$))+)/g, (_m, block: string) => {
    const items = block
      .trim()
      .split('\n')
      .map((l) => `<li>${l.replace(/^- /, '').trim()}</li>`)
      .join('');
    return `\n<ul>${items}</ul>\n`;
  });

  // Paragraphs
  return s
    .split(/\n{2,}/)
    .map((b) => b.trim())
    .filter(Boolean)
    .map((b) => (/^<(h\d|ul|ol|pre|blockquote)/.test(b) ? b : `<p>${b.replace(/\n/g, '<br>')}</p>`))
    .join('\n');
}
