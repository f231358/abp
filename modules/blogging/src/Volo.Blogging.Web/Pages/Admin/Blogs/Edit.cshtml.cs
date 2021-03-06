using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Pages.Admin.Blogs
{
    public class EditModel : AbpPageModel
    {
        private readonly IBlogAppService _blogAppService;

        [BindProperty(SupportsGet = true)]
        public Guid BlogId { get; set; }

        [BindProperty]
        public BlogEditViewModel Blog { get; set; } = new BlogEditViewModel();

        public EditModel(IBlogAppService blogAppService)
        {
            _blogAppService = blogAppService;
        }

        public async Task OnGet()
        {
            var blog = await _blogAppService.GetAsync(BlogId);

            Blog = ObjectMapper.Map<BlogDto, BlogEditViewModel>(blog);
        }

        public async Task OnPost()
        {
            await _blogAppService.Update(Blog.Id, new UpdateBlogDto()
            {
                Name = Blog.Name,
                ShortName = Blog.ShortName,
                Description = Blog.Description,
                Facebook = Blog.Facebook,
                Twitter = Blog.Twitter,
                Instagram = Blog.Instagram,
                Github = Blog.Github,
                StackOverflow = Blog.StackOverflow
            });
        }

        public class BlogEditViewModel
        {
            [HiddenInput]
            [Required]
            public Guid Id { get; set; }

            [Required]
            [StringLength(BlogConsts.MaxNameLength)]
            public string Name { get; set; }

            [Required]
            [StringLength(BlogConsts.MaxShortNameLength)]
            public string ShortName { get; set; }

            [StringLength(BlogConsts.MaxDescriptionLength)]
            public string Description { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Facebook { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Twitter { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Instagram { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string Github { get; set; }

            [StringLength(BlogConsts.MaxSocialLinkLength)]
            public string StackOverflow { get; set; }
        }
    }

   
}