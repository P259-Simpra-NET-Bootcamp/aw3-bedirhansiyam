using SimApi.Base;
using SimApi.Data;

namespace SimApi.Schema;

public class ProductResponse : BaseResponse
{
    public string Category { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }

}
