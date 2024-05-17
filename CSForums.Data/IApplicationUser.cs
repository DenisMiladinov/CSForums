﻿using CSForums.Data.Models;

namespace CSForums.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById (string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task IncrementRating(string id, Type type);

    }
}
