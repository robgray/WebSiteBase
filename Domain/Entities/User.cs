using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Common;

namespace Domain.Entities
{
    public class User : IHasId
    {
        private IList<Role> _roles;

        public User()
        {
            _roles = new List<Role>();
        }

        public virtual int Id { get; protected set; }
        public virtual string Username { get; set; }
        public virtual string EmailAddress { get; set; }
		public virtual string SecurityQuestion { get; set; }		
		public virtual string SecurityAnswer { get; set; }
        public virtual DateTime DateJoined { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string IPAddress { get; set; }
		public virtual string ConfirmationToken { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }		
		public virtual string Password { get; set; }
		public virtual DateTime? PasswordChangedDate { get; set; }		
		public virtual string PasswordSalt { get; set; }		
		public virtual string PasswordVerificationToken { get; set; }
		public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }
        
	    /// <summary>
        /// Validates whether the supplied plain text password matches
        /// the password currently on record for this Customer
        /// </summary>
        public virtual bool IsPassword(ICryptographyProvider cryptographyProvider, string password)
        {
            if ((String.IsNullOrEmpty(password)) || (String.IsNullOrEmpty(Password)) || (String.IsNullOrEmpty(PasswordSalt)))
                return false;

            return Password.Equals(cryptographyProvider.GetHash(PasswordSalt, password));
        }

        public virtual bool ChangePassword(ICryptographyProvider cryptographyProvider, string newpass, string currentpass)
        {
            if (!IsPassword(cryptographyProvider, currentpass)) return false;
            SetPassword(cryptographyProvider, newpass);
            PasswordChangedDate = DateTimeHelper.Now;
            return true;
        }

        /// <summary>
        /// Changes this Customer's password to a new value in plain text
        /// </summary>
        public virtual void SetPassword(ICryptographyProvider cryptographyProvider, string plaintext)
        {
            if (String.IsNullOrEmpty(plaintext))
                return;

            if (String.IsNullOrEmpty(PasswordSalt))
                PasswordSalt = cryptographyProvider.GetRandomString(32);

            Password = cryptographyProvider.GetHash(PasswordSalt, plaintext);
        }

        public virtual void SetResetRequest(ICryptographyProvider cryptographyProvider)
        {
            PasswordVerificationToken = cryptographyProvider.GetRandomString(32);
            PasswordVerificationTokenExpirationDate = DateTimeHelper.Now.AddHours(24);
        }

        public virtual void NoPassword()
        {
            PasswordSalt = null;
            Password = null;
        }

        public virtual void Confirm()
        {
            IsConfirmed = true;
            DomainEventDispatcher.Raise(new UserRegistrationConfirmed {User = this});
        }

        public virtual IEnumerable<Role> Roles { get { return _roles; } }
    }
}