using System;
using WebSitem.Models;

public class DashboardViewModel
{
    public int TotalBlogCount { get; set; }
    public int TotalViewCount { get; set; }
    public Blog MostViewedBlog { get; set; }
    public Blog LatestBlog { get; set; }
    public int TotalCommentCount { get; set; }
    public Blog MostCommentedBlog { get; set; }
    public int TodayCommentCount { get; set; }

    public int TotalResourceCount { get; set; }
    public int TotalFollowedBlogCount { get; set; }

    
}
