using ApplicationCore.Consts;
using ApplicationCore.Views.Docs;
using ApplicationCore.Views.Identity;
using Infrastructure.Helpers;
using Infrastructure.Views;

namespace Do3Api.Models;

public class PostLabels
{
   public string Type => "類型";
   public string ClientId => "ClientId";
   public string Name => "名稱";
   public string Url => "Url";
   public string Api => "Api";
   public string Roles => "角色";
   public string Ps => "備註";
}
public class PostsIndexModel
{
   public PostsIndexModel(PostsFetchRequest request)
   {
      Request = request;
   }
   public PostLabels Labels => new PostLabels();
   public ICollection<ReaderViewModel> Readers { get; set; }
   public PostsFetchRequest Request { get; set; }
}

public class PostsFetchRequest
{
   public PostsFetchRequest(string type)
   {
      Type = type;
   }
   public string Type { get; set; }
}
