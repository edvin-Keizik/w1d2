import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { AddButtonComponent } from '../../../shared/add-button/add-button.component';
@Component({
  selector: 'app-word-form',
  standalone: true,
  imports: [ReactiveFormsModule, AddButtonComponent],
  templateUrl: './word-form.component.html',
  styleUrl: './word-form.component.scss',
})
export class WordFormComponent {
  minLength = input(3);
  isLoading = input(false);
  submitWord = output<string>();

  wordControl = new FormControl('', {
    nonNullable: true,
    validators: [
      Validators.required,
      Validators.minLength(3),
      Validators.pattern(/^[\p{L}]+$/u),
    ],
  });

  isDirty(): boolean {
    return this.wordControl.dirty;
  }

  onSubmit(): void {
    if (this.wordControl.valid) {
      this.submitWord.emit(this.wordControl.value.trim());
    }
  }

  reset(): void {
    this.wordControl.reset();
  }
}
