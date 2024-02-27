using Microsoft.AspNetCore.Components;
using Narumikazuchi.CreativeGallery.Data.Search;
using Narumikazuchi.CreativeGallery.Infrastructure;

namespace Narumikazuchi.CreativeGallery.Components.Banner;

public sealed partial class SearchInput : IDisposable
{
    public SearchInput()
    {
        m_Results = new()
        {
            Comparison = this.Sort
        };
    }

    public void Dispose()
    {
        if (m_TokenSource.HasValue is true)
        {
            m_TokenSource.Value.Cancel();
            m_TokenSource.Value.Dispose();
            m_TokenSource = default;
        }

        if (m_Task.HasValue is true)
        {
            m_Task = default;
        }

        this.DatabaseContext.Dispose();
    }

    private Int32 Sort(SearchResultModel left,
                       SearchResultModel right)
    {
        Int32 leftScore = left.Value.IndexOf(value: m_FilterText);
        Int32 rightScore = right.Value.IndexOf(value: m_FilterText);

        Int32 result = leftScore.CompareTo(value: rightScore);
        if (result is not 0)
        {
            return left.Value.Length.CompareTo(value: right.Value.Length);
        }
        else
        {
            return left.Count.CompareTo(value: right.Count);
        }
    }

    private void HandleResultClick(SearchResultModel result)
    {
        switch (result.Type)
        {
            case SearchResultType.User:
                this.Navigator.NavigateTo(uri: $"{Routes.USERDASHBOARD}/{result.Value}");
                break;
            case SearchResultType.Album:
                this.Navigator.NavigateTo(uri: $"{Routes.ALBUM}/{result.Value}");
                break;
            case SearchResultType.Tag:
                this.Navigator.NavigateTo(uri: $"{Routes.SEARCH_BY}/tag={result.Value}");
                break;
        }
    }

    private void OnInputChanged(ChangeEventArgs eventArguments)
    {
        if (m_TokenSource.HasValue is true)
        {
            m_TokenSource.Value.Cancel();
            m_TokenSource.Value.Dispose();
            m_Task = default;
            m_TokenSource = default;
        }

        if (eventArguments.Value is not String value ||
            eventArguments.Value is null)
        {
            m_FilterText = String.Empty;
        }
        else
        {
            m_FilterText = value;
        }

        m_Results.Clear();

        if (String.IsNullOrWhiteSpace(value: m_FilterText))
        {
            return;
        }

        m_TokenSource = new CancellationTokenSource();
        IAsyncEnumerable<SearchResultModel> results = this.DatabaseContext.FindWhereAsynchronously(searchFilter: m_FilterText);
        m_Task = Task.Delay(millisecondsDelay: QUERY_DELAY_IN_MILLISECONDS,
                            cancellationToken: m_TokenSource.Value!.Token)
                     .ContinueWith(continuationFunction: (_) => this.DisplayResults(results: results,
                                                                                    cancellationToken: m_TokenSource.Value!.Token),
                                   cancellationToken: m_TokenSource.Value!.Token);

    }

    private async Task DisplayResults(IAsyncEnumerable<SearchResultModel> results,
                                      CancellationToken cancellationToken)
    {
        await foreach (SearchResultModel searchResult in results)
        {
            if (cancellationToken.IsCancellationRequested is true)
            {
                return;
            }

            m_Results.Add(item: searchResult);

            await this.InvokeAsync(workItem: this.StateHasChanged);
        }
    }

    private String IsActiveClass
    {
        get
        {
            if (String.IsNullOrWhiteSpace(value: m_FilterText) is true)
            {
                return String.Empty;
            }
            else
            {
                return ACTIVE_CLASS;
            }
        }
    }

    private const Int32 QUERY_DELAY_IN_MILLISECONDS = 250;
    private const String ACTIVE_CLASS = "active";

    private Optional<Task> m_Task;
    private Optional<CancellationTokenSource> m_TokenSource;
    private String m_FilterText = String.Empty;

    private readonly SearchResultList m_Results;
}