﻿using System.Linq;
using AutoMapper;
using InterOn.Data.DbModels;
using InterOn.Data.ModelsDto;
using InterOn.Data.ModelsDto.Category;
using InterOn.Data.ModelsDto.Comments;
using InterOn.Data.ModelsDto.Event;
using InterOn.Data.ModelsDto.Group;
using InterOn.Data.ModelsDto.Post;

namespace InterOn.Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<User, UserDto>();
            //CreateMap<UserDto, User>();
            CreateMap<Event, Event>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.User, opt => opt.Ignore())
                .ForMember(e => e.DateTimeEvent, opt => opt.Ignore())
                .ForMember(e => e.Description, opt => opt.Ignore())
                .ForMember(e => e.GroupId, opt => opt.Ignore())
                .ForMember(e => e.Latitude, opt => opt.Ignore())
                .ForMember(e => e.Longitude, opt => opt.Ignore())
                .ForMember(e => e.Posts, opt => opt.Ignore())
                .ForMember(e => e.SubCategories, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore());
            CreateMap<Group, Group>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.User, opt => opt.Ignore())
                .ForMember(e => e.CreateDateTime, opt => opt.Ignore())
                .ForMember(e => e.Description, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.Posts, opt => opt.Ignore())
                .ForMember(e => e.SubCategories, opt => opt.Ignore())
                .ForMember(e => e.UserId, opt => opt.Ignore())
                .ForMember(e => e.Users, opt => opt.Ignore());
            CreateMap<MainCategory, MainCategory>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.SubCategories, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore());
            CreateMap<SubCategory, SubCategory>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.MainCategory, opt => opt.Ignore())
                .ForMember(e => e.Name, opt => opt.Ignore())
                .ForMember(e => e.MainCategoryId, opt => opt.Ignore())
                .ForMember(e => e.Groups, opt => opt.Ignore())
                .ForMember(e => e.Events, opt => opt.Ignore());
            //CreateMap<User,User>()
            //    .ForMember(e => e.Id, opt => opt.Ignore())
            //    .ForMember(e => e.Groups, opt => opt.Ignore())
            //    .ForMember(e => e.Username, opt => opt.Ignore())
            //    .ForMember(e => e.Email, opt => opt.Ignore())
            //    .ForMember(e => e.EmailConfirmed, opt => opt.Ignore())
            //    .ForMember(e => e.GroupAdmin, opt => opt.Ignore())
            //    .ForMember(e => e.Name, opt => opt.Ignore())
            //    .ForMember(e => e.PasswordHash, opt => opt.Ignore())
            //    .ForMember(e => e.PasswordSalt, opt => opt.Ignore())
            //    .ForMember(e => e.Posts, opt => opt.Ignore())
            //    .ForMember(e => e.Surname, opt => opt.Ignore())
            //    .ForMember(e => e.Events, opt => opt.Ignore());

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            CreateMap<UserRoleDto, UserRole>();
            CreateMap<UserRole, UserRoleDto>();

            CreateMap<UserToken, UserTokenDto>();
            CreateMap<UserTokenDto, UserToken>();

            CreateMap<CreateUserDto, User>()
                .ForMember(u=>u.Groups,opt=>opt.Ignore());
            CreateMap<User, CreateUserDto>();

            CreateMap<LoginUserDto, User>()
                .ForMember(lu=>lu.Groups,opt=>opt.Ignore())
                .ForMember(gdt => gdt.Id, opt => opt.MapFrom(g => g.UserId));

            //Group
            CreateMap<Group, GroupUnauthorizedDto>()
                .ForMember(gdt => gdt.SubCategoriesDtos,
                    otp => otp.MapFrom(g => g.SubCategories.Select(id =>
                        new SubCategoriesDto { Id = id.SubCategoryId, Name = id.SubCategory.Name, SubCategoryPhoto = id.SubCategory.SubCategoryPhoto })))
                .ForMember(gdt=>gdt.NumberOfUsers,
                    opt=>opt.MapFrom(g=>g.Users.Count()));
            CreateMap<Group, GroupDto>()
                .ForMember(gdt => gdt.SubCategories,
                    otp => otp.MapFrom(g => g.SubCategories.Select(id =>
                        new SubCategoriesDto { Id = id.SubCategoryId, Name = id.SubCategory.Name, SubCategoryPhoto = id.SubCategory.SubCategoryPhoto })))
                .ForMember(gdt => gdt.Users,
                    opt => opt.MapFrom(g => g.Users.Select(id =>
                        new UserGroupDto { Id = id.User.Id, UserName = id.User.Username})));

            CreateMap<Group, CreateGroupDto>()
                .ForMember(gdt => gdt.SubCategories,
                    opt => opt.MapFrom(g => g.SubCategories.Select(gd => gd.SubCategoryId)));
            CreateMap<CreateGroupDto, Group>()
                .ForMember(g => g.SubCategories,
                    opt => opt.MapFrom(gdt => gdt.SubCategories.Select(id => new GroupCategory {SubCategoryId = id})))
                .ForMember(g => g.Users, opt => opt.Ignore());


            CreateMap<Group, UpdateGroupDto>()
                .ForMember(gdt => gdt.SubCategories,
                    opt => opt.MapFrom(g => g.SubCategories.Select(gd => gd.SubCategoryId)))
                .ForMember(gdt=>gdt.Users,
                    opt=>opt.MapFrom(g=>g.Users.Select(gd=>gd.UserId)));
            CreateMap<UpdateGroupDto, Group>()
                .ForMember(g => g.Id, opt => opt.Ignore())
                .ForMember(g => g.SubCategories,
                    opt => opt.MapFrom(gdt => gdt.SubCategories.Select(id => new GroupCategory {SubCategoryId = id})))
                .ForMember(g => g.SubCategories, opt => opt.Ignore())
                .ForMember(g=>g.Users,opt=>opt.Ignore())
                .AfterMap((gdto, g) =>
                {
                    //Remove
                    var removeCategories = g.SubCategories.Where(s => !gdto.SubCategories.Contains(s.SubCategoryId))
                        .ToList();
                    foreach (var s in removeCategories.ToList())
                         g.SubCategories.Remove(s);
                  //add
                    var addedCategories = gdto.SubCategories
                        .Where(id => g.SubCategories.All(s => s.SubCategoryId != id))
                        .Select(id => new GroupCategory { SubCategoryId = id })
                        .ToList();
                    foreach (var c in addedCategories.ToList())
                        g.SubCategories.Add(c);
                });
             
            //MainCategory
            CreateMap<SaveCategoryDto, MainCategory>()
                .ForMember(g => g.Id, opt => opt.Ignore());

            CreateMap<MainCategory, SaveCategoryDto>();
            CreateMap<MainCategory, MainCategoryDto>();
     
            //SubCategory
            CreateMap<SubCategoryDto, SubCategory>();
            CreateMap<SubCategory, SubCategoryDto>();
           
               
            CreateMap<SaveCategoryDto, SubCategory>()
                .ForMember(g => g.Id, opt => opt.Ignore());


            //Event

            CreateMap<CreateEventDto, Event>()
                .ForMember(g => g.SubCategories,
                    opt => opt.MapFrom(gdt =>
                        gdt.SubCategories.Select(id => new EventSubCategory {SubCategoryId = id})))
                .ForMember(e=>e.Latitude,
                    opt=>opt.MapFrom(a=>a.Address.Latitude))
                .ForMember(e => e.Longitude,
                    opt => opt.MapFrom(a => a.Address.Longitude))
                .ForMember(g => g.Users, opt => opt.Ignore())
                .ForMember(e => e.User, opt => opt.Ignore());
              
            CreateMap<Event, CreateEventDto>()
                .ForMember(gdt => gdt.SubCategories,
                    opt => opt.MapFrom(g => g.SubCategories.Select(gd => gd.SubCategoryId)));
            CreateMap<UpdateEventDto,Event>()
                .ForMember(g => g.Id, opt => opt.Ignore())
                .ForMember(g => g.SubCategories, opt => opt.Ignore())
                .ForMember(g => g.Users, opt => opt.Ignore())
                .ForMember(e=>e.User, opt => opt.Ignore())
                .ForMember(e => e.UserId, opt => opt.Ignore())
                .ForMember(e => e.Latitude,
                    opt => opt.MapFrom(a => a.Address.Latitude))
                .ForMember(e => e.Longitude,
                    opt => opt.MapFrom(a => a.Address.Longitude))
                .AfterMap((gdto, g) =>
                {
                    //Remove
                    var removeCategories = g.SubCategories.Where(s => !gdto.SubCategories.Contains(s.SubCategoryId))
                        .ToList();
                    foreach (var s in removeCategories.ToList())
                        g.SubCategories.Remove(s);
                    //add
                    var addedCategories = gdto.SubCategories
                        .Where(id => g.SubCategories.All(s => s.SubCategoryId != id))
                        .Select(id => new EventSubCategory { SubCategoryId = id })
                        .ToList();
                    foreach (var c in addedCategories.ToList())
                        g.SubCategories.Add(c);
                });
            CreateMap<Event, UpdateEventDto>()
                .ForMember(ue => ue.SubCategories,
                    opt => opt.MapFrom(ev => ev.SubCategories.Select(esc => esc.SubCategoryId)));
            CreateMap<Event, EventGroupDto>()
                .ForMember(gdt => gdt.SubCategories,
                    otp => otp.MapFrom(g => g.SubCategories.Select(id =>
                        new SubCategoriesDto {Id = id.SubCategoryId, Name = id.SubCategory.Name,SubCategoryPhoto = id.SubCategory.SubCategoryPhoto})));
            CreateMap<Event, EventDto>()
                .ForMember(gdt => gdt.SubCategories,
                    otp => otp.MapFrom(g => g.SubCategories.Select(id =>
                        new SubCategoriesDto { Id = id.SubCategoryId, Name = id.SubCategory.Name, SubCategoryPhoto = id.SubCategory.SubCategoryPhoto })));
            //post
            CreateMap<UpdateGroupPostDto, Post>()
                .ForMember(dto => dto.Id,
                    opt => opt.Ignore());
            CreateMap<CreateGroupPostDto, Post>();
            CreateMap<Post, PostGroupDto>()
                .ForMember(c => c.User,
                    opt => opt.MapFrom(a =>
                        new UserDto { AvatarUrl = a.User.AvatarUrl, Id = a.User.Id, Username = a.User.Username }));

            //comment

            CreateMap<CreateGroupPostCommentDto, Comment>();
            CreateMap<UpdateGroupPostCommentDto, Comment>();
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.User,
                    opt => opt.MapFrom(a =>
                        new UserDto {AvatarUrl = a.User.AvatarUrl, Id = a.User.Id, Username = a.User.Username}));
                
        }
    }
}
