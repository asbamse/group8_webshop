﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamersUnited.Core.DomainService;
using GamersUnited.Core.Entities;
using GamersUnited.Infrastructure.Data.Context;

namespace GamersUnited.Infrastructure.Data
{
    public class UserRepository : IRepository<User>, ILoginValidation
    {
        private readonly GamersUnitedContext _ctx;

        public UserRepository(GamersUnitedContext context)
        {
            _ctx = context;
        }

        public int Count()
        {
            return _ctx.User.Count();
        }

        public User Add(User obj)
        {
            if (string.IsNullOrEmpty(obj.FirstName))
                throw new ArgumentException("First name cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.LastName))
                throw new ArgumentException("Last name cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Address))
                throw new ArgumentException("Address cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Email))
                throw new ArgumentException("Email cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Password))
                throw new ArgumentException("Password cannot be null or empty.");

            var tmp = new User
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Address = obj.Address,
                Email = obj.Email,
                PhoneNumber = obj.PhoneNumber,
                Password = obj.Password,
                IsAdmin = obj.IsAdmin
            };

            User item = _ctx.User.Add(tmp).Entity;
            _ctx.SaveChanges();

            return item;
        }

        public User GetById(int id)
        {
            var item = _ctx.User.FirstOrDefault(u => u.UserId == id);

            if (item == null)
            {
                throw new ArgumentOutOfRangeException("Id not found!");
            }

            return item;
        }

        public IList<User> GetAll()
        {
            return _ctx.User.ToList();
        }

        public User Update(int id, User obj)
        {
            if (string.IsNullOrEmpty(obj.FirstName))
                throw new ArgumentException("First name cannot be null or empty.");
            if (string.IsNullOrEmpty(obj.LastName))
                throw new ArgumentException("Last name cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Address))
                throw new ArgumentException("Address cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Email))
                throw new ArgumentException("Email cannot be null or empty.");
            else if (string.IsNullOrEmpty(obj.Password))
                throw new ArgumentException("Password cannot be null or empty.");

            var item = GetById(id);
            item.FirstName = obj.FirstName;
            item.LastName = obj.LastName;
            item.Address = obj.Address;
            item.Email = obj.Email;
            item.PhoneNumber = obj.PhoneNumber;
            item.Password = obj.Password;
            item.IsAdmin = obj.IsAdmin;

            _ctx.User.Update(item);
            _ctx.SaveChanges();

            return item;
        }

        public User Remove(User obj)
        {
            var item = GetById(obj.UserId);

            _ctx.User.Remove(item);
            _ctx.SaveChanges();

            return item;
        }

        public bool ValidateLoginInformation(string email, string password)
        {
            return _ctx.User.Where(u => (u.Email.ToLower().Equals(email.ToLower())) && (u.Password.Equals(password))).FirstOrDefault() != null;
        }
    }
}