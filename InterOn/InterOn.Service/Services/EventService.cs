﻿using System.Threading.Tasks;
using AutoMapper;
using InterOn.Data.DbModels;
using InterOn.Data.ModelsDto.Event;
using InterOn.Repo.Interfaces;
using InterOn.Service.Interfaces;

namespace InterOn.Service.Services
{
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IEventRepository _repository;

        public EventService(IMapper mapper,IEventRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CreateEventDto> CreateEventAsync(CreateEventDto eventDto)
        {
            var eventt = _mapper.Map<CreateEventDto, Event>(eventDto);

            await _repository.AddAsyn(eventt);
            await _repository.SaveAsync();

            var result = _mapper.Map<Event, CreateEventDto>(eventt);

            return result;
        }

        public async Task<CreateEventDto> CreateEventForGroupAsync(CreateEventDto eventDto, int groupId)
        {
            var eventforgroup = _mapper.Map<CreateEventDto, Event>(eventDto);
            eventforgroup.GroupId = groupId;
            await _repository.AddAsyn(eventforgroup);
            await _repository.SaveAsync();

            var result = _mapper.Map<Event, CreateEventDto>(eventforgroup);
            return result;
        }

        public async Task<UpdateEventDto> UpdateEventAsync(int id, UpdateEventDto eventDto)
        {
            var eventt = await _repository.GetAsync(id);
            _mapper.Map(eventDto, eventt);
            await _repository.SaveAsync();
            eventt = await _repository.GetAsync(eventt.Id);
            var result = _mapper.Map<Event,UpdateEventDto>(eventt);
            return result;
        }

        public async Task<bool> ExistEvent(int id)
        {
            return await _repository.Exist(e => e.Id == id);
        }

        public async Task<bool> ExistGroup(int id)
        {
            return await _repository.IfGroupExist(id);
        }
    }
}