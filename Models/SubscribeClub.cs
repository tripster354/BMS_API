using Microsoft.VisualBasic;
using System;

namespace BMS_API.Models
{
    public class SubscribeClub
    {
        public long ClubSubscriptionIDP { get; set; } 
        public long UserIDF { get; set; } 
        public long PartnerIDF { get; set; } 
        public long ClubIDF { get; set; } 
        public bool IsPaid { get; set; } 
        public DateTime SubscriptionDate { get; set; } 
        public bool IsDeleted { get; set; } 
        public long EntryBy { get; set; } 
        public DateTime EntryDate { get; set; } 
    }
}