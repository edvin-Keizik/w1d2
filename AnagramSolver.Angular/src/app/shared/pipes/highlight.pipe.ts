import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({ name: 'highlight', standalone: true })
export class HighlightPipe implements PipeTransform {
  constructor(private readonly _sanitizer: DomSanitizer) {}

  transform(text: string, search: string): SafeHtml {
    if (!search || !text) return text;

    const escaped = search.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const regex = new RegExp(`(${escaped})`, 'gi');
    const highlighted = text.replace(regex, '<mark>$1</mark>');

    return this._sanitizer.bypassSecurityTrustHtml(highlighted);
  }
}
