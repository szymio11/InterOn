﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InterOn.Data.DbModels;
using InterOn.Data.ModelsDto.Post;
using InterOn.Repo.Interfaces;
using InterOn.Service.Interfaces;

namespace InterOn.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> IfExistGroup(int groupId)
        {
           return await _repository.IfGroupExist(groupId);
        }

        public async Task<PostGroupDto> CreatePostGroupAsync(int groupId, int userId, CreateGroupPostDto createGroupPostDto)
        {
            var post = _mapper.Map<CreateGroupPostDto, Post>(createGroupPostDto);
            post.UserId = userId;
            post.GroupId = groupId;
            post.CreateDateTime = DateTime.Now;

            await _repository.AddAsyn(post);
            await _repository.SaveAsync();

            var postResult = await _repository.GetAsync(post.Id);
            var result = _mapper.Map<Post, PostGroupDto>(postResult);
            return result;
        }

        public async Task<Post> UpdatePostGroupAsync(int groupId, int postId, UpdateGroupPostDto updateGroupPost)
        {
            var post = await _repository.GetPostGroup(groupId, postId);
            updateGroupPost.UpdateDateTime = DateTime.Now;;
            _mapper.Map(updateGroupPost, post);
            await _repository.SaveAsync();
            return post;
        }

        public async Task RemovePost(int postId)
        {
          var post = await _repository.GetAsync(postId);

          _repository.Remove(post);
          await _repository.SaveAsync();
        }

        public async Task<IEnumerable<PostGroupDto>> GetAllPostsForGroupAsync(int groupId)
        {
            var posts = await _repository.FindAllAsync(g => g.GroupId == groupId);
            var postDtos = _mapper.Map<IEnumerable<Post>, IEnumerable<PostGroupDto>>(posts);

            return postDtos;
        }

        public async Task<bool> IfExistPost(int postId)
        {
           return await _repository.Exist(a => a.Id == postId);
        }

        public async Task<PostGroupDto> MapPostDto(int postId)
        {
            var post = await _repository.GetAsync(postId);
            var result = _mapper.Map<Post, PostGroupDto>(post);
            return result;
        }
    }
}