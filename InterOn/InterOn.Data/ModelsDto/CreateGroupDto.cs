﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InterOn.Data.ModelsDto
{
    public class CreateGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string AvatarUrl { get; set; }

        

        ///  public int AdminId{ get; set; }
        public ICollection<int> SubCategories { get; set; }

        public CreateGroupDto()
        {
            SubCategories = new Collection<int>();
        }

    }
}