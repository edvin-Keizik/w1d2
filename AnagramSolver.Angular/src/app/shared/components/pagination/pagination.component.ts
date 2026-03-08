import { Component, input, output, computed } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
})
export class PaginationComponent {
  currentPage = input.required<number>();
  totalCount = input.required<number>();
  pageSize = input(90);
  pageChange = output<number>();

  totalPages = computed(() => Math.ceil(this.totalCount() / this.pageSize()));
}
