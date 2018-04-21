﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InterOn.Data.DbModels;
using InterOn.Data.ModelsDto.Group;
using InterOn.Repo.Interfaces;
using InterOn.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InterOn.Service.Services
{
    public class GroupPhotoService : IGroupPhotoService
    {
        private readonly IRepository<GroupPhoto> _repository;
        private readonly IMapper _mapper;

        public GroupPhotoService(IRepository<GroupPhoto> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GroupPhoto> UploadPhoto(int groupId, IFormFile file, string uploadsFolderPath)
        {
            if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new GroupPhoto {FileName = fileName, GroupRef = groupId};
            await _repository.AddAsyn(photo);
            await _repository.SaveAsync();
            return photo;
        }

        public async Task RemovePhoto(int groupId)
        {
            var photo = await _repository.FindBy(g => g.GroupRef == groupId).SingleAsync();
                _repository.Remove(photo);      
        }
        public GroupPhotoDto MapPhoto(GroupPhoto photo)
        {
            var result = _mapper.Map<GroupPhoto, GroupPhotoDto>(photo);
            return result;
        }

        public async Task<bool> IsExist(int id)
        {
            return await _repository.Exist(p => p.GroupRef == id);
        }

        public IEnumerable<GetGroupPhotoDto> MapPhotoDtoQueryable(IEnumerable<GroupPhoto> photo)
        {
            var result = _mapper.Map<IEnumerable<GroupPhoto>, IEnumerable<GetGroupPhotoDto>>(photo);
            return result;
        }

        public async Task<GroupPhoto> GetPhoto(string fileName)
        {
            var photo = await _repository.FindBy(g => g.FileName == fileName).SingleAsync();
            return photo;
        }

        public async Task<GroupPhoto> GetGroupPhoto(int id)
        {
            var photo = await _repository.FindBy(g => g.GroupRef == id).SingleAsync();
            return photo;
        }

      
    }
}