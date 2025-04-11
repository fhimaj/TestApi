﻿namespace TestApi.Domain.Models.Posts
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public int AuthorId { get; set; }
    }
}
