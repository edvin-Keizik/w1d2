import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { WordCountPipe } from '../../../shared/pipes/word-count.pipe';

@Component({
  selector: 'app-analysis-form',
  standalone: true,
  imports: [ReactiveFormsModule, WordCountPipe],
  templateUrl: './analysis-form.component.html',
  styleUrl: './analysis-form.component.scss',
})
export class AnalysisFormComponent {
  isLoading = input(false);
  analyze = output<string>();

  textControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.maxLength(100000)],
  });

  isDirty(): boolean {
    return this.textControl.dirty;
  }

  onSubmit(): void {
    if (this.textControl.valid) {
      this.analyze.emit(this.textControl.value);
    }
  }
}
