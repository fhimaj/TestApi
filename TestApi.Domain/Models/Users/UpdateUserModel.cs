﻿namespace TestApi.Domain.Models.Users
{
    public class UpdateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Preferences { get; set; }
    }
}
