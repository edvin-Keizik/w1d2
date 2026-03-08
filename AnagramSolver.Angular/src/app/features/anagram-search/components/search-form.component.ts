import { Component, input, output } from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-search-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './search-form.component.html',
  styleUrl: './search-form.component.scss',
})
export class SearchFormComponent {
  minLength = input(3);
  isLoading = input(false);
  search = output<string>();

  wordControl = new FormControl('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(3)],
  });

  constructor() {}

  ngOnInit(): void {
    // Update validators when minLength changes — signal input
    const ml = this.minLength();
    if (ml !== 3) {
      this.wordControl.setValidators([Validators.required, Validators.minLength(ml)]);
      this.wordControl.updateValueAndValidity();
    }
  }

  onSubmit(): void {
    if (this.wordControl.valid) {
      this.search.emit(this.wordControl.value.trim());
    }
  }
}
