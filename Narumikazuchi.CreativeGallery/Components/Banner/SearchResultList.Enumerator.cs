using Narumikazuchi.CreativeGallery.Data.Search;

namespace Narumikazuchi.CreativeGallery.Components.Banner;

public sealed partial class SearchResultList
{
    public struct Enumerator
    {
        public Enumerator(SearchResultList list)
        {
            m_List = list;
        }

        public Boolean MoveNext()
        {
            ++m_Index;
            return m_Index < m_List.m_Count;
        }

        public SearchResultModel Current
        {
            get
            {
                return m_List.m_Items[m_Index];
            }
        }

        private Int32 m_Index = -1;

        private readonly SearchResultList m_List;
    }
}