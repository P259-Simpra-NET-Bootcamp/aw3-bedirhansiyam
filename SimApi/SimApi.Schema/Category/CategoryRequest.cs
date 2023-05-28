using SimApi.Base;

namespace SimApi.Schema;

public class CategoryRequest : BaseRequest
{
    public string Name { get; set; }
    public int Order { get; set; }
}
