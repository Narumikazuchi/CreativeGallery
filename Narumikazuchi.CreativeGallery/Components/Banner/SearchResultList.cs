using Narumikazuchi.CreativeGallery.Data.Search;

namespace Narumikazuchi.CreativeGallery.Components.Banner;

public sealed partial class SearchResultList
{
    public void Add(SearchResultModel item)
    {
        if (m_Count.Equals(obj: m_Items.Length) is true)
        {
            SearchResultModel[] newArray = new SearchResultModel[m_Items.Length * 2];
            Array.Copy(sourceArray: m_Items,
                       destinationArray: newArray,
                       length: m_Count);
            m_Items = newArray;
        }

        if (m_Count is 0)
        {
            Monitor.Enter(obj: m_Items);
            {
                m_Items[m_Count] = item;
                m_Count++;
            }
            Monitor.Exit(obj: m_Items);
        }
        else if (m_Count is 1)
        {
            if (this.Comparison.Invoke(x: item,
                                       y: m_Items[0]) >= 0)
            {
                Monitor.Enter(obj: m_Items);
                {
                    m_Items[m_Count] = item;
                    m_Count++;
                }
                Monitor.Exit(obj: m_Items);
            }
            else
            {
                Monitor.Enter(obj: m_Items);
                {
                    Array.Copy(sourceArray: m_Items,
                               destinationArray: m_Items,
                               sourceIndex: 0,
                               destinationIndex: 1,
                               length: m_Count);
                }
                Monitor.Exit(obj: m_Items);
            }
        }
        else
        {
            Int32 countOnStack = m_Count;
            for (Int32 index = 0;
                 index < countOnStack;
                 index++)
            {
                if (this.Comparison.Invoke(x: item,
                                           y: m_Items[0]) < 0)
                {
                    Monitor.Enter(obj: m_Items);
                    {
                        Array.Copy(sourceArray: m_Items,
                                   destinationArray: m_Items,
                                   sourceIndex: index,
                                   destinationIndex: index + 1,
                                   length: m_Count - index);
                    }
                    Monitor.Exit(obj: m_Items);
                    return;
                }
            }
        }
    }

    public void Clear()
    {
        m_Count = 0;
        Array.Clear(array: m_Items);
    }

    public Enumerator GetEnumerator()
    {
        return new(list: this);
    }

    public required Comparison<SearchResultModel> Comparison
    {
        get;
        init;
    }

    public Int32 Count
    {
        get
        {
            return m_Count;
        }
    }

    private SearchResultModel[] m_Items = new SearchResultModel[64];
    private Int32 m_Count = 0;
}