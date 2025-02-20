﻿using PetStore.Data.Models;
using PetStore.Services.Models.Pet;
using System;
using System.Collections.Generic;

namespace PetStore.Services
{
    public interface IPetService
    {
        IEnumerable<PetListingServiceModel> All();
        void BuyPet(Gender gender, DateTime dateOfBirth, decimal price, string description, int breedId, int categoryId);

        void SellPet(int petId, int userId);

        bool Exists(int petId);
    }
}
