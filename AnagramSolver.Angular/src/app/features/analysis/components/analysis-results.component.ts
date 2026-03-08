import { Component, input } from '@angular/core';
import { FrequencyAnalysisResult } from '../../../core/models/analysis.model';

@Component({
  selector: 'app-analysis-results',
  standalone: true,
  templateUrl: './analysis-results.component.html',
  styleUrl: './analysis-results.component.scss',
})
export class AnalysisResultsComponent {
  data = input.required<FrequencyAnalysisResult>();
}
