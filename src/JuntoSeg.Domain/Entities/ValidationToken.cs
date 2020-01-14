using JuntoSeg.Domain.Abstract;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Domain.Entities
{
    public class ValidationToken : AEntity, IEntity
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public TimeSpan ExpirationSpan { get; set; }
        public bool IsUsed { get; set; }

        public static ValidationToken GenerateToken()
        {
            string genToken = Guid.NewGuid().ToString();
            genToken = CreateHashString(genToken);

            TimeSpan expires = new TimeSpan(DateTime.UtcNow.AddHours(5).Ticks);

            return new ValidationToken()
            {
                IsUsed = false,
                Token = genToken,
                ExpirationSpan = expires
            };
        }
    }
}
