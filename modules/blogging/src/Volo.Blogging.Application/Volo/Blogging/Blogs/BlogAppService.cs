﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Blogs
{
    public class BlogAppService : ApplicationService, IBlogAppService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogAppService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<PagedResultDto<BlogDto>> GetListPagedAsync(PagedAndSortedResultRequestDto input)
        {
            var blogs = await _blogRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount );

            var totalCount = await _blogRepository.GetTotalBlogCount();

            var dtos = ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs);

            return new PagedResultDto<BlogDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<BlogDto>> GetListAsync()
        {
            var blogs = await _blogRepository.GetListAsync();

            return new ListResultDto<BlogDto>(
                ObjectMapper.Map<List<Blog>, List<BlogDto>>(blogs)
            );
        }

        public async Task<BlogDto> GetByShortNameAsync(string shortName)
        {
            var blog = await _blogRepository.FindByShortNameAsync(shortName);

            if (blog == null)
            {
                throw new EntityNotFoundException(typeof(Blog), shortName);
            }

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        public async Task<BlogDto> GetAsync(Guid id)
        {
            var blog = await _blogRepository.GetAsync(id);

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(BloggingPermissions.Blogs.Create)]
        public async Task<BlogDto> Create(CreateBlogDto input)
        {
            var newBlog = await _blogRepository.InsertAsync(new Blog(GuidGenerator.Create(), input.Name, input.ShortName)
            {
                Description = input.Description,
                Facebook = input.Facebook,
                Twitter = input.Twitter,
                Instagram = input.Instagram,
                Github = input.Github,
                StackOverflow = input.StackOverflow
            });

            return ObjectMapper.Map<Blog, BlogDto>(newBlog);
        }

        [Authorize(BloggingPermissions.Blogs.Update)]
        public async Task<BlogDto> Update(Guid id, UpdateBlogDto input)
        {
            var blog = await _blogRepository.GetAsync(id);

            blog.SetName(input.Name);
            blog.SetShortName(input.ShortName);
            blog.Description = input.Description;
            blog.Facebook = input.Facebook;
            blog.Twitter = input.Twitter;
            blog.Instagram = input.Instagram;
            blog.Github = input.Github;
            blog.StackOverflow = input.StackOverflow;

            return ObjectMapper.Map<Blog, BlogDto>(blog);
        }

        [Authorize(BloggingPermissions.Blogs.Delete)]
        public async Task Delete(Guid id)
        {
            await _blogRepository.DeleteAsync(id);
        }
    }
}
