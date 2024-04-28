namespace Blog.API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }

        //Relationship between BlogPost and Category
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}

