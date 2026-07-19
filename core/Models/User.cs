using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static LibraryManagementSystem.Domain.Enums.UserRole;

namespace LibraryManagementSystem.Domain.Models
{
    public class User
    {
        private static int _idCounter = 0;

        public int Id { get; private set; }

        private string _userName = string.Empty;
        private string _email = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public decimal? Fee { get; private set; } = 0;

        public bool IsEmailVerified { get; private set; } = false;

        public string? VerificationCode { get; private set; }

        public DateTime? VerificationCodeExpiresAt { get; private set; }


        public string UserName
        {
            get => _userName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Username can't be empty!");

                if (value.Length < 3 || value.Length > 50)
                    throw new ArgumentException("User must enter min 3 and max 50 characters!");

                _userName = value;
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email can't be empty!");

                if (!value.Contains('@') || !value.Contains('.'))
                    throw new ArgumentException("Invalid email format!");

                _email = value;
            }
        }


        public Role Role { get; private set; } = Role.Client;


        public DateTime RegisterTime { get; private set; }


        // ახალი მომხმარებლის შექმნა
        public User(string name, string email, string password)
        {
            Id = ++_idCounter;

            UserName = name;
            Email = email;
            PasswordHash = password;

            Role = Role.Client;
            Fee = 0;

            RegisterTime = DateTime.UtcNow;
            IsEmailVerified = false;
        }


        // JSON-იდან აღდგენა
        [JsonConstructor]
        public User(
            int id,
            string userName,
            string email,
            string passwordHash,
            decimal? fee,
            bool isEmailVerified,
            string? verificationCode,
            DateTime? verificationCodeExpiresAt,
            Role role,
            DateTime registerTime)
        {
            Id = id;

            UserName = userName;
            Email = email;

            PasswordHash = passwordHash;

            Fee = fee;

            IsEmailVerified = isEmailVerified;
            VerificationCode = verificationCode;
            VerificationCodeExpiresAt = verificationCodeExpiresAt;

            Role = role;

            RegisterTime = registerTime;
        }



        public static void SyncIdCounter(List<User> users)
        {
            int maxId = users
                .Select(x => x.Id)
                .DefaultIfEmpty(0)
                .Max();

            if (maxId > _idCounter)
                _idCounter = maxId;
        }



        public void PromoteToAdmin()
        {
            Role = Role.Admin;
        }



        public void AddFee(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentException("Fee cannot be negative");

            Fee = (Fee ?? 0) + amount;
        }



        public void ClearFee()
        {
            Fee = 0;
        }



        public bool HasDebt()
        {
            return Fee > 0;
        }



        public void GenerateVerificationCode()
        {
            VerificationCode = Random.Shared
                .Next(100000, 999999)
                .ToString();


            VerificationCodeExpiresAt =
                DateTime.UtcNow.AddMinutes(15);


            IsEmailVerified = false;
        }



        public bool IsVerificationCodeValid(string code)
        {
            if (VerificationCode != code)
                return false;


            if (VerificationCodeExpiresAt < DateTime.UtcNow)
                return false;


            return true;
        }



        public void VerifyEmail()
        {
            IsEmailVerified = true;

            VerificationCode = null;

            VerificationCodeExpiresAt = null;
        }
    }
}