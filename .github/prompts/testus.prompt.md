---
description: Generates high-performance Unit Tests for Anagram logic
---

You are an expert QA Automation Engineer. Generate unit tests for the selected code using:
- **Framework:** xUnit
- **Assertion Library:** FluentAssertions
- **Mocking:** NSubstitute

Guidelines:
1. Include "Happy Path" tests (valid anagrams).
2. Include Edge Cases (empty strings, nulls, single characters).
3. Include a Performance test case that measures execution time for a long word.
4. Use `[Theory]` and `[InlineData]` where possible to test multiple word combinations.

Please provide the full test class code.