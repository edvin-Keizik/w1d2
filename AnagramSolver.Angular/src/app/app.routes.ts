import { Routes } from '@angular/router';
import { settingsLoadedGuard } from './core/guards/settings-loaded.guard';
import { chatSessionGuard } from './core/guards/chat-session.guard';
import { unsavedChangesGuard } from './core/guards/unsaved-changes.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'search', pathMatch: 'full' },
  {
    path: 'search',
    canActivate: [settingsLoadedGuard],
    loadComponent: () =>
      import('./features/anagram-search/containers/anagram-search-page.component').then(
        (m) => m.AnagramSearchPageComponent
      ),
  },
  {
    path: 'dictionary',
    canActivate: [settingsLoadedGuard],
    loadComponent: () =>
      import('./features/dictionary/containers/dictionary-page.component').then(
        (m) => m.DictionaryPageComponent
      ),
  },
  {
    path: 'add-word',
    canDeactivate: [unsavedChangesGuard],
    loadComponent: () =>
      import('./features/add-word/containers/add-word-page.component').then(
        (m) => m.AddWordPageComponent
      ),
  },
  {
    path: 'analysis',
    canDeactivate: [unsavedChangesGuard],
    loadComponent: () =>
      import('./features/analysis/containers/analysis-page.component').then(
        (m) => m.AnalysisPageComponent
      ),
  },
  {
    path: 'chat',
    canActivate: [chatSessionGuard],
    loadComponent: () =>
      import('./features/ai-chat/containers/chat-page.component').then(
        (m) => m.ChatPageComponent
      ),
  },
  {
    path: 'chat/history',
    loadComponent: () =>
      import('./features/chat-history/containers/chat-history-page.component').then(
        (m) => m.ChatHistoryPageComponent
      ),
  },
  { path: '**', redirectTo: 'search' },
];
