namespace WebApp.SharedKernel.Dtos.Response
{
    public class PagerResponseDto
    {
        private int _maxPaginationWidth;
        private int? _preCurrentPagesNumber;
        private int? _nextCurrentPagesNumber;

        public int TotalCount { get; private set; }
        public int? Skip { get; private set; }
        public int? Take { get; private set; }
        public int? TotalPages { get; private set; }

        public int? StartPage { get; private set; }
        public int? CurrentPage { get; private set; }
        public int? EndPage { get; private set; }

        public int MaxPaginationWidth
        {
            get => _maxPaginationWidth;
            private set => _maxPaginationWidth = value > 2 ? value : 3;
        }


        public PagerResponseDto(int totalCount, int? pageSize, int? currentPage, int maxPaginationWidth = 5)
        {
            TotalCount = totalCount;
            MaxPaginationWidth = maxPaginationWidth;

            if (TotalCount > 0 && pageSize.HasValue && pageSize.Value > 0 && currentPage.HasValue)
            {
                Take = pageSize;
                TotalPages = (int)Math.Ceiling(TotalCount / (decimal)Take.Value);

                CurrentPage = currentPage.Value > 0 ? currentPage.Value : 1;

                if (TotalPages.HasValue && CurrentPage.Value > TotalPages.Value)
                    CurrentPage = TotalPages.Value;
                Skip = (CurrentPage.Value - 1) * (Take.HasValue ? Take.Value : 0);

                MeasurePreAndNextPagesNumber();

                StartPage = CurrentPage.Value - _preCurrentPagesNumber;
                EndPage = CurrentPage + _nextCurrentPagesNumber;

                if (StartPage <= 0)
                {
                    EndPage = EndPage - (StartPage - 1);
                    StartPage = 1;
                }
                if (EndPage > TotalPages)
                {
                    EndPage = TotalPages;
                    if (EndPage > MaxPaginationWidth)
                        StartPage = EndPage - (MaxPaginationWidth - 1);
                    else
                        StartPage = 1;
                }
            }
        }

        private void MeasurePreAndNextPagesNumber()
        {
            _nextCurrentPagesNumber = MaxPaginationWidth / 2;
            _preCurrentPagesNumber = IsEven(MaxPaginationWidth) ? _nextCurrentPagesNumber - 1 : _nextCurrentPagesNumber;
        }

        private bool IsEven(int number) => number % 2 == 0;

    }
}
