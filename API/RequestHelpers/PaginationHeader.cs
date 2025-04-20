namespace API.RequestHelpers;

public class PaginationHeader
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public PaginationHeader(int pageIndex, int pageSize, int totalitems)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalItems = totalitems;
        TotalPages = (int)Math.Ceiling(totalitems / (double)pageSize);
    }
}
